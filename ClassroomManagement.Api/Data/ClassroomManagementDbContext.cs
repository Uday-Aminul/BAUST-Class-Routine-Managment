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
        public DbSet<Course> Courses { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<ClassSchedule> ClassSchedules { get; set; }
        public DbSet<Labroom> Labrooms { get; set; }
        public DbSet<Sessional> Sessionals { get; set; }

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
                new Classroom { Id=304, },
                new Classroom { Id=305, },
                new Classroom { Id=306, },
                new Classroom { Id=308, },
                new Classroom { Id=309, },
                new Classroom { Id=310, },
            };
            modelBuilder.Entity<Classroom>().HasData(ClassRooms);

            //These need to be actual teachers. and Courses navigation property.

            // The names need to be actual labroom names.
            var Labrooms = new List<Labroom>
            {
                new Labroom { Id=302, Name="Computer Lab A" },
                new Labroom { Id=307, Name="Computer Lab B" },
                new Labroom { Id=311, Name="Computer Lab c" },
            };
            modelBuilder.Entity<Labroom>().HasData(Labrooms);


            // The details need to be actual.
            var Teachers = new List<Teacher>
            {
                new Teacher { Id=101, Name="A", Designation="Associate Professor" },
                new Teacher { Id=102, Name="B", Designation="Associate Professor" },
                new Teacher { Id=103, Name="C", Designation="Associate Professor" },
                new Teacher { Id=104, Name="E", Designation="Associate Professor" },
                new Teacher { Id=105, Name="F", Designation="Lecturer" },
                new Teacher { Id=106, Name="G", Designation="Lecturer" },
                new Teacher { Id=107, Name="H", Designation="Lecturer" },
                new Teacher { Id=108, Name="I", Designation="Lecturer" },
                new Teacher { Id=109, Name="J", Designation="Lecturer" },
                new Teacher { Id=110, Name="K", Designation="Lecturer" },
                new Teacher { Id=111, Name="L", Designation="Lecturer" },
                new Teacher { Id=112, Name="M", Designation="Lecturer" },
                new Teacher { Id=113, Name="N", Designation="Lecturer" },
                new Teacher { Id=114, Name="O", Designation="Lecturer" },
                new Teacher { Id=115, Name="P", Designation="Lecturer" },
                new Teacher { Id=116, Name="Q", Designation="Lecturer" },
                new Teacher { Id=117, Name="R", Designation="Lecturer" },
                new Teacher { Id=118, Name="S", Designation="Lecturer" },
                new Teacher { Id=119, Name="T", Designation="Lecturer" },
                new Teacher { Id=120, Name="U", Designation="Lecturer" },
                new Teacher { Id=121, Name="V", Designation="Lecturer" },
                new Teacher { Id=122, Name="W", Designation="Lecturer" },
                new Teacher { Id=123, Name="X", Designation="Lecturer" },
                new Teacher { Id=124, Name="Y", Designation="Lecturer" },
                new Teacher { Id=125, Name="Z", Designation="Lecturer" },
                new Teacher { Id=126, Name="AA", Designation="Assistant Lecturer" },
                new Teacher { Id=127, Name="BB", Designation="Assistant Lecturer" },
                new Teacher { Id=128, Name="CC", Designation="Assistant Lecturer" },
                new Teacher { Id=129, Name="DD", Designation="Assistant Lecturer" },
                new Teacher { Id=130, Name="EE", Designation="Assistant Lecturer" },

            };
            modelBuilder.Entity<Teacher>().HasData(Teachers);

            //These need to be actual courses.
            var Courses = new List<Course>
            {
                new Course { Id=501, Name="Data Structures", Level=3, Term=1, Credit=3, TeacherId=101 },
                new Course { Id=502, Name="Algorithms", Level=3, Term=2, Credit=3, TeacherId=102 },
                new Course { Id=503, Name="Circuit Analysis", Level=3, Term=1, Credit=3, TeacherId=103 },
                new Course { Id=504, Name="Electromagnetics", Level=3, Term=2, Credit=3, TeacherId=104 },
            };
            modelBuilder.Entity<Course>().HasData(Courses);

            var ClassSchedule = new List<ClassSchedule>
            {
                new ClassSchedule { Id=4001, CourseId=501, TeacherId=101, ClassroomId=304, Day=DayOfWeek.Monday, StartTime=new TimeOnly(9,0,0), EndTime=new TimeOnly(10,30,0), },
                new ClassSchedule { Id=4002, CourseId=502, TeacherId=101, ClassroomId=304, Day=DayOfWeek.Wednesday, StartTime=new TimeOnly(11,0,0), EndTime=new TimeOnly(12,30,0), },
                new ClassSchedule { Id=4003, CourseId=503, TeacherId=102, ClassroomId=304, Day=DayOfWeek.Tuesday, StartTime=new TimeOnly(10,0,0), EndTime=new TimeOnly(11,30,0), },
                new ClassSchedule { Id=4004, CourseId=504, TeacherId=102, ClassroomId=305, Day=DayOfWeek.Thursday, StartTime=new TimeOnly(13,0,0), EndTime=new TimeOnly(14,30,0) },
                new ClassSchedule { Id=4005, CourseId=501, TeacherId=102, ClassroomId=305, Day=DayOfWeek.Sunday, StartTime=new TimeOnly(9,0,0), EndTime=new TimeOnly(10,30,0) },
                new ClassSchedule { Id=4006, CourseId=502, TeacherId=104, ClassroomId=306, Day=DayOfWeek.Tuesday, StartTime=new TimeOnly(14,0,0), EndTime=new TimeOnly(15,30,0) },
                new ClassSchedule { Id=4007, CourseId=503, TeacherId=105, ClassroomId=306, Day=DayOfWeek.Monday, StartTime=new TimeOnly(8,30,0), EndTime=new TimeOnly(10,0,0) },
                new ClassSchedule { Id=4008, CourseId=504, TeacherId=103, ClassroomId=308, Day=DayOfWeek.Wednesday, StartTime=new TimeOnly(12,0,0), EndTime=new TimeOnly(13,30,0) },
                new ClassSchedule { Id=4009, CourseId=501, TeacherId=105, ClassroomId=308, Day=DayOfWeek.Thursday, StartTime=new TimeOnly(15,0,0), EndTime=new TimeOnly(16,30,0) },
                new ClassSchedule { Id=4010, CourseId=502, TeacherId=104, ClassroomId=308, Day=DayOfWeek.Sunday, StartTime=new TimeOnly(10,0,0), EndTime=new TimeOnly(11,30,0) },
                new ClassSchedule { Id=4011, CourseId=503, TeacherId=105, ClassroomId=308, Day=DayOfWeek.Tuesday, StartTime=new TimeOnly(11,0,0), EndTime=new TimeOnly(12,30,0) },
                new ClassSchedule { Id=4012, CourseId=504, TeacherId=101, ClassroomId=309, Day=DayOfWeek.Monday, StartTime=new TimeOnly(13,0,0), EndTime=new TimeOnly(14,30,0) },
                new ClassSchedule { Id=4013, CourseId=501, TeacherId=102, ClassroomId=309, Day=DayOfWeek.Wednesday, StartTime=new TimeOnly(9,30,0), EndTime=new TimeOnly(11,0,0) },
                new ClassSchedule { Id=4014, CourseId=502, TeacherId=103, ClassroomId=310, Day=DayOfWeek.Thursday, StartTime=new TimeOnly(14,0,0), EndTime=new TimeOnly(15,30,0) },
            };
            modelBuilder.Entity<ClassSchedule>().HasData(ClassSchedule);
        }
    }
}