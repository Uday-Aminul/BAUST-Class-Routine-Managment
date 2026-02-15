using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs.Labroom;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Repositories
{
    public class SQLLabroomRepository : ILabroomRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public SQLLabroomRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Labroom>> GetAllLabroomsAsync()
        {
            var labrooms = await _dbContext.Labrooms.Include(l => l.AllowedSessionals).Include(l => l.ClassSchedules).ToListAsync();
            return labrooms;
        }

        public async Task<Labroom?> UpdateLabroomByIdAsync(int id, LabroomUpdateRequestDto updatedLabroom)
        {
            var existingLabroom = await _dbContext.Labrooms.Include(l => l.AllowedSessionals).FirstOrDefaultAsync(l => l.Id == id);
            if (existingLabroom is null)
            {
                return null;
            }
            existingLabroom.Name = updatedLabroom.Name;
            existingLabroom.AllowedSessionals = await _dbContext.Sessionals.Where(s => updatedLabroom.AllowedSessionalCodes.Contains(s.SessionalCode)).ToListAsync();
            await _dbContext.SaveChangesAsync();
            return existingLabroom;
        }
    }
}