using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;

namespace TeacherManagement.Api.Repositories
{
    public interface ITeacherRepository
    {
        Task<List<Teacher>> GetAllTeachersAsync();
        Task<Teacher?> GetTeacherByIdAsync(int id);
        Task<Teacher> CreateTeacherAsync(Teacher teacher);
        Task<Teacher?> UpdateTeacherByIdAsync(int id, Teacher teacher);
        Task<List<Teacher>?> DeleteTeacherByIdAsync(int id);
    }
}