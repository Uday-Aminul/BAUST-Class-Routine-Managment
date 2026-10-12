using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ClassroomManagement.Api.Models;
using System.Net;
using System.Text.Json;
using ClassroomManagement.Api.Models.DTOs;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ClassRoutineManagement.Tests.IntregationTests.Controllers
{
    public class ClassroomControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        public ClassroomControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsOk()
        {
            //Arrange
            var client = CreateCustomClient();

            //Act
            var response = await client.GetAsync("/api/Classroom");

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var classrooms = await response.Content.ReadFromJsonAsync<List<ClassroomDto>>();
            Assert.NotNull(classrooms);
            Assert.Equal(2, classrooms.Count);
            Assert.Contains(classrooms, classroom => classroom.Id == 8801 && classroom.RoomNumber == 101);
            Assert.Contains(classrooms, classroom => classroom.Id == 8802 && classroom.RoomNumber == 102);
        }

        [Fact]
        public async Task GetAll_WithDockerDb_ReturnsClassroomsSeededByScript()
        {
            var client = CreateCustomClient1();

            var response = await client.GetAsync("/api/Classroom");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var classrooms = await response.Content.ReadFromJsonAsync<List<ClassroomDto>>();
            Assert.NotNull(classrooms);
            Assert.Equal(17, classrooms.Count);
            Assert.Contains(classrooms, c => c.RoomNumber == 204);
            Assert.Contains(classrooms, c => c.RoomNumber == 1001);
        }

        //Creating custom client
        private HttpClient CreateCustomClient()
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

        private HttpClient CreateCustomClient1()
        {
            const string connectionString =
                "Server=localhost,1434;Database=ClassroomTestDb;User Id=sa;Password=Test!Pass123;TrustServerCertificate=True";

            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IDbContextOptionsConfiguration<ClassroomManagementDbContext>>();
                    services.AddDbContext<ClassroomManagementDbContext>(o => o.UseSqlServer(connectionString));
                });
            }).CreateClient();
        }
    }
}