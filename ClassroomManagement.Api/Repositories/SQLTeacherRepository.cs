using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace TeacherManagement.Api.Repositories
{
    public class SQLTeacherRepository: ITeacherRepository
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

        public async Task<List<Teacher>?> DeleteTeacherAsync(int id)
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
            var TeacherDomains = await _dbContext.Teachers.Include(x => x.Courses).Include(x => x.Classes).Include(x => x.Department).ToListAsync();
            return TeacherDomains;
        }

        public async Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            var TeacherDomain = await _dbContext.Teachers.Include(x => x.Courses).Include(x => x.Classes).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            return TeacherDomain;
        }

        public async Task<Teacher?> UpdateTeacherAsync(int id, Teacher teacher)
        {
            var existingTeacher = await _dbContext.Teachers.Include(x => x.Courses).Include(x => x.Classes).Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == id);
            if (existingTeacher is null)
            {
                return null;
            }
            existingTeacher.Name = teacher.Name;
            existingTeacher.Designation = teacher.Designation;
            existingTeacher.DepartmentId = teacher.DepartmentId;
            await _dbContext.SaveChangesAsync();
            return existingTeacher;
        }
    }
}