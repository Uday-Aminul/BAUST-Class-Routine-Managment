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

        public async Task<ClassSchedule> CreateClassScheduleAsync(ClassSchedule classSchedule, List<int>? teacherIds)
        {
            var teachers = await _dbContext.Teachers.Where(x => teacherIds.Contains(x.Id)).ToListAsync();
            var classScheduleDomain = new ClassSchedule
            {
                Day = classSchedule.Day,
                StartTime = classSchedule.StartTime,
                EndTime = classSchedule.EndTime,
                Level = classSchedule.Level,
                Term = classSchedule.Term,
                Section = classSchedule.Section,
                WeekType = classSchedule.WeekType,

                ClassroomId = classSchedule.ClassroomId,
                LabroomId = classSchedule.LabroomId,

                CourseId = classSchedule.CourseId,
                SessionalId = classSchedule.SessionalId,
                Teachers = teachers
            };
            await _dbContext.ClassSchedules.AddAsync(classScheduleDomain);
            await _dbContext.SaveChangesAsync();
            return classScheduleDomain;
        }

        public async Task<List<ClassSchedule>?> DeleteClassScheduleByIdAsync(int id)
        {
            var classSchedule = await _dbContext.ClassSchedules.FirstOrDefaultAsync(x => x.Id == id);
            if (classSchedule is null)
            {
                return null;
            }
            _dbContext.ClassSchedules.Remove(classSchedule);
            await _dbContext.SaveChangesAsync();
            var classScheduleDomains = await _dbContext.ClassSchedules
                .Include(x => x.Classroom)
                .Include(x => x.Labroom)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .ToListAsync();
            return classScheduleDomains;
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

        public async Task<ClassSchedule?> UpdateClassScheduleByIdAsync(int id, ClassSchedule updatedClassSchedule, List<int>? teacherIds)
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

            var teachers = await _dbContext.Teachers.Where(x => teacherIds.Contains(x.Id)).ToListAsync();

            existingClassSchedule.Day = updatedClassSchedule.Day;
            existingClassSchedule.StartTime = updatedClassSchedule.StartTime;
            existingClassSchedule.EndTime = updatedClassSchedule.EndTime;
            existingClassSchedule.Level = updatedClassSchedule.Level;
            existingClassSchedule.Term = updatedClassSchedule.Term;
            existingClassSchedule.Section = updatedClassSchedule.Section;
            existingClassSchedule.WeekType = updatedClassSchedule.WeekType;

            existingClassSchedule.ClassroomId = updatedClassSchedule.ClassroomId;
            existingClassSchedule.LabroomId = updatedClassSchedule.LabroomId;

            existingClassSchedule.CourseId = updatedClassSchedule.CourseId;
            existingClassSchedule.SessionalId = updatedClassSchedule.SessionalId;
            existingClassSchedule.Teachers = teachers;
            await _dbContext.SaveChangesAsync();
            return existingClassSchedule;
        }
    }
}