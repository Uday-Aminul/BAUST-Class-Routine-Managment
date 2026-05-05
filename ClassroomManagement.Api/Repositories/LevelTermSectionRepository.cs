using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
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
            var room502 = classrooms.First(c => c.RoomNumber == 502);
            var room506 = classrooms.First(c => c.RoomNumber == 506);
            var room507 = classrooms.First(c => c.RoomNumber == 507);
            var room311 = classrooms.First(c => c.RoomNumber == 311);

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
            level1_II_B.Classrooms.Add(room305);

            // ===== 2/I - A (Room 305) =====
            var level2_I_A = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 1 && lts.Section == "A");
            level2_I_A.Classrooms.Add(room306);

            // ===== 2/I - B (Room 310) =====
            var level2_I_B = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 1 && lts.Section == "B");
            level2_I_B.Classrooms.Add(room308);

            // ===== 2/II - A (Room 309 only) =====
            var level2_II_A = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "A");
            level2_II_A.Classrooms.Add(room309);

            // ===== 2/II - B (Room 309 + Room 310) =====
            var level2_II_B = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "B");
            level2_II_B.Classrooms.Add(room310);

            // ===== 2/II - C (Room 407 only) =====
            // var level2_II_C = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "C");
            // level2_II_C.Classrooms.Add(room309);

            // ===== 3/I - A (Room 304) =====
            var level3_I_A = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 1 && lts.Section == "A");
            level3_I_A.Classrooms.Add(room402);

            // ===== 3/I - B (Room 306) =====
            var level3_I_B = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 1 && lts.Section == "B");
            level3_I_B.Classrooms.Add(room407);

            // ===== 3/II - A (Room 204) =====
            var level3_II_A = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 2 && lts.Section == "A");
            level3_II_A.Classrooms.Add(room408);

            // ===== 3/II - B (Room 205) =====
            var level3_II_B = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 2 && lts.Section == "B");
            level3_II_B.Classrooms.Add(room510);

            // ===== 4/I - A (Room 402 + Room 306 + Room 308) =====
            var level4_I_A = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 1 && lts.Section == "A");
            // level4_I_A.Classrooms.Add(room510);
            // level4_I_A.Classrooms.Add(room408);
            // level4_I_A.Classrooms.Add(room407);
            // level4_I_A.Classrooms.Add(room402);
            // level4_I_A.Classrooms.Add(room310);
            // level4_I_A.Classrooms.Add(room309);
            // level4_I_A.Classrooms.Add(room308);
            // level4_I_A.Classrooms.Add(room306);
            // level4_I_A.Classrooms.Add(room305);
            // level4_I_A.Classrooms.Add(room304);
            // level4_I_A.Classrooms.Add(room205);
            // level4_I_A.Classrooms.Add(room204);
            level4_I_A.Classrooms.Add(room502);

            // ===== 4/I - B (Room 407 + Room 304 + Room 310) =====
            var level4_I_B = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 1 && lts.Section == "B");
            // level4_I_B.Classrooms.Add(room510);
            // level4_I_B.Classrooms.Add(room408);
            // level4_I_B.Classrooms.Add(room407);
            // level4_I_B.Classrooms.Add(room402);
            // level4_I_B.Classrooms.Add(room310);
            // level4_I_B.Classrooms.Add(room309);
            // level4_I_B.Classrooms.Add(room308);
            // level4_I_B.Classrooms.Add(room306);
            // level4_I_B.Classrooms.Add(room305);
            // level4_I_B.Classrooms.Add(room304);
            // level4_I_B.Classrooms.Add(room205);
            // level4_I_B.Classrooms.Add(room204);
            level4_I_B.Classrooms.Add(room506);

            // ===== 4/II - A (Room 407 only) =====
            var level4_II_A = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 2 && lts.Section == "A");
            // level4_II_A.Classrooms.Add(room510);
            // level4_II_A.Classrooms.Add(room408);
            // level4_II_A.Classrooms.Add(room407);
            // level4_II_A.Classrooms.Add(room402);
            // level4_II_A.Classrooms.Add(room310);
            // level4_II_A.Classrooms.Add(room309);
            // level4_II_A.Classrooms.Add(room308);
            // level4_II_A.Classrooms.Add(room306);
            // level4_II_A.Classrooms.Add(room305);
            // level4_II_A.Classrooms.Add(room304);
            // level4_II_A.Classrooms.Add(room205);
            // level4_II_A.Classrooms.Add(room204);
            level4_II_A.Classrooms.Add(room507);

            // ===== 4/II - B (Room 407 + Room 304 + Room 310) =====
            var level4_II_B = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 2 && lts.Section == "B");
            // level4_II_B.Classrooms.Add(room510);
            // level4_II_B.Classrooms.Add(room408);
            // level4_II_B.Classrooms.Add(room407);
            // level4_II_B.Classrooms.Add(room402);
            // level4_II_B.Classrooms.Add(room310);
            // level4_II_B.Classrooms.Add(room309);
            // level4_II_B.Classrooms.Add(room308);
            // level4_II_B.Classrooms.Add(room306);
            // level4_II_B.Classrooms.Add(room305);
            // level4_II_B.Classrooms.Add(room304);
            // level4_II_B.Classrooms.Add(room205);
            // level4_II_B.Classrooms.Add(room204);
            level4_II_B.Classrooms.Add(room311);

            await _dbContext.SaveChangesAsync();
        }

        public async Task SeedTeacherAssignmentsAsync()
        {
            var levelTermSections = await _dbContext.LevelTermSections.ToListAsync();
            var courses = await _dbContext.Courses.ToListAsync();
            var sessionals = await _dbContext.Sessionals.ToListAsync();
            var teachers = await _dbContext.Teachers.ToListAsync();

            var teacherAssignments = new List<TeacherAssignment>();

            // ===== Level 1, Term 1, Section A =====
            var l1t1a = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 1 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, CourseId = courses.First(c => c.CourseCode == "EEE 1163").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TQA") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 1101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NHC") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, CourseId = courses.First(c => c.CourseCode == "PHY 1131").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "PHY") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, CourseId = courses.First(c => c.CourseCode == "ENG 1127").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ENG1") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, CourseId = courses.First(c => c.CourseCode == "MATH 1141").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH1") } },
                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NHC"), teachers.First(t => t.Code == "MAM") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1100").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MH"), teachers.First(t => t.Code == "AHS") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "PHY 1132").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "PHY") } },
                new TeacherAssignment { LevelTermSectionId = l1t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "EEE 1164").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MA"), teachers.First(t => t.Code == "NHS") } }
            });

            // ===== Level 1, Term 1, Section B =====
            var l1t1b = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 1 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, CourseId = courses.First(c => c.CourseCode == "ENG 1127").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ENG1") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, CourseId = courses.First(c => c.CourseCode == "EEE 1163").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EEE1") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 1101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NHC") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, CourseId = courses.First(c => c.CourseCode == "PHY 1131").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "PHY") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, CourseId = courses.First(c => c.CourseCode == "MATH 1141").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH1") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "EEE 1164").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MA") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "PHY 1132").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "PHY") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1100").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MH"), teachers.First(t => t.Code == "AS") } },
                new TeacherAssignment { LevelTermSectionId = l1t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AHS"), teachers.First(t => t.Code == "MAM") } }
            });

            // ===== Level 1, Term 2, Section A =====
            var l1t2a = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 2 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, CourseId = courses.First(c => c.CourseCode == "EEE 1269").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EMH") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, CourseId = courses.First(c => c.CourseCode == "MATH 1243").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH2") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 1203").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AH") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 1201").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AHS") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, CourseId = courses.First(c => c.CourseCode == "HUM 1221").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "HUM1") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1204").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AH"), teachers.First(t => t.Code == "MH") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CE 1250").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "PR") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "ENG 1228").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ENG2") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "EEE 1270").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EAS") } },
                new TeacherAssignment { LevelTermSectionId = l1t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1208").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SG"), teachers.First(t => t.Code == "MSZ") } }
            });

            // ===== Level 1, Term 2, Section B =====
            var l1t2b = levelTermSections.First(lts => lts.Level == 1 && lts.Term == 2 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 1201").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AHS") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 1203").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ASM") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, CourseId = courses.First(c => c.CourseCode == "MATH 1243").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH2") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, CourseId = courses.First(c => c.CourseCode == "EEE 1269").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MA") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, CourseId = courses.First(c => c.CourseCode == "HUM 1221").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "HUM1") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1208").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SG"), teachers.First(t => t.Code == "MSZ") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 1204").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ASM"), teachers.First(t => t.Code == "SG") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CE 1250").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "PR") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "ENG 1228").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ENG2") } },
                new TeacherAssignment { LevelTermSectionId = l1t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "EEE 1270").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EAS") } }
            });

            // ===== Level 2, Term 1, Section A =====
            var l2t1a = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 1 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSA") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, CourseId = courses.First(c => c.CourseCode == "CHEM 2133").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "CHEM") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, CourseId = courses.First(c => c.CourseCode == "MATH 2145").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH3") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2103").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MZH") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2105").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MO") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2104").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MZH"), teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SA") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2100").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MAM"), teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l2t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2108").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ST"), teachers.First(t => t.Code == "MSZ") } }
            });

            // ===== Level 2, Term 1, Section B =====
            var l2t1b = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 1 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, CourseId = courses.First(c => c.CourseCode == "CHEM 2133").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "CHEM") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, CourseId = courses.First(c => c.CourseCode == "MATH 2145").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH3") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2103").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MZH") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SA") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2105").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MO") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2104").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MZH") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2108").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2100").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l2t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSA") } }
            });

            // ===== Level 2, Term 2, Section A =====
            var l2t2a = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2201").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "RR") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, CourseId = courses.First(c => c.CourseCode == "MATH 2247").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH4") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2203").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ST") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2205").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AKZ") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, CourseId = courses.First(c => c.CourseCode == "EEE 2269").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EAS") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, CourseId = courses.First(c => c.CourseCode == "HUM 2221").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "HUM2") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "EEE 2270").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EEE2") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2206").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AKZ"), teachers.First(t => t.Code == "GR") } },
                new TeacherAssignment { LevelTermSectionId = l2t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2202").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MO"), teachers.First(t => t.Code == "RR") } }
            });

            // ===== Level 2, Term 2, Section B =====
            var l2t2b = levelTermSections.First(lts => lts.Level == 2 && lts.Term == 2 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, CourseId = courses.First(c => c.CourseCode == "EEE 2269").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EAS") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, CourseId = courses.First(c => c.CourseCode == "MATH 2247").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MATH4") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2203").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ST") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, CourseId = courses.First(c => c.CourseCode == "HUM 2221").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "HUM2") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2201").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "RR") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 2205").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AKZ") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2206").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AKZ"), teachers.First(t => t.Code == "GR") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "EEE 2270").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "EEE2") } },
                new TeacherAssignment { LevelTermSectionId = l2t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 2202").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MO"), teachers.First(t => t.Code == "RR") } }
            });

            // ===== Level 3, Term 1, Section A =====
            var l3t1a = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 1 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3109").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MI") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3103").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TMM") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3107").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MAM") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SG") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, CourseId = courses.First(c => c.CourseCode == "ME 3181").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ME") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3105").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MH") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3104").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TMM"), teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3110").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MI"), teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3100").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MAM"), teachers.First(t => t.Code == "MO") } },
                new TeacherAssignment { LevelTermSectionId = l3t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SG"), teachers.First(t => t.Code == "ASM") } }
            });

            // ===== Level 3, Term 1, Section B =====
            var l3t1b = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 1 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3103").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TMM") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3107").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MAM") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3109").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MI") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, CourseId = courses.First(c => c.CourseCode == "ME 3181").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ME") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3105").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MH") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SG") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3104").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TMM"), teachers.First(t => t.Code == "AS") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3110").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MI"), teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3100").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MAM"), teachers.First(t => t.Code == "MO") } },
                new TeacherAssignment { LevelTermSectionId = l3t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SG"), teachers.First(t => t.Code == "ASM") } }
            });

            // ===== Level 3, Term 2, Section A =====
            var l3t2a = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 2 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3201").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AS") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3203").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3207").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "GR") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3205").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3209").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSZ") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3210").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AS"), teachers.First(t => t.Code == "MSZ") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3200").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ST"), teachers.First(t => t.Code == "ASM") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3206").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MI"), teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3204").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SA"), teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l3t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3202").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AS"), teachers.First(t => t.Code == "MI") } }
            });

            // ===== Level 3, Term 2, Section B =====
            var l3t2b = levelTermSections.First(lts => lts.Level == 3 && lts.Term == 2 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3203").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3201").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AS") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3205").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3207").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "GR") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 3209").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSZ") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3200").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "ST"), teachers.First(t => t.Code == "ASM") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3204").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "SA"), teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3210").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AS"), teachers.First(t => t.Code == "MSZ") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3206").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MI"), teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l3t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 3202").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AS"), teachers.First(t => t.Code == "MI") } }
            });

            // ===== Level 4, Term 1, Section A =====
            var l4t1a = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 1 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4103").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4141").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AH") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AZ") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, CourseId = courses.First(c => c.CourseCode == "HUM 4123").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "BBA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4139").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "JA") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AZ"), teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4104").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TMM"), teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4142").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AH"), teachers.First(t => t.Code == "GR") } },
                new TeacherAssignment { LevelTermSectionId = l4t1a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4140").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSA"), teachers.First(t => t.Code == "AZ") } }
            });

            // ===== Level 4, Term 1, Section B =====
            var l4t1b = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 1 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4139").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4103").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, CourseId = courses.First(c => c.CourseCode == "HUM 4123").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "BBA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4101").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AZ") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4141").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AH") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4102").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NF1"), teachers.First(t => t.Code == "NAO") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4104").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "TMM"), teachers.First(t => t.Code == "AA") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4142").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AH"), teachers.First(t => t.Code == "GR") } },
                new TeacherAssignment { LevelTermSectionId = l4t1b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4140").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "MSA"), teachers.First(t => t.Code == "AZ") } }
            });

            // ===== Level 4, Term 2, Section A =====
            var l4t2a = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 2 && lts.Section == "A");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, CourseId = courses.First(c => c.CourseCode == "IPE 4217").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "IPE") } },
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4251").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4215").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NF1") } },
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4245").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "JA") } },
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, CourseId = courses.First(c => c.CourseCode == "HUM 4273").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AIS") } },
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4249").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "RA"), teachers.First(t => t.Code == "MAS") } },

                // Sessionals
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4246").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "JA"), teachers.First(t => t.Code == "SA") } },
                new TeacherAssignment { LevelTermSectionId = l4t2a.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4252").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NR"), teachers.First(t => t.Code == "SA") } }
            });

            // ===== Level 4, Term 2, Section B =====
            var l4t2b = levelTermSections.First(lts => lts.Level == 4 && lts.Term == 2 && lts.Section == "B");
            teacherAssignments.AddRange(new[]
            {
                // Theory Courses (Same as Section A)
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, CourseId = courses.First(c => c.CourseCode == "IPE 4217").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "IPE") } },
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4251").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NR") } },
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4215").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NF1") } },
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4245").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "JA") } },
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, CourseId = courses.First(c => c.CourseCode == "HUM 4273").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "AIS") } },
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, CourseId = courses.First(c => c.CourseCode == "CSE 4249").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "RA"), teachers.First(t => t.Code == "MAS") } },

                // Sessionals (Same as Section A)
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4246").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "JA"), teachers.First(t => t.Code == "SA") } },
                new TeacherAssignment { LevelTermSectionId = l4t2b.Id, SessionalId = sessionals.First(s => s.SessionalCode == "CSE 4252").Id, Teachers = new List<Teacher> { teachers.First(t => t.Code == "NR"), teachers.First(t => t.Code == "SA") } }
            });

            await _dbContext.TeacherAssignments.AddRangeAsync(teacherAssignments);
            await _dbContext.SaveChangesAsync();
        }
    }
}