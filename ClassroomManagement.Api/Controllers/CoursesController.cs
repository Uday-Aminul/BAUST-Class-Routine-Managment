using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs.Course;
using ClassroomManagement.Api.Repositories.SQLCourse;
using Microsoft.AspNetCore.Mvc;

namespace CourseManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CoursesController : ControllerBase
    {
        private readonly ICourseRepository _coursesRepository;
        private readonly IMapper _mapper;
        public CoursesController(ICourseRepository courseRepository, IMapper mapper)
        {
            _coursesRepository = courseRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var courseDomains = await _coursesRepository.GetAllCoursesAsync();
            var courseDtos = _mapper.Map<List<CourseDto>>(courseDomains);
            return Ok(courseDtos);
        }

        [HttpGet]
        [Route("{roomNumber:int}")]
        public async Task<IActionResult> GetByRoomNumber([FromRoute] int roomNumber)
        {
            var courseDomain = await _coursesRepository.GetCourseByIdAsync(roomNumber);
            if (courseDomain is null)
            {
                return NotFound();
            }
            var courseDto = _mapper.Map<CourseDto>(courseDomain);
            return Ok(courseDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var courseDomains = await _coursesRepository.DeleteCourseByIdAsync(id);
            if (courseDomains is null)
            {
                return NotFound();
            }
            var CourseDtos = _mapper.Map<List<CourseDto>>(courseDomains);
            return Ok(CourseDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCourseRequestDto updatedCourse)
        {
            var courseDomain = _mapper.Map<Course>(updatedCourse);
            courseDomain = await _coursesRepository.UpdateCourseByIdAsync(id, courseDomain);
            if (courseDomain is null)
            {
                return NotFound();
            }
            var courseDto = _mapper.Map<CourseDto>(courseDomain);
            return Ok(courseDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddCourseRequestDto newCourse)
        {
            var courseDomain = _mapper.Map<Course>(newCourse);
            courseDomain = await _coursesRepository.CreateCourseAsync(courseDomain);
            var CourseDto = _mapper.Map<CourseDto>(courseDomain);
            return CreatedAtAction(nameof(GetByRoomNumber), new { id = courseDomain.Id }, CourseDto);
        }
    }
}