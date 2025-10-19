using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Repositories
{
    public class SQLClassroomRepository: IClassroomRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public SQLClassroomRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Classroom> CreateClassroomAsync(Classroom classroom)
        {
            await _dbContext.Classrooms.AddAsync(classroom);
            await _dbContext.SaveChangesAsync();
            return classroom;
        }

        public async Task<List<Classroom>?> DeleteClassroomAsync(int id)
        {
            var Classroom = await _dbContext.Classrooms.FirstOrDefaultAsync(x => x.Id == id);
            if (Classroom is null)
            {
                return null;
            }
            _dbContext.Classrooms.Remove(Classroom);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Classrooms.ToListAsync();
        }

        public async Task<List<Classroom>> GetAllClassroomAsync()
        {
            var classroomDomains = await _dbContext.Classrooms.Include(x => x.ClassSchedules).ThenInclude(x => x.Course).Include(x => x.ClassSchedules).ThenInclude(x => x.Teacher).ToListAsync();
            return classroomDomains;
        }

        public async Task<Classroom?> GetClassroomByIdAsync(int id)
        {
            var classroomDomain = await _dbContext.Classrooms.Include(x => x.ClassSchedules).ThenInclude(x => x.Course).Include(x => x.ClassSchedules).ThenInclude(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            return classroomDomain;
        }

        public async Task<Classroom?> UpdateClassroomAsync(int id, Classroom classroom)
        {
            var existingClassroom = await _dbContext.Classrooms.Include(x => x.ClassSchedules).ThenInclude(x => x.Course).Include(x => x.ClassSchedules).ThenInclude(x => x.Teacher).FirstOrDefaultAsync(x => x.Id == id);
            if (existingClassroom is null)
            {
                return null;
            }
            existingClassroom.Id = classroom.Id;
            existingClassroom.IsLab = classroom.IsLab;
            await _dbContext.SaveChangesAsync();
            return existingClassroom;
        }
    }
}