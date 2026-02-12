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
            /*modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Classroom)
                .WithMany(c => c.ClassSchedules)
                .HasForeignKey(cs => cs.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);
                
            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs=>cs.Course)
                .WithMany(c => c.)
                .HasForeignKey(cs => cs.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ClassSchedule>()
                .HasOne(cs => cs.Classroom)
                .WithMany(c => c.ClassSchedules)
                .HasForeignKey(cs => cs.ClassroomId)
                .OnDelete(DeleteBehavior.Restrict);*/
            var ClassRooms = new List<Classroom>
            {
                new Classroom { Id=204, },
                new Classroom { Id=205, },
                new Classroom { Id=304, },
                new Classroom { Id=305, },
                new Classroom { Id=306, },
                new Classroom { Id=308, },
                new Classroom { Id=309, },
                new Classroom { Id=310, },
                new Classroom { Id=311, },
                new Classroom { Id=407, },
                new Classroom { Id=408, },
                new Classroom { Id=1001, },//Seminal Hall
            };
            modelBuilder.Entity<Classroom>().HasData(ClassRooms);

            var Labrooms = new List<Labroom>
            {
                new Labroom { Id=202, Name="EEE" },
                new Labroom { Id=210, Name="CSE" },
                new Labroom { Id=302, Name="CSE" },
                new Labroom { Id=307, Name="CSE" },
                new Labroom { Id=311, Name="CSE" },
                new Labroom { Id=402, Name="CSE, CAD" },
                new Labroom { Id=411, Name="CSE" },
                new Labroom { Id=1001, Name="AC Circuit Lab" },
                new Labroom { Id=1002, Name="DC Circuit Lab" },
                new Labroom { Id=1003, Name="AC Circuit Lab" },
                new Labroom { Id=1004, Name="Electronics Lab" },
                new Labroom { Id=1005, Name="Physics Lab" },
            };
            modelBuilder.Entity<Labroom>().HasData(Labrooms);

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
                new Teacher { Id = 48, Name = "NHS", Code = "NHS", Designation = "Lecturer" }
            };
            modelBuilder.Entity<Teacher>().HasData(teachers);

            var courses = new List<Course>
            {
                // Level 1, Term 1 - Section A
                new Course { Id = 1, Name = "Introduction to Electrical Engineering", CourseCode = "EEE 1163", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 1 }, // TQA (Id: 1)
                new Course { Id = 2, Name = "Structured Programming Language", CourseCode = "CSE 1101", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 2 }, // NHC (Id: 2)
                new Course { Id = 3, Name = "Physics", CourseCode = "PHY 1131", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 3 }, // PHY (Id: 3)
                new Course { Id = 4, Name = "English", CourseCode = "ENG 1127", Level = 1, Term = 1, Credit = 3.0f, TeacherId = 5 }, // ENG1 (Id: 5)
                new Course { Id = 5, Name = "Differential Calculus, Integral Calculus, and Coordinate Geometry", CourseCode = "MATH 1141", Level = 1, Term = 1, Credit = 3.0f, TeacherId = null }, // MATH1 (Not in teacher list)
                
                // Level 1, Term 2 - Section A
                new Course { Id = 6, Name = "Object Oriented Programming Language I", CourseCode = "CSE 1203", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 8 }, // AH (Id: 8)
                new Course { Id = 7, Name = "Electronic Circuits", CourseCode = "EEE 1269", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 10 }, // EMH (Id: 10)
                new Course { Id = 8, Name = "Ordinary Differential Equations and Partial Differential Equations", CourseCode = "MATH 1243", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 11 }, // MATH2 (Id: 11)
                new Course { Id = 9, Name = "Discrete Mathematics", CourseCode = "CSE 1201", Level = 1, Term = 2, Credit = 3.0f, TeacherId = 14 }, // AHS (Id: 14)
                new Course { Id = 10, Name = "Bengali Language and Literature", CourseCode = "HUM 1221", Level = 1, Term = 2, Credit = 2.0f, TeacherId = 45 }, // HUM1 (Id: 45)
                
                // Level 2, Term 1 - Section A
                new Course { Id = 11, Name = "Digital Logic Design", CourseCode = "CSE 2101", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 16 }, // MSA (Id: 16)
                new Course { Id = 12, Name = "Chemistry", CourseCode = "CHEM 2133", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 17 }, // CHEM (Id: 17)
                new Course { Id = 13, Name = "Vector Calculus, Linear Algebra and Complex Variable", CourseCode = "MATH 2145", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 18 }, // MATH3 (Id: 18)
                new Course { Id = 14, Name = "Data Structures and Algorithm I", CourseCode = "CSE 2103", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 19 }, // MZH (Id: 19)
                new Course { Id = 15, Name = "Applied Statistics for Computer Science", CourseCode = "CSE 2105", Level = 2, Term = 1, Credit = 3.0f, TeacherId = 20 }, // MO (Id: 20)
                
                // Level 2, Term 2 - Section A
                new Course { Id = 16, Name = "Data Structures and Algorithm II", CourseCode = "CSE 2201", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 21 }, // RR (Id: 21)
                new Course { Id = 17, Name = "Laplace Transformation and Fourier Analysis", CourseCode = "MATH 2247", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 22 }, // MATH4 (Id: 22)
                new Course { Id = 18, Name = "Theory of Computation", CourseCode = "CSE 2203", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 23 }, // ST (Id: 23)
                new Course { Id = 19, Name = "Electrical Drives and Instrumentation", CourseCode = "EEE 2269", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 26 }, // EAS (Id: 26)
                new Course { Id = 20, Name = "Database Management Systems", CourseCode = "CSE 2205", Level = 2, Term = 2, Credit = 3.0f, TeacherId = 24 }, // AKZ (Id: 24)
                new Course { Id = 21, Name = "History of the Emergence of Bangladesh", CourseCode = "HUM 2221", Level = 2, Term = 2, Credit = 2.0f, TeacherId = 46 }, // HUM2 (Id: 46)
                
                // Level 3, Term 1 - Section A
                new Course { Id = 22, Name = "Compiler", CourseCode = "CSE 3109", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 28 }, // MI (Id: 28)
                new Course { Id = 23, Name = "Microprocessors, Microcontrollers and Embedded Systems", CourseCode = "CSE 3103", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 29 }, // TMM (Id: 29)
                new Course { Id = 24, Name = "Data Communication", CourseCode = "CSE 3107", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 4 }, // MAM (Id: 4)
                new Course { Id = 25, Name = "Software Engineering", CourseCode = "CSE 3101", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 12 }, // SG (Id: 12)
                new Course { Id = 26, Name = "Basic Mechanical Engineering", CourseCode = "ME 3181", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 47 }, // ME (Id: 47)
                new Course { Id = 27, Name = "Computer Architecture", CourseCode = "CSE 3105", Level = 3, Term = 1, Credit = 3.0f, TeacherId = 9 }, // MH (Id: 9)
                
                // Level 3, Term 2 - Section A
                new Course { Id = 28, Name = "Artificial Intelligence", CourseCode = "CSE 3201", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 32 }, // AS (Id: 32)
                new Course { Id = 29, Name = "Operating System", CourseCode = "CSE 3203", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 30 }, // NAO (Id: 30)
                new Course { Id = 30, Name = "Mathematical Analysis for Computer Science", CourseCode = "CSE 3207", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 25 }, // GR (Id: 25)
                new Course { Id = 31, Name = "Computer Networks", CourseCode = "CSE 3205", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 31 }, // NR (Id: 31)
                new Course { Id = 32, Name = "Information System Design", CourseCode = "CSE 3209", Level = 3, Term = 2, Credit = 3.0f, TeacherId = 13 }, // MSZ (Id: 13)
                new Course { Id = 33, Name = "Industrial Management", CourseCode = "IPE 4217", Level = 3, Term = 2, Credit = 2.0f, TeacherId = 39 }, // IPE (Id: 39)
                
                // Level 4, Term 1 - Section A
                new Course { Id = 34, Name = "Machine Learning", CourseCode = "CSE 4139", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 33 }, // JA (Id: 33)
                new Course { Id = 35, Name = "Computer Graphics", CourseCode = "CSE 4103", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 34 }, // AA (Id: 34)
                new Course { Id = 36, Name = "Engineering Economics", CourseCode = "HUM 4123", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 36 }, // BBA (Id: 36)
                new Course { Id = 37, Name = "Object Oriented Software Engineering", CourseCode = "CSE 4141", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 8 }, // AH (Id: 8) - Same as CSE 1203
                new Course { Id = 38, Name = "Computer Security", CourseCode = "CSE 4101", Level = 4, Term = 1, Credit = 3.0f, TeacherId = 35 }, // AZ (Id: 35)
                
                // Level 4, Term 2 - Section A
                new Course { Id = 39, Name = "Data Warehousing and Data Mining", CourseCode = "CSE 4251", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 31 }, // NR (Id: 31) - Same as CSE 3205
                new Course { Id = 40, Name = "Digital Image Processing", CourseCode = "CSE 4245", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 33 }, // JA (Id: 33) - Same as CSE 4139
                new Course { Id = 41, Name = "Professional Issues and Ethics in Computer Science", CourseCode = "CSE 4215", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 38 }, // NF1 (Id: 38)
                new Course { Id = 42, Name = "Financial, Cost and Managerial Accounting", CourseCode = "HUM 4273", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 40 }, // AIS (Id: 40)
                new Course { Id = 43, Name = "VLSI Design", CourseCode = "CSE 4249", Level = 4, Term = 2, Credit = 2.0f, TeacherId = 41 } // RA (Id: 41)
            };
            modelBuilder.Entity<Course>().HasData(courses);

            var sessionals = new List<Sessional>
            {
                // Level 1, Term 1 - Sessionals (Section A)
                new Sessional { Id = 1, Name = "Introduction to Computer System Sessional", SessionalCode = "CSE 1100", Level = 1, Term = 1, Credit = 1.5f, TeacherId = 9 }, // MH (Id: 9)  
                new Sessional { Id = 2, Name = "Structured Programming Language Sessional", SessionalCode = "CSE 1102", Level = 1, Term = 1, Credit = 1.5f, TeacherId = 2 }, // NHC (Id: 2)  
                new Sessional { Id = 3, Name = "Introduction to Electrical Engineering Sessional", SessionalCode = "EEE 1164", Level = 1, Term = 1, Credit = 1.5f, TeacherId = 7 }, // MA (Id: 7)
                new Sessional { Id = 4, Name = "Physics Sessional", SessionalCode = "PHY 1132", Level = 1, Term = 1, Credit = 0.75f, TeacherId = 3 }, // PHY (Id: 3)
                
                // Level 1, Term 2 - Sessionals (Section A)
                new Sessional { Id = 5, Name = "Object Oriented Programming Language I Sessional", SessionalCode = "CSE 1204", Level = 1, Term = 2, Credit = 1.5f, TeacherId = 8 }, // AH (Id: 8)  
                new Sessional { Id = 6, Name = "Numerical Methods Sessional", SessionalCode = "CSE 1208", Level = 1, Term = 2, Credit = 1.5f, TeacherId = 12 }, // SG (Id: 12)  
                new Sessional { Id = 7, Name = "Electronic Circuits Sessional", SessionalCode = "EEE 1270", Level = 1, Term = 2, Credit = 0.75f, TeacherId = 26 }, // EAS (Id: 26)
                new Sessional { Id = 8, Name = "Engineering Drawing and CAD Sessional", SessionalCode = "CE 1250", Level = 1, Term = 2, Credit = 0.75f, TeacherId = 43 }, // PR (Id: 43)
                new Sessional { Id = 9, Name = "Developing English Skill Sessional", SessionalCode = "ENG 1228", Level = 1, Term = 2, Credit = 0.75f, TeacherId = 44 }, // ENG2 (Id: 44)
                
                // Level 2, Term 1 - Sessionals (Section A)
                new Sessional { Id = 10, Name = "Digital Logic Design Sessional", SessionalCode = "CSE 2102", Level = 2, Term = 1, Credit = 1.5f, TeacherId = 37 }, // SA (Id: 37)
                new Sessional { Id = 11, Name = "Data Structures and Algorithm I Sessional", SessionalCode = "CSE 2104", Level = 2, Term = 1, Credit = 1.5f, TeacherId = 19 }, // MZH (Id: 19)  
                new Sessional { Id = 12, Name = "Object Oriented Programming Language II Sessional", SessionalCode = "CSE 2108", Level = 2, Term = 1, Credit = 1.5f, TeacherId = 34 }, // AA (Id: 34)
                new Sessional { Id = 13, Name = "Software Development Project I", SessionalCode = "CSE 2100", Level = 2, Term = 1, Credit = 0.75f, TeacherId = 4 }, // MAM (Id: 4)  
                
                // Level 2, Term 2 - Sessionals (Section A)
                new Sessional { Id = 14, Name = "Data Structures and Algorithm II Sessional", SessionalCode = "CSE 2202", Level = 2, Term = 2, Credit = 1.5f, TeacherId = 20 }, // MO (Id: 20)  
                new Sessional { Id = 15, Name = "Database Management Systems Sessional", SessionalCode = "CSE 2206", Level = 2, Term = 2, Credit = 1.5f, TeacherId = 24 }, // AKZ (Id: 24)  
                new Sessional { Id = 16, Name = "Electrical Drives and Instrumentation Sessional", SessionalCode = "EEE 2270", Level = 2, Term = 2, Credit = 0.75f, TeacherId = 27 }, // EEE2 (Id: 27)
                
                // Level 3, Term 1 - Sessionals (Section A)
                new Sessional { Id = 17, Name = "Software Engineering Sessional", SessionalCode = "CSE 3102", Level = 3, Term = 1, Credit = 0.75f, TeacherId = 12 }, // SG (Id: 12)  
                new Sessional { Id = 18, Name = "Microprocessors, Microcontrollers and Embedded Systems Sessional", SessionalCode = "CSE 3104", Level = 3, Term = 1, Credit = 0.75f, TeacherId = 29 }, // TMM (Id: 29)  
                new Sessional { Id = 19, Name = "Compiler Sessional", SessionalCode = "CSE 3110", Level = 3, Term = 1, Credit = 0.75f, TeacherId = 28 }, // MI (Id: 28)  
                new Sessional { Id = 20, Name = "Software Development Project II", SessionalCode = "CSE 3100", Level = 3, Term = 1, Credit = 0.75f, TeacherId = 4 }, // MAM (Id: 4)  
                
                // Level 3, Term 2 - Sessionals (Section A)
                new Sessional { Id = 21, Name = "Artificial Intelligence Sessional", SessionalCode = "CSE 3202", Level = 3, Term = 2, Credit = 0.75f, TeacherId = 32 }, // AS (Id: 32)  
                new Sessional { Id = 22, Name = "Operating System Sessional", SessionalCode = "CSE 3204", Level = 3, Term = 2, Credit = 1.5f, TeacherId = 37 }, // SA (Id: 37)  
                new Sessional { Id = 23, Name = "Computer Networks Sessional", SessionalCode = "CSE 3206", Level = 3, Term = 2, Credit = 1.5f, TeacherId = 28 }, // MI (Id: 28)  
                new Sessional { Id = 24, Name = "Information System Design Sessional", SessionalCode = "CSE 3210", Level = 3, Term = 2, Credit = 0.75f, TeacherId = 32 }, // AS (Id: 32)  
                new Sessional { Id = 25, Name = "Web Engineering Project", SessionalCode = "CSE 3200", Level = 3, Term = 2, Credit = 1.5f, TeacherId = 23 }, // ST (Id: 23)  
                
                // Level 4, Term 1 - Sessionals (Section A)
                new Sessional { Id = 26, Name = "Computer Security Sessional", SessionalCode = "CSE 4102", Level = 4, Term = 1, Credit = 0.75f, TeacherId = 35 }, // AZ (Id: 35)  
                new Sessional { Id = 27, Name = "Computer Graphics Sessional", SessionalCode = "CSE 4104", Level = 4, Term = 1, Credit = 0.75f, TeacherId = 29 }, // TMM (Id: 29)  
                new Sessional { Id = 28, Name = "Machine Learning Sessional", SessionalCode = "CSE 4140", Level = 4, Term = 1, Credit = 0.75f, TeacherId = 16 }, // MSA (Id: 16)  
                new Sessional { Id = 29, Name = "Object Oriented Software Engineering Sessional", SessionalCode = "CSE 4142", Level = 4, Term = 1, Credit = 0.75f, TeacherId = 8 }, // AH (Id: 8)  
                
                // Level 4, Term 2 - Sessionals (Section A)
                new Sessional { Id = 30, Name = "Digital Image Processing Sessional", SessionalCode = "CSE 4246", Level = 4, Term = 2, Credit = 0.75f, TeacherId = 33 }, // JA (Id: 33)  
                new Sessional { Id = 31, Name = "Data Warehousing and Data Mining Sessional", SessionalCode = "CSE 4252", Level = 4, Term = 2, Credit = 0.75f, TeacherId = 31 } // NR (Id: 31)  
            };
            modelBuilder.Entity<Sessional>().HasData(sessionals);

            var levelTerms = new List<LevelTerm>
            {
                new LevelTerm { Id = 1, Level = 1, Term = 1, ClassroomId = 408 }, // 1/I
                new LevelTerm { Id = 2, Level = 1, Term = 2, ClassroomId = 308 }, // 1/II
                new LevelTerm { Id = 3, Level = 2, Term = 1, ClassroomId = 305 }, // 2/I
                new LevelTerm { Id = 4, Level = 2, Term = 2, ClassroomId = 309 }, // 2/II
                new LevelTerm { Id = 5, Level = 3, Term = 1, ClassroomId = 304 }, // 3/I
                new LevelTerm { Id = 6, Level = 3, Term = 2, ClassroomId = 204 }, // 3/II
                new LevelTerm { Id = 7, Level = 4, Term = 1, ClassroomId = 306 }, // 4/I
                new LevelTerm { Id = 8, Level = 4, Term = 2, ClassroomId = 407 }  // 4/II
            };
            modelBuilder.Entity<LevelTerm>().HasData(levelTerms);

            var classSchedules4IIA = new List<ClassSchedule>
            {
                // SUNDAY
                new ClassSchedule { Id = 1, Day = DayOfWeek.Sunday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50), LabroomId = 307, SessionalId = 31, TeacherId = 31 }, // CSE 4252: Data Warehousing and Data Mining Sessional
                
                // MONDAY
                new ClassSchedule { Id = 2, Day = DayOfWeek.Monday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50), ClassroomId = 407, CourseId = 33, TeacherId = 39 }, // IPE 4217: Industrial Management
                new ClassSchedule { Id = 3, Day = DayOfWeek.Monday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 407, CourseId = 39, TeacherId = 31 }, // CSE 4251: Data Warehousing and Data Mining
                new ClassSchedule { Id = 4, Day = DayOfWeek.Monday, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 20), ClassroomId = 310, CourseId = 41, TeacherId = 38 }, // CSE 4215: Professional Issues and Ethics in Computer Science [310]
                
                // TUESDAY
                new ClassSchedule { Id = 5, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50), ClassroomId = 407, CourseId = 41, TeacherId = 38 }, // CSE 4215: Professional Issues and Ethics in Computer Science [407]
                new ClassSchedule { Id = 6, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 50), ClassroomId = 407, CourseId = 39, TeacherId = 31 }, // CSE 4251: Data Warehousing and Data Mining [407]
                new ClassSchedule { Id = 7, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50), ClassroomId = 407, CourseId = 40, TeacherId = 33 }, // CSE 4245: Digital Image Processing [407]
                new ClassSchedule { Id = 8, Day = DayOfWeek.Tuesday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 407, CourseId = 33, TeacherId = 39 }, // IPE 4217: Industrial Management [407]
                
                // WEDNESDAY
                new ClassSchedule { Id = 9, Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 407, CourseId = 42, TeacherId = 40 }, // HUM 4273: Financial, Cost and Managerial Accounting [407]
                new ClassSchedule { Id = 10, Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(12, 30), EndTime = new TimeOnly(13, 20), ClassroomId = 407, CourseId = 40, TeacherId = 33 }, // CSE 4245: Digital Image Processing [407]
                new ClassSchedule { Id = 11, Day = DayOfWeek.Wednesday, StartTime = new TimeOnly(13, 30), EndTime = new TimeOnly(14, 20), ClassroomId = 407, CourseId = 33, TeacherId = 39 }, // IPE 4217: Industrial Management [407]
                
                // THURSDAY
                new ClassSchedule { Id = 12, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(8, 0), EndTime = new TimeOnly(8, 50), ClassroomId = 407, CourseId = 40, TeacherId = 33 }, // CSE 4245: Digital Image Processing [407]
                new ClassSchedule { Id = 13, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(9, 0), EndTime = new TimeOnly(9, 50), ClassroomId = 407, CourseId = 39, TeacherId = 31 }, // CSE 4251: Data Warehousing and Data Mining [407]
                new ClassSchedule { Id = 14, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(10, 0), EndTime = new TimeOnly(10, 50), ClassroomId = 407, CourseId = 42, TeacherId = 40 }, // HUM 4273: Financial, Cost and Managerial Accounting [407]
                new ClassSchedule { Id = 15, Day = DayOfWeek.Thursday, StartTime = new TimeOnly(11, 30), EndTime = new TimeOnly(12, 20), ClassroomId = 305, CourseId = 43, TeacherId = 41 }, // CSE 4249: VLSI Design [305]
                
                // NOTE: CSE 4246: Digital Image Processing Sessional is NOT scheduled in the routine
            };
            modelBuilder.Entity<ClassSchedule>().HasData(classSchedules4IIA);
        }
    }
}