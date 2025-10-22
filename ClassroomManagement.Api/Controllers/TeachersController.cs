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
            var teacherDomains = await _teachersRepository.GetAllTeachersAsync();
            var teacherDtos = _mapper.Map<List<TeacherDto>>(teacherDomains);
            return Ok(teacherDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teacherDomain = await _teachersRepository.GetTeacherByIdAsync(id);
            if (teacherDomain is null)
            {
                return NotFound();
            }
            var teacherDto = _mapper.Map<TeacherDto>(teacherDomain);
            return Ok(teacherDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var teacherDomains = await _teachersRepository.DeleteTeacherByIdAsync(id);
            if (teacherDomains is null)
            {
                return NotFound();
            }
            var teacherDtos = _mapper.Map<List<TeacherDto>>(teacherDomains);
            return Ok(teacherDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTeacherRequestDto updatedteachers)
        {
            var teacherDomain = _mapper.Map<Teacher>(updatedteachers);
            teacherDomain = await _teachersRepository.UpdateTeacherByIdAsync(id, teacherDomain);
            if (teacherDomain is null)
            {
                return NotFound();
            }
            var teacherDto = _mapper.Map<TeacherDto>(teacherDomain);
            return Ok(teacherDto);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTeacherRequestDto newteachers)
        {
            var teacherDomain = _mapper.Map<Teacher>(newteachers);
            teacherDomain = await _teachersRepository.CreateTeacherAsync(teacherDomain);
            var teacherDto = _mapper.Map<TeacherDto>(teacherDomain);
            return CreatedAtAction(nameof(GetById), new{id=teacherDto.Id}, teacherDto);
        }
    }
}