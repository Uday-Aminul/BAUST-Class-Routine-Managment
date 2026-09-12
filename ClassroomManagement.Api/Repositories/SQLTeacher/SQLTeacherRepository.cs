using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace TeacherManagement.Api.Repositories
{
    public class SQLTeacherRepository : ITeacherRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public SQLTeacherRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            await _dbContext.Teachers.AddAsync(teacher);
            await _dbContext.SaveChangesAsync();
            return teacher;
        }

        public async Task<List<Teacher>?> DeleteTeacherByIdAsync(int id)
        {
            var Teacher = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id);
            if (Teacher is null)
            {
                return null;
            }
            _dbContext.Teachers.Remove(Teacher);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Teachers.ToListAsync();
        }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            var TeacherDomains = await _dbContext.Teachers
                .Include(x => x.Classes)
                .Include(x => x.AssignedSections)
                .ThenInclude(a => a.Course)
                .Include(x => x.AssignedSections)
                .ThenInclude(a => a.Sessional)
                .ToListAsync();
            return TeacherDomains;
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            var teacherDomain = await _dbContext.Teachers
                .Include(x => x.Classes)
                    .ThenInclude(c => c.Course)
                .Include(x => x.Classes)
                    .ThenInclude(c => c.Sessional)
                .Include(x => x.Classes)
                    .ThenInclude(c => c.Classroom)
                .Include(x => x.Classes)
                    .ThenInclude(c => c.Labroom)
                .Include(x => x.AssignedSections)
                    .ThenInclude(a => a.Course)
                .Include(x => x.AssignedSections)
                    .ThenInclude(a => a.Sessional)
                .Include(x => x.AssignedSections)
                    .ThenInclude(a => a.LevelTermSection)
                .FirstOrDefaultAsync(x => x.Id == id);

            return teacherDomain;
        }

        public async Task<Teacher?> UpdateTeacherByIdAsync(int id, Teacher updatedTeacher)
        {
            var existingTeacher = await _dbContext.Teachers.Include(x => x.Classes).Include(x => x.AssignedSections).FirstOrDefaultAsync(x => x.Id == id);
            if (existingTeacher is null)
            {
                return null;
            }
            existingTeacher.Name = updatedTeacher.Name;
            existingTeacher.Code = updatedTeacher.Code;
            existingTeacher.Designation = updatedTeacher.Designation;
            existingTeacher.Classes = updatedTeacher.Classes;
            await _dbContext.SaveChangesAsync();
            return existingTeacher;
        }
    }
}