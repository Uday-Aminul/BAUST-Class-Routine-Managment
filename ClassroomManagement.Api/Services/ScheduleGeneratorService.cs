using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace ClassroomManagement.Api.Services
{
    public class ScheduleGeneratorService : IScheduleGeneratorService
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public ScheduleGeneratorService(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> GenerateScheduleAsync()
        {
            var level = 1;
            var term = 1;

            //getting classroom
            var levelTerm = await _dbContext.LevelTerms.FirstOrDefaultAsync(lt => lt.Level == 1 && lt.Term == 1);
            if (levelTerm is null)
            {
                return $"Error: No level-term combination found for Level-{level} Term-{term}.";
            }
            var classroom = levelTerm.ClassroomId;

            //getting courses
            var courses = await _dbContext.Courses.Where(course => course.Level == 1 && course.Term == 1).ToListAsync();
            var totalCourseCredit = 0.0;
            foreach (var course in courses)
            {
                totalCourseCredit += course.Credit;
            }

            //getting sessionals
            var sessionals = await _dbContext.Sessionals
                .Where(s => s.Level == 1 && s.Term == 1)
                .Include(s => s.Teacher)
                .Include(s => s.Labrooms)
                .ToListAsync();
            var sessionalCount = sessionals.Count;

            if (totalCourseCredit is 0 || sessionalCount is 0)
            {
                return "No courses or sessionals found for Level-1 Term-1.";
            }

            await ClearExistingSchedules(level, term);
            //For Level-1 Term-1
            //Actual Logic
            var days = new[]
            {
                DayOfWeek.Sunday,
                DayOfWeek.Monday,
                DayOfWeek.Tuesday,
                DayOfWeek.Wednesday,
                DayOfWeek.Thursday
            };

            // var theorySlots = new[]
            // {
            //     new { Start = new TimeOnly(8, 0), End = new TimeOnly(8, 50) },
            //     new { Start = new TimeOnly(9, 0), End = new TimeOnly(9, 50) },
            //     new { Start = new TimeOnly(10, 0), End = new TimeOnly(10, 50) },
            //     new { Start = new TimeOnly(11, 30), End = new TimeOnly(12, 20) },
            //     new { Start = new TimeOnly(12, 30), End = new TimeOnly(13, 20) },
            //     new { Start = new TimeOnly(13, 30), End = new TimeOnly(14, 20) },
            //     new { Start = new TimeOnly(14, 30), End = new TimeOnly(15, 20) },
            //     new { Start = new TimeOnly(15, 30), End = new TimeOnly(16, 20) },
            //     new { Start = new TimeOnly(16, 30), End = new TimeOnly(17, 20) }
            // };
            // var labSlots = new[]
            // {
            //     new { Start = new TimeOnly(8, 0), End = new TimeOnly(10, 50) },   // Morning lab
            //     new { Start = new TimeOnly(11, 30), End = new TimeOnly(14, 20) }, // Afternoon lab
            //     new { Start = new TimeOnly(14, 30), End = new TimeOnly(17, 20) }  // Evening lab
            // };
            var dualLabPlacement = true;
            var schedulesToAdd = new List<ClassSchedule>();

            foreach (var day in days)
            {
                var labPlacedToday = 0;
                var sessionalPlaced = false;
                if (sessionals.Any())
                {
                    var schedulingState = new SchedulingState
                    {
                        Sessionals = sessionals,
                        Courses = courses,
                        SchedulesToAdd = schedulesToAdd,
                        LabPlacedToday = labPlacedToday,
                        DualLabPlacement = dualLabPlacement,
                    };
                    sessionalPlaced = await PlaceLabAsync(schedulingState, day, new TimeOnly(8, 0), new TimeOnly(10, 50));
                }

                //Placing theory in case lab couldn't be placed.
                if (sessionalPlaced is false)
                {
                    var slots = new[]{
                    new { Start = new TimeOnly(8, 0), End = new TimeOnly(8, 50) },
                    new { Start = new TimeOnly(9, 0), End = new TimeOnly(9, 50) },
                    new { Start = new TimeOnly(10, 0), End = new TimeOnly(10, 50) }
                    };
                    foreach (var slot in slots)
                    {
                        foreach (var course in courses)
                        {
                            var teacher = await _dbContext.Teachers.FindAsync(course.TeacherId);
                            var teacherAvailable = await IsTeacherAvailable(teacher.Id, slot.Start, slot.End, day);
                            if (teacherAvailable is true)
                            {
                                var schedule = new ClassSchedule
                                {
                                    Day = day,
                                    StartTime = slot.Start,
                                    EndTime = slot.End,
                                    ClassroomId = classroom,
                                    CourseId = course.Id,
                                    TeacherId = course.TeacherId.Value
                                };
                                schedulesToAdd.Add(schedule);
                                courses.Remove(course);
                                break; // Move to the next time slot after placing one course
                            }
                        }
                    }
                }


                if (dualLabPlacement)
                {

                }
            }




            if (totalCourseCredit is 0 && sessionalCount is 0)
            {
                return $"Schedule generation succesfull for Level-{level} Term-{term}";
            }
            return $"Some error occurred during generating schedules for Level-{level} Term-{term}";
        }

        //Placing a lab
        private async Task<bool> PlaceLabAsync(
            SchedulingState schedulingState,
            DayOfWeek day,
            TimeOnly startTime,
            TimeOnly endTime)
        {
            var sessionalsCopy = schedulingState.Sessionals.ToList(); // To avoid modifying the original list while iterating
            foreach (var sessional in sessionalsCopy)
            {
                var teachers = new List<Teacher>();
                teachers.Add(sessional.Teacher);  //Sessional will have a list of teachers in future.
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
                        TeacherId = sessional.TeacherId.Value,//Will be a list later
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
                    cs.TeacherId == teacherId
                    && cs.Day == day
                    && (cs.StartTime < endTime)
                    && (cs.EndTime > startTime))
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