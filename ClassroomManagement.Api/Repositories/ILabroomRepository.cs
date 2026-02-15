using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs.Labroom;

namespace ClassroomManagement.Api.Repositories
{
    public interface ILabroomRepository
    {
        Task<List<Labroom>> GetAllLabroomsAsync();
        Task<Labroom?> UpdateLabroomByIdAsync(int id, LabroomUpdateRequestDto updatedLabroom);
    }
}