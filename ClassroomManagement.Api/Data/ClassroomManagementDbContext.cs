using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Data
{
    public class ClassroomManagementDbContext : DbContext
    {
        public ClassroomManagementDbContext(DbContextOptions<ClassroomManagementDbContext> options) : base(options)
        {

        }
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Labroom> Labrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Sessional> Sessionals { get; set; }
        public DbSet<LevelTerm> LevelTerms { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Classrooms (for theory classes)
            var ClassRooms = new List<Classroom>
            {
                new Classroom { Id = 204 }, // 3/II - A
                new Classroom { Id = 205 }, // 3/II - B
                new Classroom { Id = 304 }, // 3/I - A
                new Classroom { Id = 305 }, // 4/II - A
                new Classroom { Id = 306 }, // 4/I - A
                new Classroom { Id = 308 }, // 1/I - B
                new Classroom { Id = 309 }, // 2/II - A
                new Classroom { Id = 310 }, // 2/I - B
                new Classroom { Id = 311 }, // General
                new Classroom { Id = 407 }, // 4/II - A
                new Classroom { Id = 408 }, // 1/I - A
                new Classroom { Id = 502 }, // English
                new Classroom { Id = 506 }, // English
                new Classroom { Id = 507 }, // BBA
                new Classroom { Id = 510 }, // English
                new Classroom { Id = 1001 }, // Seminar Hall
            };
            modelBuilder.Entity<Classroom>().HasData(ClassRooms);

            // Labrooms (for practical sessions)
            var Labrooms = new List<Labroom>
            {
                new Labroom { Id = 14, Name = "Room 014" },      // For CE: 2/I
                new Labroom { Id = 24, Name = "Room 024" },      // For IPE: 1/II
                new Labroom { Id = 107, Name = "Room 107" },     // For IPE: 1/II
                new Labroom { Id = 108, Name = "Room 108" },     // For ME: 1/II
                new Labroom { Id = 110, Name = "Room 110" },     // For ME, IPE labs
                new Labroom { Id = 202, Name = "EEE Lab" },      // EEE Lab
                new Labroom { Id = 206, Name = "Room 206" },     // For EEE: 2/I
                new Labroom { Id = 210, Name = "CSE Lab 210" },  // CSE Lab
                new Labroom { Id = 302, Name = "CSE Lab 302" },  // CSE Lab
                new Labroom { Id = 307, Name = "CSE Lab 307" },  // CSE Lab
                new Labroom { Id = 311, Name = "CSE Lab 311" },  // CSE Lab
                new Labroom { Id = 402, Name = "CSE/CAD Lab 402" }, // CAD Lab
                new Labroom { Id = 411, Name = "CSE Lab 411" },  // CSE Lab
                new Labroom { Id = 1001, Name = "AC Circuit Lab" },
                new Labroom { Id = 1002, Name = "DC Circuit Lab" },
                new Labroom { Id = 1003, Name = "AC Circuit Lab" },
                new Labroom { Id = 1004, Name = "Electronics Lab" },
                new Labroom { Id = 1005, Name = "Physics Lab" },
            };
            modelBuilder.Entity<Labroom>().HasData(Labrooms);

            // Teachers
            var teachers = new List<Teacher>
            {
                new Teacher { Id = 1, Name = "TQA", Code = "TQA", Designation = "Lecturer" },
                new Teacher { Id = 2, Name = "NHC", Code = "NHC", Designation = "Lecturer" },
                new Teacher { Id = 3, Name = "PHY", Code = "PHY", Designation = "Assistant Professor" },
                new Teacher { Id = 4, Name = "MAM", Code = "MAM", Designation = "Assistant Professor" },
                new Teacher { Id = 5, Name = "ENG1", Code = "ENG1", Designation = "Lecturer" },
                new Teacher { Id = 6, Name = "EEE1", Code = "EEE1", Designation = "Lecturer" },
                new Teacher { Id = 7, Name = "MA", Code = "MA", Designation = "Lecturer" },
                new Teacher { Id = 8, Name = "AH", Code = "AH", Designation = "Assistant Professor" },
                new Teacher { Id = 9, Name = "MH", Code = "MH", Designation = "Lecturer" },
                new Teacher { Id = 10, Name = "EMH", Code = "EMH", Designation = "Lecturer" },
                new Teacher { Id = 11, Name = "MATH2", Code = "MATH2", Designation = "Assistant Professor" },
                new Teacher { Id = 12, Name = "SG", Code = "SG", Designation = "Lecturer" },
                new Teacher { Id = 13, Name = "MSZ", Code = "MSZ", Designation = "Lecturer" },
                new Teacher { Id = 14, Name = "AHS", Code = "AHS", Designation = "Lecturer" },
                new Teacher { Id = 15, Name = "ASM", Code = "ASM", Designation = "Lecturer" },
                new Teacher { Id = 16, Name = "MSA", Code = "MSA", Designation = "Assistant Professor" },
                new Teacher { Id = 17, Name = "CHEM", Code = "CHEM", Designation = "Assistant Professor" },
                new Teacher { Id = 18, Name = "MATH3", Code = "MATH3", Designation = "Professor" },
                new Teacher { Id = 19, Name = "MZH", Code = "MZH", Designation = "Assistant Professor" },
                new Teacher { Id = 20, Name = "MO", Code = "MO", Designation = "Lecturer" },
                new Teacher { Id = 21, Name = "RR", Code = "RR", Designation = "Assistant Professor" },
                new Teacher { Id = 22, Name = "MATH4", Code = "MATH4", Designation = "Professor" },
                new Teacher { Id = 23, Name = "ST", Code = "ST", Designation = "Assistant Professor" },
                new Teacher { Id = 24, Name = "AKZ", Code = "AKZ", Designation = "Lecturer" },
                new Teacher { Id = 25, Name = "GR", Code = "GR", Designation = "Assistant Professor" },
                new Teacher { Id = 26, Name = "EAS", Code = "EAS", Designation = "Lecturer" },
                new Teacher { Id = 27, Name = "EEE2", Code = "EEE2", Designation = "Lecturer" },
                new Teacher { Id = 28, Name = "MI", Code = "MI", Designation = "Assistant Professor" },
                new Teacher { Id = 29, Name = "TMM", Code = "TMM", Designation = "Lecturer" },
                new Teacher { Id = 30, Name = "NAO", Code = "NAO", Designation = "Lecturer" },
                new Teacher { Id = 31, Name = "NR", Code = "NR", Designation = "Assistant Professor" },
                new Teacher { Id = 32, Name = "AS", Code = "AS", Designation = "Assistant Professor" },
                new Teacher { Id = 33, Name = "JA", Code = "JA", Designation = "Lecturer" },
                new Teacher { Id = 34, Name = "AA", Code = "AA", Designation = "Lecturer" },
                new Teacher { Id = 35, Name = "AZ", Code = "AZ", Designation = "Lecturer" },
                new Teacher { Id = 36, Name = "BBA", Code = "BBA", Designation = "Lecturer" },
                new Teacher { Id = 37, Name = "SA", Code = "SA", Designation = "Lecturer" },
                new Teacher { Id = 38, Name = "NF1", Code = "NF1", Designation = "Lecturer" },
                new Teacher { Id = 39, Name = "IPE", Code = "IPE", Designation = "Assistant Professor" },
                new Teacher { Id = 40, Name = "AIS", Code = "AIS", Designation = "Lecturer" },
                new Teacher { Id = 41, Name = "RA", Code = "RA", Designation = "Lecturer" },
                new Teacher { Id = 42, Name = "MAS", Code = "MAS", Designation = "Lecturer" },
                new Teacher { Id = 43, Name = "PR", Code = "PR", Designation = "Lecturer" },
                new Teacher { Id = 44, Name = "ENG2", Code = "ENG2", Designation = "Lecturer" },
                new Teacher { Id = 45, Name = "HUM1", Code = "HUM1", Designation = "Lecturer" },
                new Teacher { Id = 46, Name = "HUM2", Code = "HUM2", Designation = "Lecturer" },
                new Teacher { Id = 47, Name = "ME", Code = "ME", Designation = "Assistant Professor" },
                new Teacher { Id = 48, Name = "NHS", Code = "NHS", Designation = "Lecturer" },
                new Teacher { Id = 49, Name = "MATH1", Code = "MATH1", Designation = "Professor" }
            };
            modelBuilder.Entity<Teacher>().HasData(teachers);

            // Courses
            var courses = new List<Course>
            {
                // Level 1, Term 1
                new Course { Id = 1, Name = "Introduction to Electrical Engineering", CourseCode = "EEE 1163", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 1 },
                new Course { Id = 2, Name = "Structured Programming Language", CourseCode = "CSE 1101", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 2 },
                new Course { Id = 3, Name = "Physics", CourseCode = "PHY 1131", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 3 },
                new Course { Id = 4, Name = "English", CourseCode = "ENG 1127", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 5 },
                new Course { Id = 5, Name = "MATH 1141", CourseCode = "MATH 1141", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 49 },

                // Level 1, Term 2
                new Course { Id = 6, Name = "Object Oriented Programming Language I", CourseCode = "CSE 1203", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 8 },
                new Course { Id = 7, Name = "Electronic Circuits", CourseCode = "EEE 1269", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 10 },
                new Course { Id = 8, Name = "MATH 1243", CourseCode = "MATH 1243", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 11 },
                new Course { Id = 9, Name = "Discrete Mathematics", CourseCode = "CSE 1201", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 14 },
                new Course { Id = 10, Name = "Bengali Language and Literature", CourseCode = "HUM 1221", Level = 1, Term = 2, Credit = 2.0f, TeacherId = 45 },

                // Level 2, Term 1
                new Course { Id = 11, Name = "Digital Logic Design", CourseCode = "CSE 2101", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 16 },
                new Course { Id = 12, Name = "Chemistry", CourseCode = "CHEM 2133", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 17 },
                new Course { Id = 13, Name = "MATH 2145", CourseCode = "MATH 2145", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 18 },
                new Course { Id = 14, Name = "Data Structures and Algorithm I", CourseCode = "CSE 2103", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 19 },
                new Course { Id = 15, Name = "Applied Statistics", CourseCode = "CSE 2105", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 20 },

                // Level 2, Term 2
                new Course { Id = 16, Name = "Data Structures and Algorithm II", CourseCode = "CSE 2201", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 21 },
                new Course { Id = 17, Name = "MATH 2247", CourseCode = "MATH 2247", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 22 },
                new Course { Id = 18, Name = "Theory of Computation", CourseCode = "CSE 2203", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 23 },
                new Course { Id = 19, Name = "Electrical Drives", CourseCode = "EEE 2269", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 26 },
                new Course { Id = 20, Name = "Database Management Systems", CourseCode = "CSE 2205", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 24 },
                new Course { Id = 21, Name = "History of Bangladesh", CourseCode = "HUM 2221", Level = 2, Term = 2, Credit = 2.0f, TeacherId = 46 },

                // Level 3, Term 1
                new Course { Id = 22, Name = "Compiler", CourseCode = "CSE 3109", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 28 },
                new Course { Id = 23, Name = "Microprocessors", CourseCode = "CSE 3103", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 29 },
                new Course { Id = 24, Name = "Data Communication", CourseCode = "CSE 3107", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 4 },
                new Course { Id = 25, Name = "Software Engineering", CourseCode = "CSE 3101", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 12 },
                new Course { Id = 26, Name = "Mechanical Engineering", CourseCode = "ME 3181", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 47 },
                new Course { Id = 27, Name = "Computer Architecture", CourseCode = "CSE 3105", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 9 },

                // Level 3, Term 2
                new Course { Id = 28, Name = "Artificial Intelligence", CourseCode = "CSE 3201", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 32 },
                new Course { Id = 29, Name = "Operating System", CourseCode = "CSE 3203", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 30 },
                new Course { Id = 30, Name = "Math Analysis", CourseCode = "CSE 3207", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 25 },
                new Course { Id = 31, Name = "Computer Networks", CourseCode = "CSE 3205", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 31 },
                new Course { Id = 32, Name = "Information System Design", CourseCode = "CSE 3209", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 13 },

                // Level 4, Term 1
                new Course { Id = 33, Name = "Machine Learning", CourseCode = "CSE 4139", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 33 },
                new Course { Id = 34, Name = "Computer Graphics", CourseCode = "CSE 4103", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 34 },
                new Course { Id = 35, Name = "Engineering Economics", CourseCode = "HUM 4123", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 36 },
                new Course { Id = 36, Name = "Object Oriented Software Eng", CourseCode = "CSE 4141", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 8 },
                new Course { Id = 37, Name = "Computer Security", CourseCode = "CSE 4101", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 35 },

                // Level 4, Term 2
                new Course { Id = 38, Name = "Industrial Management", CourseCode = "IPE 4217", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 39 },
                new Course { Id = 39, Name = "Data Warehousing", CourseCode = "CSE 4251", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 31 },
                new Course { Id = 40, Name = "Image Processing", CourseCode = "CSE 4245", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 33 },
                new Course { Id = 41, Name = "Professional Ethics", CourseCode = "CSE 4215", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 38 },
                new Course { Id = 42, Name = "Accounting", CourseCode = "HUM 4273", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 40 },
                new Course { Id = 43, Name = "VLSI Design", CourseCode = "CSE 4249", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 41 }
            };
            modelBuilder.Entity<Course>().HasData(courses);

            // Sessionals
            var sessionals = new List<Sessional>
            {
                new Sessional { Id = 1, Name = "Intro to Computer System Sessional", SessionalCode = "CSE 1100", Level = 1, Term = 1, Credit = 1.5f },
                new Sessional { Id = 2, Name = "Structured Programming Language Sessional", SessionalCode = "CSE 1102", Level = 1, Term = 1, Credit = 1.5f },
                new Sessional { Id = 3, Name = "Intro to Electrical Engineering Sessional", SessionalCode = "EEE 1164", Level = 1, Term = 1, Credit = 1.5f },
                new Sessional { Id = 4, Name = "Physics Sessional", SessionalCode = "PHY 1132", Level = 1, Term = 1, Credit = 0.75f },
                new Sessional { Id = 5, Name = "OOP I Sessional", SessionalCode = "CSE 1204", Level = 1, Term = 2, Credit = 1.5f },
                new Sessional { Id = 6, Name = "Numerical Methods Sessional", SessionalCode = "CSE 1208", Level = 1, Term = 2, Credit = 1.5f },
                new Sessional { Id = 7, Name = "Electronic Circuits Sessional", SessionalCode = "EEE 1270", Level = 1, Term = 2, Credit = 0.75f },
                new Sessional { Id = 8, Name = "Engineering Drawing Sessional", SessionalCode = "CE 1250", Level = 1, Term = 2, Credit = 0.75f },
                new Sessional { Id = 9, Name = "English Skill Sessional", SessionalCode = "ENG 1228", Level = 1, Term = 2, Credit = 0.75f },
                new Sessional { Id = 10, Name = "Digital Logic Design Sessional", SessionalCode = "CSE 2102", Level = 2, Term = 1, Credit = 1.5f },
                new Sessional { Id = 11, Name = "Data Structures I Sessional", SessionalCode = "CSE 2104", Level = 2, Term = 1, Credit = 1.5f },
                new Sessional { Id = 12, Name = "OOP II Sessional", SessionalCode = "CSE 2108", Level = 2, Term = 1, Credit = 1.5f },
                new Sessional { Id = 13, Name = "Software Development Project I", SessionalCode = "CSE 2100", Level = 2, Term = 1, Credit = 0.75f },
                new Sessional { Id = 14, Name = "Data Structures II Sessional", SessionalCode = "CSE 2202", Level = 2, Term = 2, Credit = 1.5f },
                new Sessional { Id = 15, Name = "Database Sessional", SessionalCode = "CSE 2206", Level = 2, Term = 2, Credit = 1.5f },
                new Sessional { Id = 16, Name = "Electrical Drives Sessional", SessionalCode = "EEE 2270", Level = 2, Term = 2, Credit = 0.75f },
                new Sessional { Id = 17, Name = "Software Engineering Sessional", SessionalCode = "CSE 3102", Level = 3, Term = 1, Credit = 0.75f },
                new Sessional { Id = 18, Name = "Microprocessors Sessional", SessionalCode = "CSE 3104", Level = 3, Term = 1, Credit = 0.75f },
                new Sessional { Id = 19, Name = "Compiler Sessional", SessionalCode = "CSE 3110", Level = 3, Term = 1, Credit = 0.75f },
                new Sessional { Id = 20, Name = "Software Development Project II", SessionalCode = "CSE 3100", Level = 3, Term = 1, Credit = 0.75f },
                new Sessional { Id = 21, Name = "AI Sessional", SessionalCode = "CSE 3202", Level = 3, Term = 2, Credit = 0.75f },
                new Sessional { Id = 22, Name = "OS Sessional", SessionalCode = "CSE 3204", Level = 3, Term = 2, Credit = 1.5f },
                new Sessional { Id = 23, Name = "Networks Sessional", SessionalCode = "CSE 3206", Level = 3, Term = 2, Credit = 1.5f },
                new Sessional { Id = 24, Name = "Info System Design Sessional", SessionalCode = "CSE 3210", Level = 3, Term = 2, Credit = 0.75f },
                new Sessional { Id = 25, Name = "Web Engineering Project", SessionalCode = "CSE 3200", Level = 3, Term = 2, Credit = 1.5f },
                new Sessional { Id = 26, Name = "Computer Security Sessional", SessionalCode = "CSE 4102", Level = 4, Term = 1, Credit = 0.75f },
                new Sessional { Id = 27, Name = "Computer Graphics Sessional", SessionalCode = "CSE 4104", Level = 4, Term = 1, Credit = 0.75f },
                new Sessional { Id = 28, Name = "Machine Learning Sessional", SessionalCode = "CSE 4140", Level = 4, Term = 1, Credit = 0.75f },
                new Sessional { Id = 29, Name = "OOSE Sessional", SessionalCode = "CSE 4142", Level = 4, Term = 1, Credit = 0.75f },
                new Sessional { Id = 30, Name = "Image Processing Sessional", SessionalCode = "CSE 4246", Level = 4, Term = 2, Credit = 0.75f },
                new Sessional { Id = 31, Name = "Data Mining Sessional", SessionalCode = "CSE 4252", Level = 4, Term = 2, Credit = 0.75f }
            };
            modelBuilder.Entity<Sessional>().HasData(sessionals);

            // Sessional-Teacher Many-to-Many
            modelBuilder.Entity("SessionalTeacher").HasData(
                new { SessionalsId = 1, TeachersId = 9 },  // CSE 1100 - MH
                new { SessionalsId = 1, TeachersId = 14 }, // CSE 1100 - AHS
                new { SessionalsId = 2, TeachersId = 2 },  // CSE 1102 - NHC
                new { SessionalsId = 2, TeachersId = 4 },  // CSE 1102 - MAM
                new { SessionalsId = 3, TeachersId = 7 },  // EEE 1164 - MA
                new { SessionalsId = 4, TeachersId = 3 },  // PHY 1132 - PHY
                new { SessionalsId = 5, TeachersId = 8 },  // CSE 1204 - AH
                new { SessionalsId = 5, TeachersId = 9 },  // CSE 1204 - MH
                new { SessionalsId = 5, TeachersId = 15 }, // CSE 1204 - ASM
                new { SessionalsId = 5, TeachersId = 12 }, // CSE 1204 - SG
                new { SessionalsId = 6, TeachersId = 12 }, // CSE 1208 - SG
                new { SessionalsId = 6, TeachersId = 13 }, // CSE 1208 - MSZ
                new { SessionalsId = 7, TeachersId = 26 }, // EEE 1270 - EAS
                new { SessionalsId = 8, TeachersId = 43 }, // CE 1250 - PR
                new { SessionalsId = 9, TeachersId = 44 }, // ENG 1228 - ENG2
                new { SessionalsId = 10, TeachersId = 37 }, // CSE 2102 - SA
                new { SessionalsId = 11, TeachersId = 19 }, // CSE 2104 - MZH
                new { SessionalsId = 11, TeachersId = 34 }, // CSE 2104 - AA
                new { SessionalsId = 12, TeachersId = 34 }, // CSE 2108 - AA
                new { SessionalsId = 13, TeachersId = 4 },  // CSE 2100 - MAM
                new { SessionalsId = 13, TeachersId = 30 }, // CSE 2100 - NAO
                new { SessionalsId = 14, TeachersId = 20 }, // CSE 2202 - MO
                new { SessionalsId = 14, TeachersId = 21 }, // CSE 2202 - RR
                new { SessionalsId = 15, TeachersId = 24 }, // CSE 2206 - AKZ
                new { SessionalsId = 15, TeachersId = 25 }, // CSE 2206 - GR
                new { SessionalsId = 16, TeachersId = 27 }, // EEE 2270 - EEE2
                new { SessionalsId = 16, TeachersId = 26 }, // EEE 2270 - EAS
                new { SessionalsId = 17, TeachersId = 12 }, // CSE 3102 - SG
                new { SessionalsId = 18, TeachersId = 29 }, // CSE 3104 - TMM
                new { SessionalsId = 18, TeachersId = 31 }, // CSE 3104 - NR
                new { SessionalsId = 18, TeachersId = 32 }, // CSE 3104 - AS
                new { SessionalsId = 19, TeachersId = 28 }, // CSE 3110 - MI
                new { SessionalsId = 19, TeachersId = 34 }, // CSE 3110 - AA
                new { SessionalsId = 20, TeachersId = 4 },  // CSE 3100 - MAM
                new { SessionalsId = 21, TeachersId = 32 }, // CSE 3202 - AS
                new { SessionalsId = 21, TeachersId = 28 }, // CSE 3202 - MI
                new { SessionalsId = 22, TeachersId = 37 }, // CSE 3204 - SA
                new { SessionalsId = 22, TeachersId = 30 }, // CSE 3204 - NAO
                new { SessionalsId = 23, TeachersId = 28 }, // CSE 3206 - MI
                new { SessionalsId = 23, TeachersId = 31 }, // CSE 3206 - NR
                new { SessionalsId = 24, TeachersId = 32 }, // CSE 3210 - AS
                new { SessionalsId = 24, TeachersId = 13 }, // CSE 3210 - MSZ
                new { SessionalsId = 25, TeachersId = 23 }, // CSE 3200 - ST
                new { SessionalsId = 25, TeachersId = 15 }, // CSE 3200 - ASM
                new { SessionalsId = 26, TeachersId = 35 }, // CSE 4102 - AZ
                new { SessionalsId = 26, TeachersId = 30 }, // CSE 4102 - NAO
                new { SessionalsId = 26, TeachersId = 38 }, // CSE 4102 - NF1
                new { SessionalsId = 27, TeachersId = 29 }, // CSE 4104 - TMM
                new { SessionalsId = 27, TeachersId = 34 }, // CSE 4104 - AA
                new { SessionalsId = 28, TeachersId = 16 }, // CSE 4140 - MSA
                new { SessionalsId = 28, TeachersId = 35 }, // CSE 4140 - AZ
                new { SessionalsId = 29, TeachersId = 8 },  // CSE 4142 - AH
                new { SessionalsId = 29, TeachersId = 25 }, // CSE 4142 - GR
                new { SessionalsId = 30, TeachersId = 33 }, // CSE 4246 - JA
                new { SessionalsId = 30, TeachersId = 37 }, // CSE 4246 - SA
                new { SessionalsId = 31, TeachersId = 31 }, // CSE 4252 - NR
                new { SessionalsId = 31, TeachersId = 37 }  // CSE 4252 - SA
            );

            // Sessional-Labroom Many-to-Many
            modelBuilder.Entity("SessionalLabroom").HasData(
                new { SessionalsId = 1, LabroomsId = 411 },
                new { SessionalsId = 2, LabroomsId = 411 },
                new { SessionalsId = 3, LabroomsId = 1003 },
                new { SessionalsId = 3, LabroomsId = 1002 },
                new { SessionalsId = 4, LabroomsId = 1005 },
                new { SessionalsId = 5, LabroomsId = 411 },
                new { SessionalsId = 5, LabroomsId = 311 },
                new { SessionalsId = 6, LabroomsId = 302 },
                new { SessionalsId = 6, LabroomsId = 311 },
                new { SessionalsId = 7, LabroomsId = 1004 },
                new { SessionalsId = 8, LabroomsId = 402 },
                new { SessionalsId = 9, LabroomsId = 402 },
                new { SessionalsId = 10, LabroomsId = 307 },
                new { SessionalsId = 11, LabroomsId = 302 },
                new { SessionalsId = 12, LabroomsId = 302 },
                new { SessionalsId = 12, LabroomsId = 311 },
                new { SessionalsId = 13, LabroomsId = 402 },
                new { SessionalsId = 14, LabroomsId = 302 },
                new { SessionalsId = 15, LabroomsId = 411 },
                new { SessionalsId = 15, LabroomsId = 311 },
                new { SessionalsId = 16, LabroomsId = 202 },
                new { SessionalsId = 17, LabroomsId = 302 },
                new { SessionalsId = 17, LabroomsId = 411 },
                new { SessionalsId = 18, LabroomsId = 402 },
                new { SessionalsId = 19, LabroomsId = 302 },
                new { SessionalsId = 20, LabroomsId = 302 },
                new { SessionalsId = 20, LabroomsId = 411 },
                new { SessionalsId = 21, LabroomsId = 411 },
                new { SessionalsId = 22, LabroomsId = 311 },
                new { SessionalsId = 23, LabroomsId = 311 },
                new { SessionalsId = 24, LabroomsId = 411 },
                new { SessionalsId = 25, LabroomsId = 302 },
                new { SessionalsId = 25, LabroomsId = 210 },
                new { SessionalsId = 26, LabroomsId = 311 },
                new { SessionalsId = 26, LabroomsId = 302 },
                new { SessionalsId = 27, LabroomsId = 311 },
                new { SessionalsId = 28, LabroomsId = 311 },
                new { SessionalsId = 29, LabroomsId = 311 },
                new { SessionalsId = 30, LabroomsId = 411 },
                new { SessionalsId = 31, LabroomsId = 307 }
            );

            // LevelTerms
            var levelTerms = new List<LevelTerm>
            {
                new LevelTerm { Id = 1, Level = 1, Term = 1, ClassroomId = 408 },
                new LevelTerm { Id = 2, Level = 1, Term = 2, ClassroomId = 308 },
                new LevelTerm { Id = 3, Level = 2, Term = 1, ClassroomId = 305 },
                new LevelTerm { Id = 4, Level = 2, Term = 2, ClassroomId = 309 },
                new LevelTerm { Id = 5, Level = 3, Term = 1, ClassroomId = 304 },
                new LevelTerm { Id = 6, Level = 3, Term = 2, ClassroomId = 204 },
                new LevelTerm { Id = 7, Level = 4, Term = 1, ClassroomId = 306 },
                new LevelTerm { Id = 8, Level = 4, Term = 2, ClassroomId = 407 }
            };
            modelBuilder.Entity<LevelTerm>().HasData(levelTerms);

            // Class Schedules for 4/II A
            var classSchedules4IIA = new List<ClassSchedule>
            {
                // Sunday
                new ClassSchedule { Id = 1, Day = DayOfWeek.Sunday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50), LabroomId = 307, SessionalId = 31, TeacherId = 31 },
                new ClassSchedule { Id = 16, Day = DayOfWeek.Sunday, StartTime = new TimeOnly(14, 30), EndTime = new TimeOnly(15, 20), LabroomId = 411, SessionalId = 30, TeacherId = 33 },
                
                // Monday
                new ClassSchedule { Id = 2, Day = DayOfWeek.Monday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50), ClassroomId = 407, CourseId = 38, TeacherId = 39 },
                new ClassSchedule { Id = 3, Day = DayOfWeek.Monday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 407, CourseId = 39, TeacherId = 31 },
                new ClassSchedule { Id = 4, Day = DayOfWeek.Monday, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 20), ClassroomId = 310, CourseId = 41, TeacherId = 38 },
                
                // Tuesday
                new ClassSchedule { Id = 5, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50), ClassroomId = 407, CourseId = 41, TeacherId = 38 },
                new ClassSchedule { Id = 6, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 50), ClassroomId = 407, CourseId = 39, TeacherId = 31 },
                new ClassSchedule { Id = 7, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50), ClassroomId = 407, CourseId = 40, TeacherId = 33 },
                new ClassSchedule { Id = 8, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 407, CourseId = 38, TeacherId = 39 },
                
                // Wednesday
                new ClassSchedule { Id = 9, Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 407, CourseId = 42, TeacherId = 40 },
                new ClassSchedule { Id = 10, Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 20), ClassroomId = 407, CourseId = 40, TeacherId = 33 },
                new ClassSchedule { Id = 11, Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(14, 20), ClassroomId = 407, CourseId = 38, TeacherId = 39 },
                
                // Thursday
                new ClassSchedule { Id = 12, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50), ClassroomId = 407, CourseId = 40, TeacherId = 33 },
                new ClassSchedule { Id = 13, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 50), ClassroomId = 407, CourseId = 39, TeacherId = 31 },
                new ClassSchedule { Id = 14, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50), ClassroomId = 407, CourseId = 42, TeacherId = 40 },
                new ClassSchedule { Id = 15, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 305, CourseId = 43, TeacherId = 41 }
            };
            modelBuilder.Entity<ClassSchedule>().HasData(classSchedules4IIA);
        }
    }
}