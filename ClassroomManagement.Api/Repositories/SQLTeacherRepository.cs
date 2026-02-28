using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace TeacherManagement.Api.Repositories
{
    public class SQLTeacherRepository : ITeacherRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public SQLTeacherRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // public async Task<Teacher> CreateTeacherAsync(Teacher teacher)
        // {
        //     await _dbContext.Teachers.AddAsync(teacher);
        //     await _dbContext.SaveChangesAsync();
        //     return teacher;
        // }

        // public async Task<List<Teacher>?> DeleteTeacherByIdAsync(int id)
        // {
        //     var Teacher = await _dbContext.Teachers.FirstOrDefaultAsync(x => x.Id == id);
        //     if (Teacher is null)
        //     {
        //         return null;
        //     }
        //     _dbContext.Teachers.Remove(Teacher);
        //     await _dbContext.SaveChangesAsync();
        //     return await _dbContext.Teachers.ToListAsync();
        // }

        public async Task<List<Teacher>> GetAllTeachersAsync()
        {
            var TeacherDomains = await _dbContext.Teachers
                .Include(x => x.AssignedCourses)
                .Include(x => x.AssignedSessionals)
                .Include(x => x.Classes)
                .ToListAsync();
            return TeacherDomains;
        }

        // public async Task<Teacher?> GetTeacherByIdAsync(int id)
        // {
        //     var TeacherDomain = await _dbContext.Teachers.Include(x => x.Courses).Include(x => x.Classes).FirstOrDefaultAsync(x => x.Id == id);
        //     return TeacherDomain;
        // }

        // public async Task<Teacher?> UpdateTeacherByIdAsync(int id, Teacher teacher)
        // {
        //     var existingTeacher = await _dbContext.Teachers.Include(x => x.Courses).Include(x => x.Classes).FirstOrDefaultAsync(x => x.Id == id);
        //     if (existingTeacher is null)
        //     {
        //         return null;
        //     }
        //     existingTeacher.Name = teacher.Name;
        //     existingTeacher.Designation = teacher.Designation;
        //     await _dbContext.SaveChangesAsync();
        //     return existingTeacher;
        // }
        public Task<Teacher> CreateTeacherAsync(Teacher teacher)
        {
            throw new NotImplementedException();
        }

        public Task<List<Teacher>?> DeleteTeacherByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Teacher?> GetTeacherByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task SeedSessionalsForTeachersAsync()
        {
            var teachers = await _dbContext.Teachers
                .Include(t => t.AssignedSessionals)
                .ToListAsync();

            // Load all sessionals
            var sessionals = await _dbContext.Sessionals
                .Include(s => s.Teachers)
                .ToListAsync();

            // ===== LEVEL 1, TERM 1 =====
            // CSE 1100 - Intro to Computer System Sessional (Id=1)
            var cse1100 = sessionals.First(s => s.Id == 1);
            cse1100.Teachers.Add(teachers.First(t => t.Id == 9));  // MH
            cse1100.Teachers.Add(teachers.First(t => t.Id == 14)); // AHS

            // CSE 1102 - Structured Programming Language Sessional (Id=2)
            var cse1102 = sessionals.First(s => s.Id == 2);
            cse1102.Teachers.Add(teachers.First(t => t.Id == 2));  // NHC
            cse1102.Teachers.Add(teachers.First(t => t.Id == 4));  // MAM

            // EEE 1164 - Intro to Electrical Engineering Sessional (Id=3)
            var eee1164 = sessionals.First(s => s.Id == 3);
            eee1164.Teachers.Add(teachers.First(t => t.Id == 7));  // MA

            // PHY 1132 - Physics Sessional (Id=4)
            var phy1132 = sessionals.First(s => s.Id == 4);
            phy1132.Teachers.Add(teachers.First(t => t.Id == 3));  // PHY

            // ===== LEVEL 1, TERM 2 =====
            // CSE 1204 - OOP I Sessional (Id=5)
            var cse1204 = sessionals.First(s => s.Id == 5);
            cse1204.Teachers.Add(teachers.First(t => t.Id == 8));  // AH
            cse1204.Teachers.Add(teachers.First(t => t.Id == 9));  // MH
            cse1204.Teachers.Add(teachers.First(t => t.Id == 15)); // ASM
            cse1204.Teachers.Add(teachers.First(t => t.Id == 12)); // SG

            // CSE 1208 - Numerical Methods Sessional (Id=6)
            var cse1208 = sessionals.First(s => s.Id == 6);
            cse1208.Teachers.Add(teachers.First(t => t.Id == 12)); // SG
            cse1208.Teachers.Add(teachers.First(t => t.Id == 13)); // MSZ

            // EEE 1270 - Electronic Circuits Sessional (Id=7)
            var eee1270 = sessionals.First(s => s.Id == 7);
            eee1270.Teachers.Add(teachers.First(t => t.Id == 26)); // EAS

            // CE 1250 - Engineering Drawing Sessional (Id=8)
            var ce1250 = sessionals.First(s => s.Id == 8);
            ce1250.Teachers.Add(teachers.First(t => t.Id == 43)); // PR

            // ENG 1228 - English Skill Sessional (Id=9)
            var eng1228 = sessionals.First(s => s.Id == 9);
            eng1228.Teachers.Add(teachers.First(t => t.Id == 44)); // ENG2

            // ===== LEVEL 2, TERM 1 =====
            // CSE 2102 - Digital Logic Design Sessional (Id=10)
            var cse2102 = sessionals.First(s => s.Id == 10);
            cse2102.Teachers.Add(teachers.First(t => t.Id == 37)); // SA

            // CSE 2104 - Data Structures I Sessional (Id=11)
            var cse2104 = sessionals.First(s => s.Id == 11);
            cse2104.Teachers.Add(teachers.First(t => t.Id == 19)); // MZH
            cse2104.Teachers.Add(teachers.First(t => t.Id == 34)); // AA

            // CSE 2108 - OOP II Sessional (Id=12)
            var cse2108 = sessionals.First(s => s.Id == 12);
            cse2108.Teachers.Add(teachers.First(t => t.Id == 34)); // AA

            // CSE 2100 - Software Development Project I (Id=13)
            var cse2100 = sessionals.First(s => s.Id == 13);
            cse2100.Teachers.Add(teachers.First(t => t.Id == 4));  // MAM
            cse2100.Teachers.Add(teachers.First(t => t.Id == 30)); // NAO

            // ===== LEVEL 2, TERM 2 =====
            // CSE 2202 - Data Structures II Sessional (Id=14)
            var cse2202 = sessionals.First(s => s.Id == 14);
            cse2202.Teachers.Add(teachers.First(t => t.Id == 20)); // MO
            cse2202.Teachers.Add(teachers.First(t => t.Id == 21)); // RR

            // CSE 2206 - Database Sessional (Id=15)
            var cse2206 = sessionals.First(s => s.Id == 15);
            cse2206.Teachers.Add(teachers.First(t => t.Id == 24)); // AKZ
            cse2206.Teachers.Add(teachers.First(t => t.Id == 25)); // GR

            // EEE 2270 - Electrical Drives Sessional (Id=16)
            var eee2270 = sessionals.First(s => s.Id == 16);
            eee2270.Teachers.Add(teachers.First(t => t.Id == 27)); // EEE2
            eee2270.Teachers.Add(teachers.First(t => t.Id == 26)); // EAS

            // ===== LEVEL 3, TERM 1 =====
            // CSE 3102 - Software Engineering Sessional (Id=17)
            var cse3102 = sessionals.First(s => s.Id == 17);
            cse3102.Teachers.Add(teachers.First(t => t.Id == 12)); // SG

            // CSE 3104 - Microprocessors Sessional (Id=18)
            var cse3104 = sessionals.First(s => s.Id == 18);
            cse3104.Teachers.Add(teachers.First(t => t.Id == 29)); // TMM
            cse3104.Teachers.Add(teachers.First(t => t.Id == 31)); // NR
            cse3104.Teachers.Add(teachers.First(t => t.Id == 32)); // AS

            // CSE 3110 - Compiler Sessional (Id=19)
            var cse3110 = sessionals.First(s => s.Id == 19);
            cse3110.Teachers.Add(teachers.First(t => t.Id == 28)); // MI
            cse3110.Teachers.Add(teachers.First(t => t.Id == 34)); // AA

            // CSE 3100 - Software Development Project II (Id=20)
            var cse3100 = sessionals.First(s => s.Id == 20);
            cse3100.Teachers.Add(teachers.First(t => t.Id == 4));  // MAM

            // ===== LEVEL 3, TERM 2 =====
            // CSE 3202 - AI Sessional (Id=21)
            var cse3202 = sessionals.First(s => s.Id == 21);
            cse3202.Teachers.Add(teachers.First(t => t.Id == 32)); // AS
            cse3202.Teachers.Add(teachers.First(t => t.Id == 28)); // MI

            // CSE 3204 - OS Sessional (Id=22)
            var cse3204 = sessionals.First(s => s.Id == 22);
            cse3204.Teachers.Add(teachers.First(t => t.Id == 37)); // SA
            cse3204.Teachers.Add(teachers.First(t => t.Id == 30)); // NAO

            // CSE 3206 - Networks Sessional (Id=23)
            var cse3206 = sessionals.First(s => s.Id == 23);
            cse3206.Teachers.Add(teachers.First(t => t.Id == 28)); // MI
            cse3206.Teachers.Add(teachers.First(t => t.Id == 31)); // NR

            // CSE 3210 - Info System Design Sessional (Id=24)
            var cse3210 = sessionals.First(s => s.Id == 24);
            cse3210.Teachers.Add(teachers.First(t => t.Id == 32)); // AS
            cse3210.Teachers.Add(teachers.First(t => t.Id == 13)); // MSZ

            // CSE 3200 - Web Engineering Project (Id=25)
            var cse3200 = sessionals.First(s => s.Id == 25);
            cse3200.Teachers.Add(teachers.First(t => t.Id == 23)); // ST
            cse3200.Teachers.Add(teachers.First(t => t.Id == 15)); // ASM

            // ===== LEVEL 4, TERM 1 =====
            // CSE 4102 - Computer Security Sessional (Id=26)
            var cse4102 = sessionals.First(s => s.Id == 26);
            cse4102.Teachers.Add(teachers.First(t => t.Id == 35)); // AZ
            cse4102.Teachers.Add(teachers.First(t => t.Id == 30)); // NAO
            cse4102.Teachers.Add(teachers.First(t => t.Id == 38)); // NF1

            // CSE 4104 - Computer Graphics Sessional (Id=27)
            var cse4104 = sessionals.First(s => s.Id == 27);
            cse4104.Teachers.Add(teachers.First(t => t.Id == 29)); // TMM
            cse4104.Teachers.Add(teachers.First(t => t.Id == 34)); // AA

            // CSE 4140 - Machine Learning Sessional (Id=28)
            var cse4140 = sessionals.First(s => s.Id == 28);
            cse4140.Teachers.Add(teachers.First(t => t.Id == 16)); // MSA
            cse4140.Teachers.Add(teachers.First(t => t.Id == 35)); // AZ

            // CSE 4142 - OOSE Sessional (Id=29)
            var cse4142 = sessionals.First(s => s.Id == 29);
            cse4142.Teachers.Add(teachers.First(t => t.Id == 8));  // AH
            cse4142.Teachers.Add(teachers.First(t => t.Id == 25)); // GR

            // ===== LEVEL 4, TERM 2 =====
            // CSE 4246 - Image Processing Sessional (Id=30)
            var cse4246 = sessionals.First(s => s.Id == 30);
            cse4246.Teachers.Add(teachers.First(t => t.Id == 33)); // JA
            cse4246.Teachers.Add(teachers.First(t => t.Id == 37)); // SA

            // CSE 4252 - Data Mining Sessional (Id=31)
            var cse4252 = sessionals.First(s => s.Id == 31);
            cse4252.Teachers.Add(teachers.First(t => t.Id == 31)); // NR
            cse4252.Teachers.Add(teachers.First(t => t.Id == 37)); // SA

            // Save all changes
            var result = await _dbContext.SaveChangesAsync();
        }

        public Task<Teacher?> UpdateTeacherByIdAsync(int id, Teacher teacher)
        {
            throw new NotImplementedException();
        }
    }
}