using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace TeacherAssignmentManagement.Api.Repositories.SQLTeacherAssignment
{
    public class TeacherAssignmentRepository : ITeacherAssignment
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public TeacherAssignmentRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<TeacherAssignment> CreateTeacherAssignmentAsync(TeacherAssignment newTeacherAssignment)
        {
            await _dbContext.TeacherAssignments.AddAsync(newTeacherAssignment);
            await _dbContext.SaveChangesAsync();
            return newTeacherAssignment;
        }

        public async Task<List<TeacherAssignment>?> DeleteTeacherAssignmentByIdAsync(int id)
        {
            var teacherAssignment = await _dbContext.TeacherAssignments.FirstOrDefaultAsync(x => x.Id == id);
            if (teacherAssignment is null)
            {
                return null;
            }
            _dbContext.TeacherAssignments.Remove(teacherAssignment);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.TeacherAssignments.ToListAsync();
        }

        public async Task<List<TeacherAssignment>> GetAllTeacherAssignmentsAsync()
        {
            var teacherAssignmentDomains = await _dbContext.TeacherAssignments
                .Include(x => x.LevelTermSection)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .ToListAsync();
            return teacherAssignmentDomains;
        }

        public async Task<TeacherAssignment?> GetTeacherAssignmentByIdAsync(int id)
        {
            var teacherAssignmentDomain = await _dbContext.TeacherAssignments
                .Include(x => x.LevelTermSection)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (teacherAssignmentDomain is null)
            {
                return null;
            }
            return teacherAssignmentDomain;
        }

        public async Task<TeacherAssignment?> UpdateTeacherAssignmentByIdAsync(int id, TeacherAssignment teacherAssignment)
        {
            var existingTeacherAssignment = await _dbContext.TeacherAssignments
                .Include(x => x.LevelTermSection)
                .Include(x => x.Course)
                .Include(x => x.Sessional)
                .Include(x => x.Teachers)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingTeacherAssignment is null)
            {
                return null;
            }
            existingTeacherAssignment.LevelTermSectionId = teacherAssignment.LevelTermSectionId;
            existingTeacherAssignment.CourseId = teacherAssignment.CourseId;
            existingTeacherAssignment.SessionalId = teacherAssignment.SessionalId;
            existingTeacherAssignment.Teachers = teacherAssignment.Teachers;
            await _dbContext.SaveChangesAsync();
            return existingTeacherAssignment;
        }
    }
}