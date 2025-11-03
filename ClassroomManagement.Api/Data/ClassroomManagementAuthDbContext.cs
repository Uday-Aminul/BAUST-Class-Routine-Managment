using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Data
{
    public class ClassroomManagementAuthDbContext : IdentityDbContext
    {
        public ClassroomManagementAuthDbContext(DbContextOptions<ClassroomManagementAuthDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var readerRoleId = "3a4c51c5-9f18-4c2f-9024-7cce011b2e15";
            var writerRoleId = "d6b4a1cb-2f93-4d2e-bec8-94cd3d46f6c8";
            var roles = new List<IdentityRole>()
            {
                new IdentityRole(){
                    Id=readerRoleId,
                    Name="Reader",
                    ConcurrencyStamp=readerRoleId,
                    NormalizedName="Reader".ToUpper()
                },
                new IdentityRole(){
                    Id=writerRoleId,
                    Name="Writer",
                    ConcurrencyStamp=writerRoleId,
                    NormalizedName="Writer".ToUpper()
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);
        }
    }
}