using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace SessionalManagement.Api.Repositories.SQLSessional
{
    public class SessionalRepository : ISessionalRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public SessionalRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Sessional> CreateSessionalAsync(Sessional sessional, List<int>? labroomIds)
        {
            var sessionalDomain = new Sessional
            {
                Name = sessional.Name,
                SessionalCode = sessional.SessionalCode,
                Level = sessional.Level,
                Term = sessional.Term,
                Credit = sessional.Credit,
            };
            var labrooms = await _dbContext.Labrooms.Where(x => labroomIds.Contains(x.Id)).ToListAsync();
            sessionalDomain.Labrooms = labrooms;
            await _dbContext.Sessionals.AddAsync(sessionalDomain);
            await _dbContext.SaveChangesAsync();
            return sessionalDomain;
        }

        public async Task<List<Sessional>?> DeleteSessionalByIdAsync(int id)
        {
            var Sessional = await _dbContext.Sessionals.FirstOrDefaultAsync(x => x.Id == id);
            if (Sessional is null)
            {
                return null;
            }
            _dbContext.Sessionals.Remove(Sessional);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Sessionals.ToListAsync();
        }

        public async Task<List<Sessional>> GetAllSessionalsAsync()
        {
            var sessionalDomains = await _dbContext.Sessionals
                .Include(x => x.Labrooms)
                .ToListAsync();
            return sessionalDomains;
        }

        public async Task<Sessional?> GetSessionalByIdAsync(int id)
        {
            var sessionalDomain = await _dbContext.Sessionals
                .Include(x => x.Labrooms)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (sessionalDomain is null)
            {
                return null;
            }
            return sessionalDomain;
        }

        public async Task<Sessional?> UpdateSessionalByIdAsync(int id, Sessional sessional, List<int>? labroomIds)
        {
            var existingSessional = await _dbContext.Sessionals
                .Include(x => x.Labrooms)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (existingSessional is null)
            {
                return null;
            }
            var labrooms = await _dbContext.Labrooms.Where(x => labroomIds.Contains(x.Id)).ToListAsync();

            existingSessional.Name = sessional.Name;
            existingSessional.SessionalCode = sessional.SessionalCode;
            existingSessional.Level = sessional.Level;
            existingSessional.Term = sessional.Term;
            existingSessional.Credit = sessional.Credit;
            existingSessional.Labrooms = labrooms;
            await _dbContext.SaveChangesAsync();
            return existingSessional;
        }
    }
}