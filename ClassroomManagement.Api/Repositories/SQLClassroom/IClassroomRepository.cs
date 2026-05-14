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
        Task<Classroom?> GetClassroomByRoomNumberAsync(int roomNumber);
        Task<Classroom> CreateClassroomAsync(Classroom classroom);
        Task<Classroom?> UpdateClassroomByIdAsync(int id, Classroom classroom);
        Task<List<Classroom>?> DeleteClassroomByIdAsync(int id);
    }
}