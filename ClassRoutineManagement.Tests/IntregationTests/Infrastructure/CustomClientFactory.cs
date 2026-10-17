using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClassRoutineManagement.Tests.IntregationTests.Infrastructure
{
    public class CustomClientFactory
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CustomClientFactory(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        public HttpClient CreateCustomClientForInMemoryTest()
        {
            var dbName = "TestDb" + Guid.NewGuid();
            //Configuring the builder
            var customBuilder = _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    //Finding and removing the existing DbContext
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IDbContextOptionsConfiguration<ClassroomManagementDbContext>));
                    if (descriptor is not null)
                    {
                        services.Remove(descriptor);
                    }

                    //Adding new DbContext using In Memory
                    services.AddDbContext<ClassroomManagementDbContext>(Options => Options.UseInMemoryDatabase(dbName));
                });
            });

            //Seeding dummy data
            using (var scope = customBuilder.Services.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ClassroomManagementDbContext>();
                dbContext.Classrooms.AddRange(new List<Classroom>(){
                new Classroom { Id = 8801, RoomNumber = 101 },
                new Classroom { Id = 8802, RoomNumber = 102 }
                });
                dbContext.SaveChanges();
            }

            //Creating the Client
            var customClient = customBuilder.CreateClient();

            return customClient;
        }

        public HttpClient CreateCustomClientForRealDbTest()
        {
            const string connectionString =
                "Server=localhost,1434;Database=ClassroomDb;User Id=sa;Password=YourStrong!Pass123;TrustServerCertificate=True";

            var customBuilder = _factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                services.RemoveAll<IDbContextOptionsConfiguration<ClassroomManagementDbContext>>();
                services.AddDbContext<ClassroomManagementDbContext>(options => options.UseSqlServer(connectionString));
            }));
            var customClient = customBuilder.CreateClient();
            return customClient;
        }
    }
}