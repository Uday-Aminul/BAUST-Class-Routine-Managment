using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;

namespace LevelTermSectionManagement.Api.Repositories
{
    public interface ILevelTermSectionRepository
    {
        Task SeedClassroomsForLevelTermSectionsAsync();
        Task SeedTeacherAssignmentsAsync();

        Task<List<LevelTermSection>> GetAllLevelTermSectionsAsync();
        Task<LevelTermSection?> GetLevelTermSectionByLevelTermSectionAsync(int level, int term, string section);
        Task<LevelTermSection> CreateLevelTermSectionAsync(LevelTermSection levelTermSection, List<int>? classroomIds);
        Task<LevelTermSection?> UpdateLevelTermSectionByIdAsync(int id, LevelTermSection newLevelTermSection, List<int>? classroomIds);
        Task<List<LevelTermSection>?> DeleteLevelTermSectionByIdAsync(int id);
    }
}