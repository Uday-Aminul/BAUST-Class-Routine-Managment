using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Controllers;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace ClassRoutineManagement.Tests
{
    public class ClassroomControllerTests
    {
        private readonly Mock<IClassroomRepository> _classroomRepository;
        private readonly Mock<IMapper> _mapper;
        private readonly ClassroomController _classroomController;

        public ClassroomControllerTests()
        {
            _classroomRepository = new Mock<IClassroomRepository>();
            _mapper = new Mock<IMapper>();
            _classroomController = new ClassroomController(_classroomRepository.Object, _mapper.Object);
        }

        [Fact]
        public async Task ClassroomController_GetAll_ReturnsOk()
        {
            //Arrange
            var classroomDomains = new List<Classroom>()
            {
                new Classroom()
                {
                    Id=8801,
                    RoomNumber=101
                },
                new Classroom()
                {
                    Id=8801,
                    RoomNumber=101
                }
            };
            var classroomDtos = new List<ClassroomDto>()
            {
                new ClassroomDto()
                {
                    Id=8801,
                    RoomNumber=101
                },
                new ClassroomDto()
                {
                    Id=8801,
                    RoomNumber=101
                }
            };
            _classroomRepository.Setup(repository => repository.GetAllClassroomsAsync()).ReturnsAsync(classroomDomains);
            _mapper.Setup(mapper => mapper.Map<List<ClassroomDto>>(classroomDomains)).Returns(classroomDtos);

            //Act
            var result = await _classroomController.GetAll();

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            var returnType = Assert.IsAssignableFrom<List<ClassroomDto>>(okObjectResult.Value);
            Assert.Equal(2, returnType.Count);
            Assert.Same(classroomDtos, okObjectResult.Value);
        }
    }
}