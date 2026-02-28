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

        public async Task SeedSessionalsForLabroomsAsync()
        {
            var labrooms = await _dbContext.Labrooms
                .Include(l => l.AllowedSessionals)
                .ToListAsync();

            var sessionals = await _dbContext.Sessionals
                .Include(s => s.Labrooms)
                .ToListAsync();

            // ===== LAB 202 =====
            var lab202 = labrooms.First(l => l.Id == 202);
            lab202.AllowedSessionals.Add(sessionals.First(s => s.Id == 16)); // EEE 2270

            // ===== LAB 210 =====
            var lab210 = labrooms.First(l => l.Id == 210);
            lab210.AllowedSessionals.Add(sessionals.First(s => s.Id == 25)); // CSE 3200

            // ===== LAB 302 =====
            var lab302 = labrooms.First(l => l.Id == 302);
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 6));  // CSE 1208
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 11)); // CSE 2104
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 12)); // CSE 2108
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 14)); // CSE 2202
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 17)); // CSE 3102
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 19)); // CSE 3110
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 20)); // CSE 3100
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 25)); // CSE 3200
            lab302.AllowedSessionals.Add(sessionals.First(s => s.Id == 26)); // CSE 4102

            // ===== LAB 307 =====
            var lab307 = labrooms.First(l => l.Id == 307);
            lab307.AllowedSessionals.Add(sessionals.First(s => s.Id == 10)); // CSE 2102
            lab307.AllowedSessionals.Add(sessionals.First(s => s.Id == 31)); // CSE 4252

            // ===== LAB 311 =====
            var lab311 = labrooms.First(l => l.Id == 311);
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 5));  // CSE 1204
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 6));  // CSE 1208
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 12)); // CSE 2108
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 15)); // CSE 2206
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 22)); // CSE 3204
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 23)); // CSE 3206
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 26)); // CSE 4102
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 27)); // CSE 4104
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 28)); // CSE 4140
            lab311.AllowedSessionals.Add(sessionals.First(s => s.Id == 29)); // CSE 4142

            // ===== LAB 402 =====
            var lab402 = labrooms.First(l => l.Id == 402);
            lab402.AllowedSessionals.Add(sessionals.First(s => s.Id == 8));  // CE 1250
            lab402.AllowedSessionals.Add(sessionals.First(s => s.Id == 9));  // ENG 1228
            lab402.AllowedSessionals.Add(sessionals.First(s => s.Id == 13)); // CSE 2100
            lab402.AllowedSessionals.Add(sessionals.First(s => s.Id == 18)); // CSE 3104

            // ===== LAB 411 =====
            var lab411 = labrooms.First(l => l.Id == 411);
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 1));  // CSE 1100
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 2));  // CSE 1102
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 5));  // CSE 1204
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 15)); // CSE 2206
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 17)); // CSE 3102
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 20)); // CSE 3100
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 21)); // CSE 3202
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 24)); // CSE 3210
            lab411.AllowedSessionals.Add(sessionals.First(s => s.Id == 30)); // CSE 4246

            // ===== DC Circuit Lab (1002) =====
            var dcCircuitLab = labrooms.First(l => l.Id == 1002);
            dcCircuitLab.AllowedSessionals.Add(sessionals.First(s => s.Id == 3)); // EEE 1164

            // ===== AC Circuit Lab (1003) =====
            var acCircuitLab2 = labrooms.First(l => l.Id == 1003);
            acCircuitLab2.AllowedSessionals.Add(sessionals.First(s => s.Id == 3)); // EEE 1164

            // ===== Electronics Lab (1004) =====
            var electronicsLab = labrooms.First(l => l.Id == 1004);
            electronicsLab.AllowedSessionals.Add(sessionals.First(s => s.Id == 7)); // EEE 1270

            // ===== Physics Lab (1005) =====
            var physicsLab = labrooms.First(l => l.Id == 1005);
            physicsLab.AllowedSessionals.Add(sessionals.First(s => s.Id == 4)); // PHY 1132

            await _dbContext.SaveChangesAsync();
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