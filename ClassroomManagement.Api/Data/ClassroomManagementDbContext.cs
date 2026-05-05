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
        public DbSet<LevelTermSection> LevelTermSections { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }

        public DbSet<Dictionary<string, object>> SessionalTeacher { get; set; }
        public DbSet<Dictionary<string, object>> SessionalLabroom { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Classrooms (for theory classes)
            var ClassRooms = new List<Classroom>
            {
                new Classroom { Id = 1, RoomNumber = 204 }, // 3/II - A
                new Classroom { Id = 2, RoomNumber = 205 }, // 3/II - B
                new Classroom { Id = 3, RoomNumber = 304 }, // 3/I - A
                new Classroom { Id = 4, RoomNumber = 305 }, // 4/II - A
                new Classroom { Id = 5, RoomNumber = 306 }, // 4/I - A
                new Classroom { Id = 6, RoomNumber = 308 }, // 1/I - B
                new Classroom { Id = 7, RoomNumber = 309 }, // 2/II - A
                new Classroom { Id = 8, RoomNumber = 310 }, // 2/I - B
                new Classroom { Id = 9, RoomNumber = 311 }, // General
                new Classroom { Id = 10, RoomNumber = 407 }, // 4/II - A
                new Classroom { Id = 11, RoomNumber = 408 }, // 1/I - A
                new Classroom { Id = 12, RoomNumber = 402 }, // 4/I - A (ADD THIS)
                new Classroom { Id = 13, RoomNumber = 502 }, // English
                new Classroom { Id = 14, RoomNumber = 506 }, // English
                new Classroom { Id = 15, RoomNumber = 507 }, // BBA
                new Classroom { Id = 16, RoomNumber = 510 }, // English
                new Classroom { Id = 17, RoomNumber = 1001 }, // Seminar Hall
            };
            modelBuilder.Entity<Classroom>().HasData(ClassRooms);

            // Labrooms (for practical sessions)
            var Labrooms = new List<Labroom>
            {
                new Labroom { Id = 1, RoomNumber = 14, Name = "Room 014" },      // For CE: 2/I
                new Labroom { Id = 2, RoomNumber = 24, Name = "Room 024" },      // For IPE: 1/II
                new Labroom { Id = 3, RoomNumber = 107, Name = "Room 107" },     // For IPE: 1/II
                new Labroom { Id = 4, RoomNumber = 108, Name = "Room 108" },     // For ME: 1/II
                new Labroom { Id = 5, RoomNumber = 110, Name = "Room 110" },     // For ME, IPE labs
                new Labroom { Id = 6, RoomNumber = 202, Name = "EEE Lab" },      // EEE Lab
                new Labroom { Id = 7, RoomNumber = 206, Name = "Room 206" },     // For EEE: 2/I
                new Labroom { Id = 8, RoomNumber = 210, Name = "CSE Lab 210" },  // CSE Lab
                new Labroom { Id = 9, RoomNumber = 302, Name = "CSE Lab 302" },  // CSE Lab
                new Labroom { Id = 10, RoomNumber = 307, Name = "CSE Lab 307" }, // CSE Lab
                new Labroom { Id = 11, RoomNumber = 311, Name = "CSE Lab 311" }, // CSE Lab
                new Labroom { Id = 12, RoomNumber = 402, Name = "CSE/CAD Lab 402" }, // CAD Lab
                new Labroom { Id = 13, RoomNumber = 411, Name = "CSE Lab 411" }, // CSE Lab
                new Labroom { Id = 14, RoomNumber = 1001, Name = "AC Circuit Lab" },
                new Labroom { Id = 15, RoomNumber = 1002, Name = "DC Circuit Lab" },
                new Labroom { Id = 16, RoomNumber = 1003, Name = "AC Circuit Lab" },
                new Labroom { Id = 17, RoomNumber = 1004, Name = "Electronics Lab" },
                new Labroom { Id = 18, RoomNumber = 1005, Name = "Physics Lab" },
                new Labroom { Id = 19, RoomNumber = 109, Name = "IPE Computer Lab" },
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
                new Course { Id = 1, Name = "Introduction to Electrical Engineering", CourseCode = "EEE 1163", Level = 1, Term = 1, Credit = 3.0f },
                new Course { Id = 2, Name = "Structured Programming Language", CourseCode = "CSE 1101", Level = 1, Term = 1, Credit = 3.0f },
                new Course { Id = 3, Name = "Physics", CourseCode = "PHY 1131", Level = 1, Term = 1, Credit = 3.0f },
                new Course { Id = 4, Name = "English", CourseCode = "ENG 1127", Level = 1, Term = 1, Credit = 3.0f },
                new Course { Id = 5, Name = "MATH 1141", CourseCode = "MATH 1141", Level = 1, Term = 1, Credit = 3.0f },

                // Level 1, Term 2
                new Course { Id = 6, Name = "Object Oriented Programming Language I", CourseCode = "CSE 1203", Level = 1, Term = 2, Credit = 3.0f },
                new Course { Id = 7, Name = "Electronic Circuits", CourseCode = "EEE 1269", Level = 1, Term = 2, Credit = 3.0f },
                new Course { Id = 8, Name = "MATH 1243", CourseCode = "MATH 1243", Level = 1, Term = 2, Credit = 3.0f },
                new Course { Id = 9, Name = "Discrete Mathematics", CourseCode = "CSE 1201", Level = 1, Term = 2, Credit = 3.0f },
                new Course { Id = 10, Name = "Bengali Language and Literature", CourseCode = "HUM 1221", Level = 1, Term = 2, Credit = 2.0f },

                // Level 2, Term 1
                new Course { Id = 11, Name = "Digital Logic Design", CourseCode = "CSE 2101", Level = 2, Term = 1, Credit = 3.0f },
                new Course { Id = 12, Name = "Chemistry", CourseCode = "CHEM 2133", Level = 2, Term = 1, Credit = 3.0f },
                new Course { Id = 13, Name = "MATH 2145", CourseCode = "MATH 2145", Level = 2, Term = 1, Credit = 3.0f },
                new Course { Id = 14, Name = "Data Structures and Algorithm I", CourseCode = "CSE 2103", Level = 2, Term = 1, Credit = 3.0f },
                new Course { Id = 15, Name = "Applied Statistics", CourseCode = "CSE 2105", Level = 2, Term = 1, Credit = 3.0f },

                // Level 2, Term 2
                new Course { Id = 16, Name = "Data Structures and Algorithm II", CourseCode = "CSE 2201", Level = 2, Term = 2, Credit = 3.0f },
                new Course { Id = 17, Name = "MATH 2247", CourseCode = "MATH 2247", Level = 2, Term = 2, Credit = 3.0f },
                new Course { Id = 18, Name = "Theory of Computation", CourseCode = "CSE 2203", Level = 2, Term = 2, Credit = 3.0f },
                new Course { Id = 19, Name = "Electrical Drives", CourseCode = "EEE 2269", Level = 2, Term = 2, Credit = 3.0f },
                new Course { Id = 20, Name = "Database Management Systems", CourseCode = "CSE 2205", Level = 2, Term = 2, Credit = 3.0f },
                new Course { Id = 21, Name = "History of Bangladesh", CourseCode = "HUM 2221", Level = 2, Term = 2, Credit = 2.0f },

                // Level 3, Term 1
                new Course { Id = 22, Name = "Compiler", CourseCode = "CSE 3109", Level = 3, Term = 1, Credit = 3.0f },
                new Course { Id = 23, Name = "Microprocessors", CourseCode = "CSE 3103", Level = 3, Term = 1, Credit = 3.0f },
                new Course { Id = 24, Name = "Data Communication", CourseCode = "CSE 3107", Level = 3, Term = 1, Credit = 3.0f },
                new Course { Id = 25, Name = "Software Engineering", CourseCode = "CSE 3101", Level = 3, Term = 1, Credit = 3.0f },
                new Course { Id = 26, Name = "Mechanical Engineering", CourseCode = "ME 3181", Level = 3, Term = 1, Credit = 3.0f },
                new Course { Id = 27, Name = "Computer Architecture", CourseCode = "CSE 3105", Level = 3, Term = 1, Credit = 3.0f },

                // Level 3, Term 2
                new Course { Id = 28, Name = "Artificial Intelligence", CourseCode = "CSE 3201", Level = 3, Term = 2, Credit = 3.0f },
                new Course { Id = 29, Name = "Operating System", CourseCode = "CSE 3203", Level = 3, Term = 2, Credit = 3.0f },
                new Course { Id = 30, Name = "Math Analysis", CourseCode = "CSE 3207", Level = 3, Term = 2, Credit = 3.0f },
                new Course { Id = 31, Name = "Computer Networks", CourseCode = "CSE 3205", Level = 3, Term = 2, Credit = 3.0f },
                new Course { Id = 32, Name = "Information System Design", CourseCode = "CSE 3209", Level = 3, Term = 2, Credit = 3.0f },

                // Level 4, Term 1
                new Course { Id = 33, Name = "Machine Learning", CourseCode = "CSE 4139", Level = 4, Term = 1, Credit = 3.0f },
                new Course { Id = 34, Name = "Computer Graphics", CourseCode = "CSE 4103", Level = 4, Term = 1, Credit = 3.0f },
                new Course { Id = 35, Name = "Engineering Economics", CourseCode = "HUM 4123", Level = 4, Term = 1, Credit = 3.0f },
                new Course { Id = 36, Name = "Object Oriented Software Eng", CourseCode = "CSE 4141", Level = 4, Term = 1, Credit = 3.0f },
                new Course { Id = 37, Name = "Computer Security", CourseCode = "CSE 4101", Level = 4, Term = 1, Credit = 3.0f },

                // Level 4, Term 2
                new Course { Id = 38, Name = "Industrial Management", CourseCode = "IPE 4217", Level = 4, Term = 2, Credit = 2.0f },
                new Course { Id = 39, Name = "Data Warehousing", CourseCode = "CSE 4251", Level = 4, Term = 2, Credit = 2.0f },
                new Course { Id = 40, Name = "Image Processing", CourseCode = "CSE 4245", Level = 4, Term = 2, Credit = 2.0f },
                new Course { Id = 41, Name = "Professional Ethics", CourseCode = "CSE 4215", Level = 4, Term = 2, Credit = 2.0f },
                new Course { Id = 42, Name = "Accounting", CourseCode = "HUM 4273", Level = 4, Term = 2, Credit = 2.0f },
                new Course { Id = 43, Name = "VLSI Design", CourseCode = "CSE 4249", Level = 4, Term = 2, Credit = 2.0f }
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

            // LevelTermSections
            var levelTermSections = new List<LevelTermSection>
            {
                // Level 1, Term 1 - Sections A, B
                new LevelTermSection { Id = 1, Level = 1, Term = 1, Section = "A" },
                new LevelTermSection { Id = 2, Level = 1, Term = 1, Section = "B" },
                
                // Level 1, Term 2 - Sections A, B
                new LevelTermSection { Id = 3, Level = 1, Term = 2, Section = "A" },
                new LevelTermSection { Id = 4, Level = 1, Term = 2, Section = "B" },
                
                // Level 2, Term 1 - Sections A, B
                new LevelTermSection { Id = 5, Level = 2, Term = 1, Section = "A" },
                new LevelTermSection { Id = 6, Level = 2, Term = 1, Section = "B" },
                
                // Level 2, Term 2 - Sections A, B, C
                new LevelTermSection { Id = 7, Level = 2, Term = 2, Section = "A" },
                new LevelTermSection { Id = 8, Level = 2, Term = 2, Section = "B" },
                //new LevelTermSection { Id = 9, Level = 2, Term = 2, Section = "C" },
                
                // Level 3, Term 1 - Sections A, B
                new LevelTermSection { Id = 10, Level = 3, Term = 1, Section = "A" },
                new LevelTermSection { Id = 11, Level = 3, Term = 1, Section = "B" },
                
                // Level 3, Term 2 - Sections A, B
                new LevelTermSection { Id = 12, Level = 3, Term = 2, Section = "A" },
                new LevelTermSection { Id = 13, Level = 3, Term = 2, Section = "B" },
                
                // Level 4, Term 1 - Sections A, B
                new LevelTermSection { Id = 14, Level = 4, Term = 1, Section = "A" },
                new LevelTermSection { Id = 15, Level = 4, Term = 1, Section = "B" },
                
                // Level 4, Term 2 - Section A only
                new LevelTermSection { Id = 16, Level = 4, Term = 2, Section = "A" },
                new LevelTermSection { Id = 17, Level = 4, Term = 2, Section = "B" },
            };
            modelBuilder.Entity<LevelTermSection>().HasData(levelTermSections);
        }
    }
}