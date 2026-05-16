using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;

namespace SessionalManagement.Api.Repositories.SQLSessional
{
    public interface ISessionalRepository
    {
        Task<List<Sessional>> GetAllSessionalsAsync();
        Task<Sessional?> GetSessionalByIdAsync(int id);
        Task<Sessional> CreateSessionalAsync(Sessional sessional, List<int>? labroomIds);
        Task<Sessional?> UpdateSessionalByIdAsync(int id, Sessional sessional, List<int>? labroomIds);
        Task<List<Sessional>?> DeleteSessionalByIdAsync(int id);
    }
}