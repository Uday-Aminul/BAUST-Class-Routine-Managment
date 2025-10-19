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
    }
}