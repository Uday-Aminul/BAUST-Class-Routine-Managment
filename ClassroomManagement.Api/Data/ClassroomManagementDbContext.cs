using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Data
{
    public class ClassroomManagementDbContext: DbContext
    {
        public ClassroomManagementDbContext(DbContextOptions<ClassroomManagementDbContext> options) : base(options)
        {

        }
        
        public DbSet<Classroom> Classrooms { get; set; }
        public DbSet<Course> Courses { get; set; }
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
                new Classroom { Id=301, IsLab=true },
                new Classroom { Id=302, IsLab=false },
                new Classroom { Id=303, IsLab=false },
                new Classroom { Id=304, IsLab=false },
                new Classroom { Id=305, IsLab=false },
                new Classroom { Id=306, IsLab=false },
                new Classroom { Id=307, IsLab=false },
                new Classroom { Id=308, IsLab=false },
                new Classroom { Id=309, IsLab=false },
                new Classroom { Id=310, IsLab=false },
                new Classroom { Id=311, IsLab=true }
            };
            modelBuilder.Entity<Classroom>().HasData(ClassRooms);

            //These need to be actual courses.
            var Courses = new List<Course>
            {
                new Course { Id=501, Name="Data Structures", Level=3, Term=1, Credit=3, TeacherId=2001 },
                new Course { Id=502, Name="Algorithms", Level=3, Term=2, Credit=3, TeacherId=2001 },
                new Course { Id=503, Name="Circuit Analysis", Level=3, Term=1, Credit=3, TeacherId=2002 },
                new Course { Id=504, Name="Electromagnetics", Level=3, Term=2, Credit=3, TeacherId=2003 },
            };
            modelBuilder.Entity<Course>().HasData(Courses);

            //These need to be actual teachers. and Courses navigation property.
            var Teachers = new List<Teacher>
            {
                new Teacher { Id=2001, Name="Alice Smith", Designation="Assistant Professor" },
                new Teacher { Id=2002, Name="Bob Johnson", Designation="Associate Professor" },
                new Teacher { Id=2003, Name="Carol Williams", Designation="Professor" }
            };
            modelBuilder.Entity<Teacher>().HasData(Teachers);

            var ClassSchedule = new List<ClassSchedule>
            {
                new ClassSchedule { Id=4001, CourseId=501, TeacherId=2001, ClassroomId=302, Day=DayOfWeek.Monday, StartTime=new TimeOnly(9,0,0), EndTime=new TimeOnly(10,30,0), Section="A" },
                new ClassSchedule { Id=4002, CourseId=502, TeacherId=2001, ClassroomId=303, Day=DayOfWeek.Wednesday, StartTime=new TimeOnly(11,0,0), EndTime=new TimeOnly(12,30,0), Section="A" },
                new ClassSchedule { Id=4003, CourseId=503, TeacherId=2002, ClassroomId=304, Day=DayOfWeek.Tuesday, StartTime=new TimeOnly(10,0,0), EndTime=new TimeOnly(11,30,0), Section="B" },
                new ClassSchedule { Id=4004, CourseId=504, TeacherId=2003, ClassroomId=305, Day=DayOfWeek.Thursday, StartTime=new TimeOnly(13,0,0), EndTime=new TimeOnly(14,30,0), Section="c" }
            };
            modelBuilder.Entity<ClassSchedule>().HasData(ClassSchedule);
        }
    }
}