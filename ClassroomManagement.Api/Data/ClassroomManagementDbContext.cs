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
                new Labroom { Id=1004, Name="DWM Seminar Hall" },
                new Labroom { Id=1005, Name="Electronics Lab" },
                new Labroom { Id=1006, Name="Physics Lab" },
            };
            modelBuilder.Entity<Labroom>().HasData(Labrooms);

            // var Teachers = new List<Teacher>
            // {

            // };
            // modelBuilder.Entity<Teacher>().HasData(Teachers);

            // var Courses = new List<Course>
            // {

            // };
            // modelBuilder.Entity<Course>().HasData(Courses);

            // var ClassSchedule = new List<ClassSchedule>
            // {

            // };
            // modelBuilder.Entity<ClassSchedule>().HasData(ClassSchedule);
        }
    }
}