using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Services
{
    public class ScheduleGeneratorService : IScheduleGeneratorService
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public ScheduleGeneratorService(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateScheduleAsync(int level, int term)
        {
            //getting classroom
            var levelTerm = await _dbContext.LevelTerms.FirstOrDefaultAsync(lt => lt.Level == level && lt.Term == term);
            if (levelTerm is null)
            {
                return $"Error: No level-term combination found for Level-{level} Term-{term}.";
            }
            var classroom = levelTerm.ClassroomId;

            //getting courses
            var courses = await _dbContext.Courses
                .Where(course => course.Level == level && course.Term == term)
                .Include(c => c.Teacher)
                .ToListAsync();
            var totalCourseCredit = 0.0;
            foreach (var course in courses)
            {
                totalCourseCredit += course.Credit;
            }

            //getting sessionals
            var sessionals = await _dbContext.Sessionals
                .Where(s => s.Level == level && s.Term == term)
                .Include(s => s.Teachers)
                .Include(s => s.Labrooms)
                .ToListAsync();
            var sessionalCount = sessionals.Count;

            if (totalCourseCredit is 0 || sessionalCount is 0)
            {
                return $"No courses or sessionals found for Level-{level} Term-{term}.";
            }

            //Creating SchedulingState a backpack object 
            var schedulingState = new SchedulingState
            {
                Sessionals = sessionals.ToList(),
                Courses = courses.ToList(),
                LabPlacedToday = 0,
                SchedulesToAdd = new List<ClassSchedule>(),
                DualLabPlacement = true,
                Classroom = classroom
            };

            //Clearing existing schedules for the level and term before generating new ones.
            await ClearExistingSchedules(level, term);

            //Actual Logic
            var days = new[]
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday
            };

            foreach (var day in days)
            {
                schedulingState.LabPlacedToday = 0;
                var sessionalPlacedBeforeBreak = false;
                var placedTheoryCoursesBeforeBreak = new List<Course>();

                if (schedulingState.Sessionals.Any())
                {
                    sessionalPlacedBeforeBreak = await PlaceSessionalAsync(schedulingState, day, new TimeOnly(8, 0), new TimeOnly(10, 50));
                }
                //Placing theory in case lab couldn't be placed.
                if (sessionalPlacedBeforeBreak is false)
                {
                    placedTheoryCoursesBeforeBreak = await PlaceTheoryAsync(schedulingState, day, new TimeOnly(8, 0), new TimeOnly(8, 50), new TimeOnly(9, 0), new TimeOnly(9, 50), new TimeOnly(10, 0), new TimeOnly(10, 50), placedTheoryCoursesBeforeBreak);
                }

                //After Tiffin Break
                var sessionalPlacedAfterBreak = false;
                if (sessionalPlacedBeforeBreak is true)
                {
                    if (schedulingState.DualLabPlacement is true && schedulingState.Sessionals.Any())
                    {
                        sessionalPlacedAfterBreak = await PlaceSessionalAsync(schedulingState, day, new TimeOnly(11, 30), new TimeOnly(14, 20));
                    }
                }
                else
                {
                    sessionalPlacedAfterBreak = await PlaceSessionalAsync(schedulingState, day, new TimeOnly(11, 30), new TimeOnly(14, 20));
                }
                //Placing theory in case lab couldn't be placed.
                if (sessionalPlacedAfterBreak is false)
                {
                    await PlaceTheoryAsync(schedulingState, day, new TimeOnly(11, 30), new TimeOnly(12, 20), new TimeOnly(12, 30), new TimeOnly(13, 20), new TimeOnly(13, 30), new TimeOnly(14, 20), placedTheoryCoursesBeforeBreak);
                }

                if (schedulingState.LabPlacedToday >= 2)
                {
                    schedulingState.DualLabPlacement = false;
                }
            }

            foreach (var course in courses)
            {
                await _dbContext.Entry(course).ReloadAsync();  // Discard changes, get DB values
            }
            await _dbContext.ClassSchedules.AddRangeAsync(schedulingState.SchedulesToAdd);
            await _dbContext.SaveChangesAsync();

            totalCourseCredit = schedulingState.Courses.Sum(c => c.Credit);
            if (totalCourseCredit is 0 && schedulingState.Sessionals.Count is 0)
            {
                return $"Schedule generation succesfull for Level-{level} Term-{term}";
            }
            return $"Some error occurred during generating schedules for Level-{level} Term-{term}";
        }

        //Placing a theory
        private async Task<List<Course>> PlaceTheoryAsync(
            SchedulingState schedulingState,
            DayOfWeek day,
            TimeOnly startTime1, TimeOnly endTime1,
            TimeOnly startTime2, TimeOnly endTime2,
            TimeOnly startTime3, TimeOnly endTime3,
            List<Course> coursesPlacedBeforeBreak)
        {
            var slots = new[]{
                new { Start = startTime1, End = endTime1 },
                new { Start = startTime2, End = endTime2 },
                new { Start = startTime3, End = endTime3 }
                };
            var courseToConsider = schedulingState.Courses.Where(c => c.Credit > 0).ToList(); // For Modifying the list while iterating
            if (coursesPlacedBeforeBreak != null && coursesPlacedBeforeBreak.Any())
            {
                courseToConsider = courseToConsider
                    .Where(c => !coursesPlacedBeforeBreak.Any(pc => pc.Id == c.Id))
                    .ToList();
            }
            var placedCourses = new List<Course>();

            foreach (var slot in slots)
            {
                var coursesCopy = courseToConsider.ToList(); // Refreshing the copy for each slot
                foreach (var course in coursesCopy)
                {
                    var teacher = course.Teacher;
                    var teacherAvailable = await IsTeacherAvailable(teacher.Id, slot.Start, slot.End, day);
                    if (teacherAvailable is true)
                    {
                        var schedule = new ClassSchedule
                        {
                            Day = day,
                            StartTime = slot.Start,
                            EndTime = slot.End,
                            ClassroomId = schedulingState.Classroom,
                            CourseId = course.Id,
                            Teachers = new List<Teacher> { teacher }
                        };
                        schedulingState.SchedulesToAdd.Add(schedule);
                        courseToConsider.Remove(course);
                        placedCourses.Add(course);
                        schedulingState.Courses.FirstOrDefault(c => c.Id == course.Id).Credit--;
                        break; // Done assigning a course to this slot.
                    }
                }
            }
            return placedCourses;
        }

        //Placing a lab
        private async Task<bool> PlaceSessionalAsync(
            SchedulingState schedulingState,
            DayOfWeek day,
            TimeOnly startTime,
            TimeOnly endTime)
        {
            var sessionalsCopy = schedulingState.Sessionals.ToList(); // To avoid modifying the original list while iterating
            foreach (var sessional in sessionalsCopy)
            {
                var teachers = sessional.Teachers.ToList();
                var allTeachersAvailable = await AreAllTeachersAvailable(teachers, startTime, endTime, day);
                var labroom = await FindAvailableLabroom(sessional.Labrooms, startTime, day);
                if (allTeachersAvailable is true && labroom is not null)
                {
                    var schedule = new ClassSchedule
                    {
                        Day = day,
                        StartTime = startTime,
                        EndTime = endTime,
                        LabroomId = labroom.Id,
                        SessionalId = sessional.Id,
                        Teachers = sessional.Teachers,
                    };
                    schedulingState.SchedulesToAdd.Add(schedule);
                    schedulingState.Sessionals.Remove(sessional);
                    schedulingState.LabPlacedToday++;
                    return true; // Lab placed successfully
                }
            }
            return false; // No suitable sessional found for this time slot
        }

        //Checks a list of teachers availability
        private async Task<bool> AreAllTeachersAvailable(List<Teacher> teachers, TimeOnly startTime, TimeOnly endTime, DayOfWeek day)
        {
            foreach (var teacher in teachers)
            {
                var teacherAvailability = await IsTeacherAvailable(teacher.Id, startTime, endTime, day);
                if (teacherAvailability is false)
                {
                    return false; // At least one teacher is not available
                }
            }
            return true; // All teachers are available
        }

        //Find if any of the labrooms is available
        private async Task<Labroom?> FindAvailableLabroom(List<Labroom> labrooms, TimeOnly startTime, DayOfWeek day)
        {
            foreach (var labroom in labrooms)
            {
                var labroomAvailability = await IsLabroomAvailable(labroom.Id, startTime, day);
                if (labroomAvailability is true)
                {
                    return labroom; // Found an available labroom
                }
            }
            return null; // No labrooms are available
        }

        //Checks if the teacher is available for the given time slot and day.
        private async Task<bool> IsTeacherAvailable(int teacherId, TimeOnly startTime, TimeOnly endTime, DayOfWeek day)
        {
            var overlappingSchedules = await _dbContext.ClassSchedules
                .Where(cs =>
                    cs.Day == day
                    && (cs.StartTime < endTime)
                    && (cs.EndTime > startTime))
                    .Where(cs => cs.Teachers.Any(t => t.Id == teacherId))
                    .AnyAsync();
            if (overlappingSchedules)
            {
                return false; // Teacher is not available
            }
            return true; // Teacher is available
        }

        //Checks if the labroom is available for the given time slot and day.
        private async Task<bool> IsLabroomAvailable(int labroomId, TimeOnly startTime, DayOfWeek day)
        {
            var isBooked = await _dbContext.ClassSchedules
                .AnyAsync(cs =>
                cs.LabroomId == labroomId
                && cs.Day == day
                && cs.StartTime == startTime);
            if (isBooked)
            {
                return false; // Labroom is not available
            }
            return true; // Labroom is available
        }

        //Clears Existing Schedules for the given level and term.
        private async Task ClearExistingSchedules(int level, int term)
        {
            var courseIds = await _dbContext.Courses
            .Where(c => c.Level == level && c.Term == term)
            .Select(c => c.Id)
            .ToListAsync();
            var sessionalIds = await _dbContext.Sessionals
            .Where(s => s.Level == level && s.Term == term)
            .Select(s => s.Id)
            .ToListAsync();

            var existingSchedules = await _dbContext.ClassSchedules.Where(cs => (cs.CourseId != null && courseIds.Contains(cs.CourseId.Value))
                || (cs.SessionalId != null && sessionalIds.Contains(cs.SessionalId.Value)))
                .ToListAsync();

            if (existingSchedules.Any())
            {
                _dbContext.ClassSchedules.RemoveRange(existingSchedules);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}