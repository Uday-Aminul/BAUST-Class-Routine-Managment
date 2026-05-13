using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassScheduleManagement.Api.Repositories
{
    public class SQLClassScheduleRepository : IClassScheduleRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public SQLClassScheduleRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ClassSchedule> CreateClassScheduleAsync(ClassSchedule classSchedule)
        {
            await _dbContext.ClassSchedules.AddAsync(classSchedule);
            await _dbContext.SaveChangesAsync();
            return classSchedule;
        }

        public async Task<List<ClassSchedule>?> DeleteClassScheduleByIdAsync(int id)
        {
            // var classSchedule = await _dbContext.ClassSchedules.FirstOrDefaultAsync(x => x.Id == id);
            // if (classSchedule is null)
            // {
            //     return null;
            // }
            // _dbContext.ClassSchedules.Remove(classSchedule);
            // await _dbContext.SaveChangesAsync();
            // var classScheduleDomains = await _dbContext.ClassSchedules
            //     .Include(x => x.Classroom)
            //     .Include(x => x.Labroom)
            //     .Include(x => x.Course)
            //     .Include(x => x.Sessional)
            //     .Include(x => x.Teachers)
            //     .ToListAsync();
            // return classScheduleDomains;
            var levelTermSection = await
                _dbContext
                .LevelTermSections
                .Include(lt => lt.AssignedTeachers)
                .ThenInclude(at => at.Teachers)
                .FirstOrDefaultAsync(lt =>
                    lt.Level == 1 &&
                    lt.Term == 1 &&
                    lt.Section == "A");
            //Error Debug
            var course = await _dbContext.Courses.FirstOrDefaultAsync(c => c.CourseCode == "CSE 1101");
            var assignment = levelTermSection.AssignedTeachers.FirstOrDefault(at => at.CourseId == course.Id);
            if (assignment is null)
            {
                Console.WriteLine($"No TeacherAssignment .");
            }
            var teacher = assignment.Teachers.FirstOrDefault();
            if (assignment is not null && teacher is null)
            {
                var demoTeacher = assignment.Teachers.ToList();
                Console.WriteLine($"TeacherAssignment exists but no Teacher for Course: {course.CourseCode}, ");
                if (demoTeacher.Any())
                {
                    var teacherCodes = string.Join(", ", demoTeacher.Select(t => t.Code));
                    Console.WriteLine($"Teachers for Course {course.CourseCode}: {teacherCodes}");
                }
            }
            else if (assignment is null && teacher is null)
            {
                Console.WriteLine($"No TeacherAssignment and no Teacher for Course: {course.CourseCode}");
            }
            else if (assignment is not null && teacher is not null)
            {
                Console.WriteLine($"TeacherAssignment and Teacher exist for Course: {course.CourseCode}, Teacher Code: {teacher.Code}");
            }
            return null;
        }

        public async Task<List<ClassSchedule>> GetAllClassSchedulesAsync(int? level, int? term, string? section)
        {
            var classSchedules = _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .AsQueryable();

            if (level is not null && term is not null && section is not null)
            {
                classSchedules = classSchedules.Where(x => (x.Level == level && x.Term == term && x.Section == section));
            }
            return await classSchedules.ToListAsync();
        }

        public async Task<List<ClassSchedule>> GetAllClassSchedulesByDayAsync(int level, int term, string section, DayOfWeek day)
        {
            var classSchedules = _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .AsQueryable();

            classSchedules = classSchedules.Where(s => s.Level == level && s.Term == term && s.Section == section && s.Day == day);
            return await classSchedules.ToListAsync();
        }

        public async Task<ClassSchedule?> GetClassScheduleByIdAsync(int id)
        {
            var ClassScheduleDomain = await _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .FirstOrDefaultAsync(x => x.Id == id);
            return ClassScheduleDomain;
        }

        public async Task<ClassSchedule?> UpdateClassScheduleByIdAsync(int id, ClassSchedule updatedClassSchedule)
        {
            var existingClassSchedule = await _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Course)
                .Include(x => x.Teachers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingClassSchedule is null)
            {
                return null;
            }
            existingClassSchedule.Day = updatedClassSchedule.Day;
            existingClassSchedule.StartTime = updatedClassSchedule.StartTime;
            existingClassSchedule.EndTime = updatedClassSchedule.EndTime;
            existingClassSchedule.ClassroomId = updatedClassSchedule.ClassroomId;
            existingClassSchedule.CourseId = updatedClassSchedule.CourseId;
            await _dbContext.SaveChangesAsync();
            return existingClassSchedule;
        }
    }
}