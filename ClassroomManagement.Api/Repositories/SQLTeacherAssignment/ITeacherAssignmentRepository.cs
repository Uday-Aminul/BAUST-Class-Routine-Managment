using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;

namespace TeacherAssignmentManagement.Api.Repositories.SQLTeacherAssignment
{
    public interface ITeacherAssignmentRepository
    {
        Task<List<TeacherAssignment>> GetAllTeacherAssignmentsAsync();
        Task<TeacherAssignment?> GetTeacherAssignmentByIdAsync(int id);
        Task<TeacherAssignment> CreateTeacherAssignmentAsync(TeacherAssignment newTeacherAssignment, List<int> teacherIds);
        Task<TeacherAssignment?> UpdateTeacherAssignmentByIdAsync(int id, TeacherAssignment updatedTeacherAssignment, List<int> teacherIds);
        Task<List<TeacherAssignment>?> DeleteTeacherAssignmentByIdAsync(int id);
    }
}