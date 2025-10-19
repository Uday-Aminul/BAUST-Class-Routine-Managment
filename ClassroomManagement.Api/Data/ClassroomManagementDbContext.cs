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
        public DbSet<Department> Departments { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

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

            var Departments = new List<Department>
            {
                new Department { Id=1001, Name="CSE" },
                new Department { Id=1002, Name="EEE" },
                new Department { Id=1003, Name="BBA" },
                new Department { Id=1004, Name="English" },
                new Department { Id=1005, Name="ME" }
            };
            modelBuilder.Entity<Department>().HasData(Departments);

            //These need to be actual courses.
            var Courses = new List<Course>
            {
                new Course { Id=501, Name="Data Structures", Level=3, Term=1, Credit=3, DepartmentId=1001 },
                new Course { Id=502, Name="Algorithms", Level=3, Term=2, Credit=3, DepartmentId=1001 },
                new Course { Id=503, Name="Circuit Analysis", Level=3, Term=1, Credit=3, DepartmentId=1002 },
                new Course { Id=504, Name="Electromagnetics", Level=3, Term=2, Credit=3, DepartmentId=1002 }
            };
            modelBuilder.Entity<Course>().HasData(Courses);

            //These need to be actual teachers. and Courses navigation property.
            var Teachers = new List<Teacher>
            {
                new Teacher { Id=2001, Name="Alice Smith", Designation="Assistant Professor", DepartmentId=1001 },
                new Teacher { Id=2002, Name="Bob Johnson", Designation="Associate Professor", DepartmentId=1002 },
                new Teacher { Id=2003, Name="Carol Williams", Designation="Professor", DepartmentId=1003 }
            };
            modelBuilder.Entity<Teacher>().HasData(Teachers);
        }
    }
}