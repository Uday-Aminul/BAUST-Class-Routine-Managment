using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;

namespace TeacherAssignmentManagement.Api.Repositories.SQLTeacherAssignment
{
    public interface ITeacherAssignment
    {
        Task<List<TeacherAssignment>> GetAllTeacherAssignmentsAsync();
        Task<TeacherAssignment?> GetTeacherAssignmentByIdAsync(int id);
        Task<TeacherAssignment> CreateTeacherAssignmentAsync(TeacherAssignment newTeacherAssignment);
        Task<TeacherAssignment?> UpdateTeacherAssignmentByIdAsync(int id, TeacherAssignment updatedTeacherAssignment);
        Task<List<TeacherAssignment>?> DeleteTeacherAssignmentByIdAsync(int id);
    }
}