using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassScheduleManagement.Api.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClassScheduleManagement.Api.Repositories
{
    public class SQLClassScheduleRepository:IClassScheduleRepository
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
            var classSchedule = await _dbContext.ClassSchedules.FirstOrDefaultAsync(x => x.Id == id);
            if (classSchedule is null)
            {
                return null;
            }
            _dbContext.ClassSchedules.Remove(classSchedule);
            await _dbContext.SaveChangesAsync();
            var classScheduleDomains = await _dbContext.ClassSchedules.Include(x => x.Classroom).Include(x => x.Course).Include(x => x.Teacher).ToListAsync();
            return classScheduleDomains;
        }

        public async Task<List<ClassSchedule>> GetAllClassSchedulesAsync()
        {
            var classScheduleDomains = await _dbContext.ClassSchedules.Include(x => x.Classroom).Include(x => x.Course).Include(x=>x.Teacher).ToListAsync();
            return classScheduleDomains;
        }

        public async Task<ClassSchedule?> GetClassScheduleByIdAsync(int id)
        {
            var ClassScheduleDomain = await _dbContext.ClassSchedules.Include(x => x.Classroom).Include(x => x.Course).Include(x=>x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            return ClassScheduleDomain;
        }

        public async Task<ClassSchedule?> UpdateClassScheduleByIdAsync(int id, ClassSchedule updatedClassSchedule)
        {
            var existingClassSchedule = await _dbContext.ClassSchedules.Include(x => x.Classroom).Include(x => x.Course).Include(x=>x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            if (existingClassSchedule is null)
            {
                return null;
            }
            existingClassSchedule.Day = updatedClassSchedule.Day;
            existingClassSchedule.StartTime = updatedClassSchedule.StartTime;
            existingClassSchedule.EndTime = updatedClassSchedule.EndTime;
            existingClassSchedule.Section = updatedClassSchedule.Section;
            existingClassSchedule.ClassroomId = updatedClassSchedule.ClassroomId;
            existingClassSchedule.CourseId = updatedClassSchedule.CourseId;
            existingClassSchedule.TeacherId = updatedClassSchedule.TeacherId;
            await _dbContext.SaveChangesAsync();
            return existingClassSchedule;
        }
    }
}