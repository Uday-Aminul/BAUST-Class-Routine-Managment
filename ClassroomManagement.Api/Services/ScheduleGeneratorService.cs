using System.ComponentModel;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Services
{
    public class ScheduleGeneratorService : IScheduleGeneratorService
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public static readonly DayOfWeek[] Days = new[]
        {
            DayOfWeek.Sunday,
            DayOfWeek.Monday,
            DayOfWeek.Tuesday,
            DayOfWeek.Wednesday,
            DayOfWeek.Thursday
        };

        public ScheduleGeneratorService(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<string>> GenerateAllSchedulesAsync()
        {
            var response = new List<string>();
            var levelTermSections = await _dbContext.LevelTermSections.Include(lt => lt.AssignedTeachers).ThenInclude(at => at.Teachers).ToListAsync();

            // Clearing existing schedules before generating new ones
            await _dbContext.ClassSchedules.ExecuteDeleteAsync();

            foreach (var levelTermSection in levelTermSections)
            {
                var sessionals075cr = await _dbContext.Sessionals.Include(s => s.Labrooms).Where(s => s.Level == levelTermSection.Level && s.Term == levelTermSection.Term && s.Credit == 0.75).ToListAsync();
                if (sessionals075cr.Count >= 2)
                {
                    var schedulesToAdd = new List<ClassSchedule>();
                    foreach (var day in Days)
                    {
                        //Before Tiffin Break
                        var twoSessionalsPlacedFirstSlot = await PlaceTwoSessionalsInSingleSlotAsync(levelTermSection, schedulesToAdd, sessionals075cr, day, new TimeOnly(8, 0), new TimeOnly(10, 50));
                        if (twoSessionalsPlacedFirstSlot)
                        {
                            await _dbContext.ClassSchedules.AddRangeAsync(schedulesToAdd);
                            await _dbContext.SaveChangesAsync();
                            schedulesToAdd.Clear();
                        }
                        if (sessionals075cr.Count < 2)
                        {
                            break;
                        }

                        //After Tiffin Break
                        var twoSessionalsPlacedSecondSlot = await PlaceTwoSessionalsInSingleSlotAsync(levelTermSection, schedulesToAdd, sessionals075cr, day, new TimeOnly(11, 30), new TimeOnly(14, 20));
                        if (twoSessionalsPlacedSecondSlot)
                        {
                            await _dbContext.ClassSchedules.AddRangeAsync(schedulesToAdd);
                            await _dbContext.SaveChangesAsync();
                            schedulesToAdd.Clear();
                        }
                        if (sessionals075cr.Count < 2)
                        {
                            break;
                        }
                    }
                }
            }

            foreach (var levelTermSection in levelTermSections)
            {
                var message = await GenerateScheduleAsync(levelTermSection.Level, levelTermSection.Term, levelTermSection.Section);
                response.Add(message);
            }
            return response;
        }

        private async Task<bool> PlaceTwoSessionalsInSingleSlotAsync(LevelTermSection levelTermSection, List<ClassSchedule> schedulesToAdd, List<Sessional> sessionals, DayOfWeek day, TimeOnly startTime, TimeOnly endTime)
        {
            var sessionalPlaced = 0;
            var EvenPlaced = false;
            var sessionalsToRemove = new List<Sessional>();
            var sessionalsCopy = sessionals.ToList(); // To avoid modifying the original list while iterating
            foreach (var sessional in sessionalsCopy)
            {
                var teachers = levelTermSection.AssignedTeachers.First(at => at.SessionalId == sessional.Id).Teachers.ToList();
                var allTeachersAvailable = await AreAllTeachersAvailable(teachers, startTime, endTime, day, schedulingState: null);
                var labroom = await FindAvailableLabroom(sessional.Labrooms, startTime, day, schedulingState: null);
                if (allTeachersAvailable is true && labroom is not null)
                {
                    var weekType = EvenPlaced ? "ODD" : "EVEN";
                    var schedule = new ClassSchedule
                    {
                        Day = day,
                        Level = levelTermSection.Level,
                        Term = levelTermSection.Term,
                        Section = levelTermSection.Section,
                        StartTime = startTime,
                        EndTime = endTime,
                        LabroomId = labroom.Id,
                        SessionalId = sessional.Id,
                        Teachers = teachers,
                        WeekType = weekType
                    };
                    schedulesToAdd.Add(schedule);
                    sessionalsToRemove.Add(sessional);
                    sessionalPlaced++;
                    EvenPlaced = !EvenPlaced;
                }
                if (sessionalPlaced == 2)
                {
                    foreach (var sessionalToRemove in sessionalsToRemove)
                    {
                        sessionals.Remove(sessionalToRemove);
                    }
                    return true; // Two sessionals placed successfully
                }
            }
            schedulesToAdd.Clear();
            return false;
        }

        private async Task<string> GenerateScheduleAsync(int level, int term, string section)
        {
            //getting classroom
            var levelTerm = await _dbContext.LevelTermSections.Include(lt => lt.Classrooms).FirstOrDefaultAsync(lt => lt.Level == level && lt.Term == term && lt.Section == section);
            if (levelTerm is null)
            {
                return $"Error: No level-term combination found for Level-{level} Term-{term}.";
            }
            var classrooms = levelTerm.Classrooms.ToList();

            //getting courses
            var courses = await _dbContext.Courses
                .Where(course => course.Level == level && course.Term == term)
                .ToListAsync();
            var totalCourseCredit = 0.0;
            foreach (var course in courses)
            {
                totalCourseCredit += course.Credit;
            }

            //getting sessionals
            var sessionals = await _dbContext.Sessionals
                .Where(s => s.Level == level && s.Term == term)
                .Include(s => s.Labrooms)
                .ToListAsync();
            var sessionalCount = sessionals.Count;

            if (totalCourseCredit is 0)
            {
                return $"No courses found for Level-{level} Term-{term}.";
            }
            else if (sessionalCount is 0)
            {
                return $"No sessionals found for Level-{level} Term-{term}.";
            }

            //Removing 0.75 sessionals that have already been scheduled
            var sessionalsCopy = sessionals.ToList(); // To avoid modifying the original list while iterating
            foreach (var sessional in sessionalsCopy)
            {
                if (sessional.Credit == 0.75)
                {
                    var sessionalScheduled = await _dbContext.ClassSchedules
                        .AnyAsync(cs => cs.Level == level &&
                        cs.Term == term &&
                        cs.Section == section &&
                        cs.SessionalId == sessional.Id);
                    if (sessionalScheduled is true)
                    {
                        sessionals.Remove(sessional);
                    }
                }
            }

            //Creating SchedulingState a backpack object 
            var schedulingState = new SchedulingState
            {
                Sessionals = sessionals.ToList(),
                Courses = courses.ToList(),
                Level = level,
                Term = term,
                Section = section,
                LabPlacedToday = 0,
                SchedulesToAdd = new List<ClassSchedule>(),
                DualLabPlacement = true,
                Classrooms = classrooms
            };

            //Actual Logic
            foreach (var day in Days)
            {
                schedulingState.LabPlacedToday = 0;
                var sessionalPlacedBeforeBreak = false;
                var placedTheoryCoursesBeforeBreak = new List<Course>();

                var isBusyBeforeBreak = await _dbContext.ClassSchedules
                    .AnyAsync(cs =>
                    cs.Day == day &&
                    cs.StartTime == new TimeOnly(8, 0) &&
                    // ((cs.CourseId != null && cs.Course.Level == level && cs.Course.Term == term) ||
                    // (cs.SessionalId != null && cs.Sessional.Level == level && cs.Sessional.Term == term)));
                    cs.Level == level && cs.Term == term && cs.Section == section);
                if (isBusyBeforeBreak is true)
                {
                    schedulingState.LabPlacedToday++;
                }

                if (schedulingState.Sessionals.Any() && isBusyBeforeBreak is false)
                {
                    sessionalPlacedBeforeBreak = await PlaceSessionalAsync(schedulingState, day, new TimeOnly(8, 0), new TimeOnly(10, 50));
                }
                //Placing theory in case lab couldn't be placed.
                if (sessionalPlacedBeforeBreak is false && isBusyBeforeBreak is false)
                {
                    var slots = new List<Slot>
                    {
                        new Slot { StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50) },
                        new Slot { StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 50) },
                        new Slot { StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50) }
                    };
                    placedTheoryCoursesBeforeBreak = await PlaceChainedTheoryAsync(schedulingState, day, slots, placedTheoryCoursesBeforeBreak);
                }

                //After Tiffin Break
                var isBusyAfterBreak = await _dbContext.ClassSchedules
                    .AnyAsync(cs =>
                    cs.Day == day &&
                    cs.StartTime == new TimeOnly(11, 30) &&
                    // ((cs.CourseId != null && cs.Course.Level == level && cs.Course.Term == term) ||
                    // (cs.SessionalId != null && cs.Sessional.Level == level && cs.Sessional.Term == term)));
                    cs.Level == level && cs.Term == term && cs.Section == section);
                if (isBusyAfterBreak is true)
                {
                    schedulingState.LabPlacedToday++;
                }

                var sessionalPlacedAfterBreak = false;
                if (sessionalPlacedBeforeBreak is true)
                {
                    if (schedulingState.DualLabPlacement is true && schedulingState.Sessionals.Any() && isBusyAfterBreak is false)
                    {
                        sessionalPlacedAfterBreak = await PlaceSessionalAsync(schedulingState, day, new TimeOnly(11, 30), new TimeOnly(14, 20));
                    }
                }
                else if (schedulingState.Sessionals.Any() && isBusyAfterBreak is false)
                {
                    sessionalPlacedAfterBreak = await PlaceSessionalAsync(schedulingState, day, new TimeOnly(11, 30), new TimeOnly(14, 20));
                }
                //Placing theory in case lab couldn't be placed.
                if (sessionalPlacedAfterBreak is false && isBusyAfterBreak is false)
                {
                    var slots = new List<Slot>
                    {
                        // Morning slots
                        // new Slot { StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50) },
                        // new Slot { StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 50) },
                        // new Slot { StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50) },
                        
                        // Afternoon slots
                        new Slot { StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20) },
                        new Slot { StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 20) },
                        new Slot { StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(14, 20) }
                    };
                    await PlaceChainedTheoryAsync(schedulingState, day, slots, placedTheoryCoursesBeforeBreak);
                }

                if (schedulingState.LabPlacedToday >= 2)
                {
                    schedulingState.DualLabPlacement = false;
                }
            }

            totalCourseCredit = schedulingState.Courses.Sum(c => c.Credit);
            foreach (var course in courses)
            {
                await _dbContext.Entry(course).ReloadAsync();  // Discard changes, get DB values
            }
            await _dbContext.ClassSchedules.AddRangeAsync(schedulingState.SchedulesToAdd);
            await _dbContext.SaveChangesAsync();


            if (totalCourseCredit is 0 && schedulingState.Sessionals.Count is 0)
            {
                return $"Schedule generation succesfull for Level-{level} Term-{term} Section-{section}.";
            }
            else if (totalCourseCredit is not 0 && schedulingState.Sessionals.Count is 0)
            {
                return $"Schedule generation partially succesfull for Level-{level} Term-{term} Section-{section}. Course credits left {totalCourseCredit}.";
            }
            else if (totalCourseCredit is 0 && schedulingState.Sessionals.Count is not 0)
            {
                return $"Schedule generation partially succesfull for Level-{level} Term-{term} Section-{section}. Sessionals left {schedulingState.Sessionals.Count}.";
            }
            else
            {
                return $"Schedule generation partially succesfull for Level-{level} Term-{term} Section-{section}. Course credits left {totalCourseCredit}. Sessionals left {schedulingState.Sessionals.Count}.";
            }
            //return $"Some error occurred during generating schedules for Level-{level} Term-{term} Section-{section}.";
        }

        //Placing a theory
        // private async Task<List<Course>> PlaceTheoryAsync(
        //     SchedulingState schedulingState,
        //     DayOfWeek day,
        //     TimeOnly startTime1, TimeOnly endTime1,
        //     TimeOnly startTime2, TimeOnly endTime2,
        //     TimeOnly startTime3, TimeOnly endTime3,
        //     List<Course> coursesPlacedBeforeBreak)
        // {
        //     var slots = new[]{
        //         new { Start = startTime1, End = endTime1 },
        //         new { Start = startTime2, End = endTime2 },
        //         new { Start = startTime3, End = endTime3 }
        //         };

        //     // For Modifying the list while iterating
        //     var courseToConsider = schedulingState.Courses.Where(c => c.Credit > 0).ToList();

        //     if (coursesPlacedBeforeBreak != null && coursesPlacedBeforeBreak.Any())
        //     {
        //         courseToConsider = courseToConsider
        //             .Where(c => !coursesPlacedBeforeBreak.Any(pc => pc.Id == c.Id))
        //             .OrderByDescending(c => c.Credit)  // Sort from highest credit to lowest
        //             .ToList();
        //     }
        //     var placedCourses = new List<Course>();
        //     var levelTermSection = await _dbContext.LevelTermSections
        //         .Include(lt => lt.AssignedTeachers)
        //         .ThenInclude(at => at.Teachers)
        //         .FirstOrDefaultAsync(lt =>
        //             lt.Level == schedulingState.Level &&
        //             lt.Term == schedulingState.Term &&
        //             lt.Section == schedulingState.Section);
        //     foreach (var slot in slots)
        //     {
        //         var coursesCopy = courseToConsider
        //             .OrderByDescending(c => c.Credit)  // Sort from highest credit to lowest
        //             .ToList(); // Refreshing the copy for each slot
        //         foreach (var course in coursesCopy)
        //         {
        //             //Error Debug
        //             var teacher = levelTermSection.AssignedTeachers.FirstOrDefault(at => at.CourseId == course.Id).Teachers.FirstOrDefault();
        //             var teacherAvailable = await IsTeacherAvailable(teacher.Id, slot.Start, slot.End, day);
        //             var availableClassroom = await FindAvailableClassroom(schedulingState.Classrooms, slot.Start, day);
        //             if (teacherAvailable is true && availableClassroom is not null)
        //             {
        //                 var schedule = new ClassSchedule
        //                 {
        //                     Day = day,
        //                     StartTime = slot.Start,
        //                     EndTime = slot.End,
        //                     Level = schedulingState.Level,
        //                     Term = schedulingState.Term,
        //                     Section = schedulingState.Section,
        //                     ClassroomId = availableClassroom.Id,
        //                     CourseId = course.Id,
        //                     Teachers = new List<Teacher> { teacher }
        //                 };
        //                 schedulingState.SchedulesToAdd.Add(schedule);
        //                 courseToConsider.Remove(course);
        //                 placedCourses.Add(course);
        //                 schedulingState.Courses.FirstOrDefault(c => c.Id == course.Id).Credit--;
        //                 break; // Done assigning a course to this slot.
        //             }
        //             else
        //             {
        //                 var swapPerformed = false;
        //                 var courseSchedules = schedulingState.SchedulesToAdd.Where(s => s.Day == day && s.CourseId is not null).ToList(); // To avoid modifying the original list while iterating
        //                 foreach (var courseSchedule in courseSchedules)
        //                 {
        //                     teacherAvailable = await IsTeacherAvailable(teacher.Id, courseSchedule.StartTime, courseSchedule.EndTime, day);
        //                     availableClassroom = await FindAvailableClassroom(schedulingState.Classrooms, courseSchedule.StartTime, day);
        //                     if (teacherAvailable is true && availableClassroom is not null)
        //                     {
        //                         var teacherAvailableToExchange = await IsTeacherAvailable(courseSchedule.Teachers.First().Id, slot.Start, slot.End, day);
        //                         var classroomAvailableToExchange = await FindAvailableClassroom(schedulingState.Classrooms, slot.Start, day);
        //                         if (teacherAvailableToExchange is true && classroomAvailableToExchange is not null)
        //                         {
        //                             //Exchange
        //                             var schedule = new ClassSchedule
        //                             {
        //                                 Day = day,
        //                                 StartTime = slot.Start,
        //                                 EndTime = slot.End,
        //                                 Level = schedulingState.Level,
        //                                 Term = schedulingState.Term,
        //                                 Section = schedulingState.Section,
        //                                 ClassroomId = classroomAvailableToExchange.Id,
        //                                 CourseId = courseSchedule.CourseId,
        //                                 Teachers = courseSchedule.Teachers
        //                             };
        //                             schedulingState.SchedulesToAdd.Add(schedule);
        //                             courseSchedule.ClassroomId = availableClassroom.Id;
        //                             courseSchedule.CourseId = course.Id;
        //                             courseSchedule.Teachers = new List<Teacher> { teacher };

        //                             courseToConsider.Remove(course);
        //                             placedCourses.Add(course);
        //                             schedulingState.Courses.FirstOrDefault(c => c.Id == course.Id).Credit--;
        //                             swapPerformed = true;
        //                             break; // Done assigning a course to this slot.
        //                         }
        //                     }
        //                 }
        //                 if (swapPerformed is true)
        //                 {
        //                     break; // Move to the next slot after a successful swap and placement
        //                 }
        //             }


        //             //Manual 2step hopping
        //             // else
        //             // {
        //             //     var swapPerformed = false;
        //             //     var courseSchedules = schedulingState.SchedulesToAdd.Where(s => s.Day == day && s.CourseId is not null).ToList(); // To avoid modifying the original list while iterating
        //             //     foreach (var courseSchedule in courseSchedules)
        //             //     {
        //             //         teacherAvailable = await IsTeacherAvailable(teacher.Id, courseSchedule.StartTime, courseSchedule.EndTime, day);
        //             //         availableClassroom = await FindAvailableClassroom(schedulingState.Classrooms, courseSchedule.StartTime, day);
        //             //         if (teacherAvailable is true && availableClassroom is not null)
        //             //         {
        //             //             var teacherAvailableToExchange = await IsTeacherAvailable(courseSchedule.Teachers.First().Id, slot.Start, slot.End, day);
        //             //             var classroomAvailableToExchange = await FindAvailableClassroom(schedulingState.Classrooms, slot.Start, day);
        //             //             if (teacherAvailableToExchange is true && classroomAvailableToExchange is not null)
        //             //             {
        //             //                 //Exchange
        //             //                 var schedule = new ClassSchedule
        //             //                 {
        //             //                     Day = day,
        //             //                     StartTime = slot.Start,
        //             //                     EndTime = slot.End,
        //             //                     Level = schedulingState.Level,
        //             //                     Term = schedulingState.Term,
        //             //                     Section = schedulingState.Section,
        //             //                     ClassroomId = classroomAvailableToExchange.Id,
        //             //                     CourseId = courseSchedule.CourseId,
        //             //                     Teachers = courseSchedule.Teachers
        //             //                 };
        //             //                 schedulingState.SchedulesToAdd.Add(schedule);
        //             //                 courseSchedule.ClassroomId = availableClassroom.Id;
        //             //                 courseSchedule.CourseId = course.Id;
        //             //                 courseSchedule.Teachers = new List<Teacher> { teacher };

        //             //                 courseToConsider.Remove(course);
        //             //                 placedCourses.Add(course);
        //             //                 schedulingState.Courses.FirstOrDefault(c => c.Id == course.Id).Credit--;
        //             //                 swapPerformed = true;
        //             //                 break; // Done assigning a course to this slot.
        //             //             }
        //             //             else
        //             //             {
        //             //                 foreach (var courseSchedule1 in courseSchedules)
        //             //                 {
        //             //                     var teacherAvailable1 = await IsTeacherAvailable(courseSchedule.Teachers.First().Id, courseSchedule1.StartTime, courseSchedule1.EndTime, day);
        //             //                     var availableClassroom1 = await FindAvailableClassroom(schedulingState.Classrooms, courseSchedule1.StartTime, day);
        //             //                     if (teacherAvailable1 is true && availableClassroom1 is not null)
        //             //                     {
        //             //                         var teacherAvailableToExchange1 = await IsTeacherAvailable(courseSchedule1.Teachers.First().Id, slot.Start, slot.End, day);
        //             //                         var classroomAvailableToExchange1 = await FindAvailableClassroom(schedulingState.Classrooms, slot.Start, day);
        //             //                         if (teacherAvailableToExchange1 is true && classroomAvailableToExchange1 is not null)
        //             //                         {
        //             //                             //One Hop Exchange
        //             //                             var schedule = new ClassSchedule
        //             //                             {
        //             //                                 Day = day,
        //             //                                 StartTime = slot.Start,
        //             //                                 EndTime = slot.End,
        //             //                                 Level = schedulingState.Level,
        //             //                                 Term = schedulingState.Term,
        //             //                                 Section = schedulingState.Section,
        //             //                                 ClassroomId = classroomAvailableToExchange1.Id,
        //             //                                 CourseId = courseSchedule1.CourseId,
        //             //                                 Teachers = courseSchedule1.Teachers
        //             //                             };
        //             //                             schedulingState.SchedulesToAdd.Add(schedule);
        //             //                             courseSchedule1.ClassroomId = availableClassroom1.Id;
        //             //                             courseSchedule1.CourseId = courseSchedule.Course.Id;
        //             //                             courseSchedule1.Teachers = courseSchedule.Teachers;

        //             //                             courseSchedule.ClassroomId = availableClassroom.Id;
        //             //                             courseSchedule.CourseId = course.Id;
        //             //                             courseSchedule.Teachers = new List<Teacher> { teacher };

        //             //                             courseToConsider.Remove(course);
        //             //                             placedCourses.Add(course);
        //             //                             schedulingState.Courses.FirstOrDefault(c => c.Id == course.Id).Credit--;
        //             //                             swapPerformed = true;
        //             //                             break; // Done assigning a course to this slot.
        //             //                         }
        //             //                     }
        //             //                 }
        //             //             }
        //             //         }
        //             //         if (swapPerformed is true)
        //             //         {
        //             //             break; // Move to the next slot after a successful swap and placement
        //             //         }
        //             //     }
        //             // }
        //         }

        //     }
        //     return placedCourses;
        // }

        //Placing a theory with a chain reaction
        private async Task<List<Course>> PlaceChainedTheoryAsync(
            SchedulingState schedulingState,
            DayOfWeek day,
            List<Slot> slots,
            List<Course> coursesPlacedBeforeBreak)
        {
            // For Modifying the list while iterating
            var courseToConsider = schedulingState.Courses.Where(c => c.Credit > 0).ToList();

            if (coursesPlacedBeforeBreak != null && coursesPlacedBeforeBreak.Any())
            {
                courseToConsider = courseToConsider
                    .Where(c => !coursesPlacedBeforeBreak.Any(pc => pc.Id == c.Id))
                    .OrderByDescending(c => c.Credit)  // Sort from highest credit to lowest
                    .ToList();
            }
            else
            {
                courseToConsider = courseToConsider
                    .OrderByDescending(c => c.Credit)  // Sort from highest credit to lowest
                    .ToList();
            }

            var placedCourses = new List<Course>();
            var levelTermSection = await _dbContext.LevelTermSections
                .Include(lt => lt.AssignedTeachers)
                .ThenInclude(at => at.Teachers)
                .FirstOrDefaultAsync(lt =>
                    lt.Level == schedulingState.Level &&
                    lt.Term == schedulingState.Term &&
                    lt.Section == schedulingState.Section);

            foreach (var slot in slots)
            {
                var coursesCopy = courseToConsider
                    .OrderByDescending(c => c.Credit)  // Sort from highest credit to lowest
                    .ToList(); // Refreshing the copy for each slot
                foreach (var course in coursesCopy) // Create a copy to avoid modifying the collection during iteration
                {
                    var teacher = levelTermSection.AssignedTeachers.FirstOrDefault(at => at.CourseId == course.Id).Teachers.FirstOrDefault();
                    var visitedStartTimes = new HashSet<TimeOnly>();
                    var placed = await TryPlaceCourseInSlotAsync(course, teacher, slot.StartTime, slot.EndTime, visitedStartTimes, schedulingState, day, placedCourses, courseToConsider);
                    if (placed)
                    {
                        break; // Move to the next slot after a successful placement
                    }
                }

                //Re arrange based on course credit left
                courseToConsider = courseToConsider
                    .OrderByDescending(c => c.Credit)  // Sort from highest credit to lowest
                    .ToList();
            }
            return placedCourses;
        }

        //Second method for the chaining
        private async Task<bool> TryPlaceCourseInSlotAsync(Course course,
        Teacher teacher,
        TimeOnly slotStart,
        TimeOnly slotEnd,
        HashSet<TimeOnly> visitedStartTimes,
        SchedulingState schedulingState,
        DayOfWeek day,
        List<Course> placedCourses,
        List<Course> courseToConsider,
        bool isRelocation = false)
        {
            if (visitedStartTimes.Contains(slotStart))
            {
                return false; // Already tried this slot, avoid infinite loops
            }
            visitedStartTimes.Add(slotStart);

            var teacherAvailable = await IsTeacherAvailable(teacher.Id, slotStart, slotEnd, day, schedulingState);
            var availableClassroom = await FindAvailableClassroom(schedulingState.Classrooms, slotStart, day, schedulingState);

            if (teacherAvailable is true && availableClassroom is not null)
            {
                var schedule = new ClassSchedule
                {
                    Day = day,
                    StartTime = slotStart,
                    EndTime = slotEnd,
                    Level = schedulingState.Level,
                    Term = schedulingState.Term,
                    Section = schedulingState.Section,
                    ClassroomId = availableClassroom.Id,
                    CourseId = course.Id,
                    Teachers = new List<Teacher> { teacher }
                };
                schedulingState.SchedulesToAdd.Add(schedule);

                if (isRelocation is false)
                {
                    placedCourses.Add(course);
                    courseToConsider.Remove(course);
                    schedulingState.Courses.FirstOrDefault(c => c.Id == course.Id).Credit--;
                }

                return true;
            }

            // Direct placement failed — try relocating an already-placed course to free up a slot for this one
            var occupiedSchedules = schedulingState.SchedulesToAdd
                .Where(s => s.Day == day && s.CourseId is not null && !visitedStartTimes.Contains(s.StartTime))
                .ToList();

            foreach (var occupantSchedule in occupiedSchedules)
            {
                teacherAvailable = await IsTeacherAvailable(teacher.Id, occupantSchedule.StartTime, occupantSchedule.EndTime, day, schedulingState);
                availableClassroom = await FindAvailableClassroom(schedulingState.Classrooms, occupantSchedule.StartTime, day, schedulingState);
                if (teacherAvailable is false || availableClassroom is null)
                {
                    continue; // Can't move this occupant, try the next one
                }

                var occupantCourse = schedulingState.Courses.FirstOrDefault(c => c.Id == occupantSchedule.CourseId);
                var occupantTeacher = occupantSchedule.Teachers.FirstOrDefault();
                if (occupantCourse is null || occupantTeacher is null)
                    continue;

                if (await TryPlaceCourseInSlotAsync(occupantCourse, occupantTeacher, slotStart, slotEnd, visitedStartTimes, schedulingState, day, placedCourses, courseToConsider, isRelocation: true))
                {
                    // Occupant successfully relocated (possibly several hops deep) — its old slot is now free
                    occupantSchedule.CourseId = course.Id;
                    occupantSchedule.Teachers = new List<Teacher> { teacher };
                    occupantSchedule.ClassroomId = availableClassroom.Id;
                    return true;
                }
                //Relocation failed, continue to the next occupant
            }
            return false; //Could not place the course in this slot, even after trying to relocate others
        }

        //Placing a lab
        private async Task<bool> PlaceSessionalAsync(
            SchedulingState schedulingState,
            DayOfWeek day,
            TimeOnly startTime,
            TimeOnly endTime)
        {
            var levelTermSection = await
                        _dbContext.LevelTermSections
                        .Include(lt => lt.AssignedTeachers)
                        .ThenInclude(at => at.Teachers)
                        .FirstOrDefaultAsync(lt =>
                            lt.Level == schedulingState.Level &&
                            lt.Term == schedulingState.Term &&
                            lt.Section == schedulingState.Section);
            //Ordered by Ascending
            var sessionalsCopy = schedulingState.Sessionals.OrderBy(s => s.Credit).ToList(); // To avoid modifying the original list while iterating
            foreach (var sessional in sessionalsCopy)
            {
                var teachers = levelTermSection.AssignedTeachers.FirstOrDefault(at => at.SessionalId == sessional.Id).Teachers.ToList();
                var allTeachersAvailable = await AreAllTeachersAvailable(teachers, startTime, endTime, day, schedulingState);
                Labroom labroom = null;
                bool? evenAvailability = null;
                //Verifying for 0.75credit labs
                if (sessional.Credit == 0.75)
                {
                    (labroom, evenAvailability) = await FindAvailableLabroomFor075CreditLab(sessional.Labrooms, startTime, day);
                    if (allTeachersAvailable is true && labroom is not null)
                    {
                        var schedule = new ClassSchedule
                        {
                            Day = day,
                            StartTime = startTime,
                            EndTime = endTime,
                            Level = schedulingState.Level,
                            Term = schedulingState.Term,
                            Section = schedulingState.Section,
                            WeekType = evenAvailability.Value ? "EVEN" : "ODD",
                            LabroomId = labroom.Id,
                            SessionalId = sessional.Id,
                            Teachers = teachers,
                        };
                        schedulingState.SchedulesToAdd.Add(schedule);
                        schedulingState.Sessionals.Remove(sessional);
                        schedulingState.LabPlacedToday++;
                        return true; // Lab placed successfully
                    }
                    else if (allTeachersAvailable is true && labroom is null)
                    {
                        labroom = await FindAvailableLabroom(sessional.Labrooms, startTime, day, schedulingState);
                        if (labroom is not null)
                        {
                            var schedule = new ClassSchedule
                            {
                                Day = day,
                                StartTime = startTime,
                                EndTime = endTime,
                                Level = schedulingState.Level,
                                Term = schedulingState.Term,
                                Section = schedulingState.Section,
                                WeekType = "EVEN",
                                LabroomId = labroom.Id,
                                SessionalId = sessional.Id,
                                Teachers = teachers,
                            };
                            schedulingState.SchedulesToAdd.Add(schedule);
                            schedulingState.Sessionals.Remove(sessional);
                            schedulingState.LabPlacedToday++;
                            return true; // Lab placed successfully
                        }
                    }
                }
                if (sessional.Credit == 1.5)
                {
                    labroom = await FindAvailableLabroom(sessional.Labrooms, startTime, day, schedulingState);
                    if (allTeachersAvailable is true && labroom is not null)
                    {
                        var schedule = new ClassSchedule
                        {
                            Day = day,
                            StartTime = startTime,
                            EndTime = endTime,
                            Level = schedulingState.Level,
                            Term = schedulingState.Term,
                            Section = schedulingState.Section,
                            LabroomId = labroom.Id,
                            SessionalId = sessional.Id,
                            Teachers = teachers,
                        };
                        schedulingState.SchedulesToAdd.Add(schedule);
                        schedulingState.Sessionals.Remove(sessional);
                        schedulingState.LabPlacedToday++;
                        return true; // Lab placed successfully
                    }
                }
            }
            return false; // No suitable sessional found for this time slot
        }

        //Checks a list of teachers availability
        private async Task<bool> AreAllTeachersAvailable(List<Teacher> teachers, TimeOnly startTime, TimeOnly endTime, DayOfWeek day, SchedulingState schedulingState)
        {
            foreach (var teacher in teachers)
            {
                var teacherAvailability = await IsTeacherAvailable(teacher.Id, startTime, endTime, day, schedulingState);
                if (teacherAvailability is false)
                {
                    return false; // At least one teacher is not available
                }
            }
            return true; // All teachers are available
        }

        //Find if any of the classroom is available
        private async Task<Classroom?> FindAvailableClassroom(List<Classroom> classrooms, TimeOnly startTime, DayOfWeek day, SchedulingState schedulingState)
        {
            foreach (var classroom in classrooms)
            {
                var classroomAvailability = await IsClassroomAvailable(classroom.Id, startTime, day, schedulingState);
                if (classroomAvailability is true)
                {
                    return classroom; // Found an available labroom
                }
            }
            return null; // No labrooms are available
        }

        //Find if any of the labrooms is available
        private async Task<Labroom?> FindAvailableLabroom(List<Labroom> labrooms, TimeOnly startTime, DayOfWeek day, SchedulingState schedulingState)
        {
            foreach (var labroom in labrooms)
            {
                var labroomAvailability = await IsLabroomAvailable(labroom.Id, startTime, day, schedulingState);
                if (labroomAvailability is true)
                {
                    return labroom; // Found an available labroom
                }
            }
            return null; // No labrooms are available
        }

        //Find if any of the labrooms is available for 0.75 credit lab
        private async Task<(Labroom? labroom, bool evenAvailability)> FindAvailableLabroomFor075CreditLab(List<Labroom> labrooms, TimeOnly startTime, DayOfWeek day)
        {
            foreach (var labroom in labrooms)
            {
                var (labroomAvailability, evenAvailability) = await IsLabroomAvailableFor075CreditLab(labroom.Id, startTime, day);
                if (labroomAvailability is true)
                {
                    return (labroom, evenAvailability); // Found an available labroom
                }
            }
            return (null, false); // No labrooms are available
        }

        //Checks if the teacher is available for the given time slot and day.
        private async Task<bool> IsTeacherAvailable(int teacherId, TimeOnly startTime, TimeOnly endTime, DayOfWeek day, SchedulingState schedulingState)
        {
            var overlappingSchedules = await _dbContext.ClassSchedules
                .Where(cs =>
                    cs.Day == day
                    && (cs.StartTime < endTime)
                    && (cs.EndTime > startTime))
                    .Where(cs => cs.Teachers.Any(t => t.Id == teacherId))
                    .AnyAsync();

            //For Chained method
            if (schedulingState is not null)
            {
                overlappingSchedules = overlappingSchedules || schedulingState.SchedulesToAdd
                .Any(cs =>
                    cs.Day == day
                    && (cs.StartTime < endTime)
                    && (cs.EndTime > startTime)
                    && cs.Teachers.Any(t => t.Id == teacherId));
            }

            if (overlappingSchedules)
            {
                return false; // Teacher is not available
            }
            return true; // Teacher is available
        }

        //Checks if the labroom is available for the given time slot and day.
        private async Task<bool> IsClassroomAvailable(int classroomId, TimeOnly startTime, DayOfWeek day, SchedulingState schedulingState)
        {
            var isBooked = await _dbContext.ClassSchedules
                .AnyAsync(cs =>
                cs.ClassroomId == classroomId
                && cs.Day == day
                && cs.StartTime == startTime);

            //For Chained method
            isBooked = isBooked || schedulingState.SchedulesToAdd
                .Any(cs =>
                    cs.ClassroomId == classroomId
                    && cs.Day == day
                    && cs.StartTime == startTime);

            if (isBooked)
            {
                return false; // Labroom is not available
            }
            return true; // Labroom is available
        }

        //Checks if the labroom is available for the given time slot and day.
        private async Task<bool> IsLabroomAvailable(int labroomId, TimeOnly startTime, DayOfWeek day, SchedulingState schedulingState)
        {
            var isBooked = await _dbContext.ClassSchedules
                .AnyAsync(cs =>
                cs.LabroomId == labroomId
                && cs.Day == day
                && cs.StartTime == startTime);

            //For Chained method
            if (schedulingState is not null)
            {
                isBooked = isBooked || schedulingState.SchedulesToAdd
                .Any(cs =>
                    cs.LabroomId == labroomId
                    && cs.Day == day
                    && cs.StartTime == startTime);
            }

            if (isBooked)
            {
                return false; // Labroom is not available
            }
            return true; // Labroom is available
        }

        //Checks if the labroom is available for the given time slot and day for 0.75 credit lab.
        private async Task<(bool labroomAvailability, bool evenAvailability)> IsLabroomAvailableFor075CreditLab(int labroomId, TimeOnly startTime, DayOfWeek day)
        {
            var bookedSchedules = await _dbContext.ClassSchedules
                .Where(cs =>
                cs.LabroomId == labroomId
                && cs.Day == day
                && cs.StartTime == startTime
                && cs.Sessional != null
                && cs.Sessional.Credit == 0.75).ToListAsync();
            if (bookedSchedules.Count == 1)
            {
                var labroomAvailability = true;
                var evenAvailability = bookedSchedules.First().WeekType == "ODD" ? true : false;
                return (labroomAvailability, evenAvailability); // Labroom is available
            }
            return (false, false); // Labroom is not available
        }

        //Clears Existing Schedules for the given level and term.
        // private async Task ClearExistingSchedules(int level, int term)
        // {
        //     var courseIds = await _dbContext.Courses
        //     .Where(c => c.Level == level && c.Term == term)
        //     .Select(c => c.Id)
        //     .ToListAsync();
        //     var sessionalIds = await _dbContext.Sessionals
        //     .Where(s => s.Level == level && s.Term == term)
        //     .Select(s => s.Id)
        //     .ToListAsync();

        //     var existingSchedules = await _dbContext.ClassSchedules.Where(cs => (cs.CourseId != null && courseIds.Contains(cs.CourseId.Value))
        //         || (cs.SessionalId != null && sessionalIds.Contains(cs.SessionalId.Value)))
        //         .ToListAsync();

        //     if (existingSchedules.Any())
        //     {
        //         _dbContext.ClassSchedules.RemoveRange(existingSchedules);
        //         await _dbContext.SaveChangesAsync();
        //     }
        // }
    }
}