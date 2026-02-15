using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
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
            var theorySlots = new[]
            {
                new { Start = new TimeOnly(8, 0), End = new TimeOnly(8, 50) },
                new { Start = new TimeOnly(9, 0), End = new TimeOnly(9, 50) },
                new { Start = new TimeOnly(10, 0), End = new TimeOnly(10, 50) },
                new { Start = new TimeOnly(11, 30), End = new TimeOnly(12, 20) },
                new { Start = new TimeOnly(12, 30), End = new TimeOnly(13, 20) },
                new { Start = new TimeOnly(13, 30), End = new TimeOnly(14, 20) },
                new { Start = new TimeOnly(14, 30), End = new TimeOnly(15, 20) },
                new { Start = new TimeOnly(15, 30), End = new TimeOnly(16, 20) },
                new { Start = new TimeOnly(16, 30), End = new TimeOnly(17, 20) }
            };
            var labSlots = new[]
            {
                new { Start = new TimeOnly(8, 0), End = new TimeOnly(10, 50) },   // Morning lab
                new { Start = new TimeOnly(11, 30), End = new TimeOnly(14, 20) }, // Afternoon lab
                new { Start = new TimeOnly(14, 30), End = new TimeOnly(17, 20) }  // Evening lab
            };
            var dualLabPlacement = true;
            for (int i = 0; i < 7; i++)
            {
                if (sessionals.Any())
                {
                    if (dualLabPlacement)
                    {

                    }
                }
            }




            if (totalCourseCredit is 0 && sessionalCount is 0)
            {
                return $"Schedule generation succesfull for Level-{level} Term-{term}";
            }
            return $"Some error occurred during generating schedules for Level-{level} Term-{term}";
        }

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