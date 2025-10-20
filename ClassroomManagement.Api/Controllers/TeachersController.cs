using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TeacherManagement.Api.Repositories;

namespace teachersManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeachersController : ControllerBase
    {
        private readonly ITeacherRepository _teachersRepository;
        private readonly IMapper _mapper;
        public TeachersController(ITeacherRepository teachersRepository, IMapper mapper)
        {
            _mapper=mapper;
            _teachersRepository = teachersRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var teachersDomain = await _teachersRepository.GetAllTeachersAsync();
            var teachersDto = _mapper.Map<List<TeacherDto>>(teachersDomain);
            return Ok(teachersDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teachersDomain = await _teachersRepository.GetTeacherByIdAsync(id);
            if (teachersDomain is null)
            {
                return NotFound();
            }
            var teachersDto = _mapper.Map<TeacherDto>(teachersDomain);
            return Ok(teachersDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var teacherssDomain = await _teachersRepository.DeleteTeacherAsync(id);
            if (teacherssDomain is null)
            {
                return NotFound();
            }
            var teacherssDto = _mapper.Map<List<TeacherDto>>(teacherssDomain);
            return Ok(teacherssDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TeacherDto updatedteachers)
        {
            var teachersDomain = _mapper.Map<Teacher>(updatedteachers);
            teachersDomain = await _teachersRepository.UpdateTeacherAsync(id, teachersDomain);
            if (teachersDomain is null)
            {
                return NotFound();
            }
            var teachersDto = _mapper.Map<TeacherDto>(teachersDomain);
            return CreatedAtAction(nameof(GetById), new{id=teachersDto.Id}, teachersDto);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherDto newteachers)
        {
            var teachersDomain = _mapper.Map<Teacher>(newteachers);
            teachersDomain = await _teachersRepository.CreateTeacherAsync(teachersDomain);
            var teachersDto = _mapper.Map<TeacherDto>(teachersDomain);
            return CreatedAtAction(nameof(GetById), new{id=teachersDto.Id}, teachersDto);
        }
    }
}