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
using AutoMapper.Configuration.Annotations;
using ClassRoutineManagement.Tests.IntregationTests.Infrastructure;

namespace ClassRoutineManagement.Tests.IntregationTests.Controllers
{
    public class ClassroomControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly CustomClientFactory _factory;
        public ClassroomControllerIntegrationTests(WebApplicationFactory<Program> factory)
        {
            _factory = new CustomClientFactory(factory);
        }

        [Fact]
        public async Task GetAll_WhenCalled_ReturnsOk()
        {
            //Arrange
            var client = _factory.CreateCustomClientForInMemoryTest();

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
    }
}