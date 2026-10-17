using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.DTOs;
using ClassRoutineManagement.Tests.IntregationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ClassRoutineManagement.Tests.IntregationTests.Controllers
{
    public class ClassroomControllerDatabaseTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly CustomClientFactory _factory;
        public ClassroomControllerDatabaseTests(WebApplicationFactory<Program> factory)
        {
            _factory = new CustomClientFactory(factory);
        }

        [Fact]
        public async Task GetAll_WithDockerDb_ReturnsClassroomsSeededByScript()
        {
            var client = _factory.CreateCustomClientForRealDbTest();

            var response = await client.GetAsync("/api/Classroom");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var classrooms = await response.Content.ReadFromJsonAsync<List<ClassroomDto>>();
            Assert.NotNull(classrooms);
            Assert.Equal(17, classrooms.Count);
            Assert.Contains(classrooms, c => c.RoomNumber == 204);
            Assert.Contains(classrooms, c => c.RoomNumber == 1001);
        }
    }
}