using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Repositories
{
    public class LevelTermSectionRepository : ILevelTermSectionRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public LevelTermSectionRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SeedClassroomsForLevelTermSectionsAsync()
        {
            var levelTermSections = await _dbContext.LevelTermSections.Include(lt => lt.Classrooms).ToListAsync();
            var classrooms = await _dbContext.Classrooms.ToListAsync();

            // FIRST, get ALL classroom references
            var room204 = classrooms.First(c => c.RoomNumber == 204);
            var room205 = classrooms.First(c => c.RoomNumber == 205);
            var room304 = classrooms.First(c => c.RoomNumber == 304);
            var room305 = classrooms.First(c => c.RoomNumber == 305);
            var room306 = classrooms.First(c => c.RoomNumber == 306);
            var room308 = classrooms.First(c => c.RoomNumber == 308);
            var room309 = classrooms.First(c => c.RoomNumber == 309);
            var room310 = classrooms.First(c => c.RoomNumber == 310);
            var room402 = classrooms.First(c => c.RoomNumber == 402);
            var room407 = classrooms.First(c => c.RoomNumber == 407);
            var room408 = classrooms.First(c => c.RoomNumber == 408);
            var room510 = classrooms.First(c => c.RoomNumber == 510);

            // THEN assign to sections
            // ===== 1/I - A (Room 408) =====
            var level1_I_A = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 1 && lts.Section == "A");
            level1_I_A.Classrooms.Add(room204);

            // ===== 1/I - B (Room 308) =====
            var level1_I_B = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 1 && lts.Section == "B");
            level1_I_B.Classrooms.Add(room205);

            // ===== 1/II - A (Room 408 only) =====
            var level1_II_A = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 2 && lts.Section == "A");
            level1_II_A.Classrooms.Add(room304);

            // ===== 1/II - B (Room 308 only) =====
            var level1_II_B = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 2 && lts.Section == "B");
            level1_II_B.Classrooms.Add(room204);
            level1_II_B.Classrooms.Add(room205);
            level1_II_B.Classrooms.Add(room304);

            // ===== 2/I - A (Room 305) =====
            var level2_I_A = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 1 && lts.Section == "A");
            level2_I_A.Classrooms.Add(room305);

            // ===== 2/I - B (Room 310) =====
            var level2_I_B = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 1 && lts.Section == "B");
            level2_I_B.Classrooms.Add(room306);

            // ===== 2/II - A (Room 309 only) =====
            var level2_II_A = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "A");
            level2_II_A.Classrooms.Add(room308);

            // ===== 2/II - B (Room 309 + Room 310) =====
            var level2_II_B = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "B");
            level2_II_B.Classrooms.Add(room305);
            level2_II_B.Classrooms.Add(room306);
            level2_II_B.Classrooms.Add(room308);

            // ===== 2/II - C (Room 407 only) =====
            var level2_II_C = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "C");
            level2_II_C.Classrooms.Add(room309);

            // ===== 3/I - A (Room 304) =====
            var level3_I_A = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 1 && lts.Section == "A");
            level3_I_A.Classrooms.Add(room310);

            // ===== 3/I - B (Room 306) =====
            var level3_I_B = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 1 && lts.Section == "B");
            level3_I_B.Classrooms.Add(room402);

            // ===== 3/II - A (Room 204) =====
            var level3_II_A = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 2 && lts.Section == "A");
            level3_II_A.Classrooms.Add(room309);
            level3_II_A.Classrooms.Add(room310);
            level3_II_A.Classrooms.Add(room402);

            // ===== 3/II - B (Room 205) =====
            var level3_II_B = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 2 && lts.Section == "B");
            level3_II_B.Classrooms.Add(room407);

            // ===== 4/I - A (Room 402 + Room 306 + Room 308) =====
            var level4_I_A = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 1 && lts.Section == "A");
            level4_I_A.Classrooms.Add(room408);

            // ===== 4/I - B (Room 407 + Room 304 + Room 310) =====
            var level4_I_B = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 1 && lts.Section == "B");
            level4_I_B.Classrooms.Add(room510);

            // ===== 4/II - A (Room 407 only) =====
            var level4_II_A = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 2 && lts.Section == "A");
            level4_II_A.Classrooms.Add(room407);
            level4_II_A.Classrooms.Add(room408);
            level4_II_A.Classrooms.Add(room510);

            await _dbContext.SaveChangesAsync();
        }
    }
}