using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;

namespace ClassroomManagement.Api.Repositories
{
    public interface IClassroomRepository
    {
        Task<List<Classroom>> GetAllClassroomsAsync();
        Task<Classroom?> GetClassroomByIdAsync(int id);
        Task<Classroom> CreateClassroomAsync(Classroom classroom);
        Task<Classroom?> UpdateClassroomAsync(int id, Classroom classroom);
        Task<List<Classroom>?> DeleteClassroomAsync(int id);
    }
}