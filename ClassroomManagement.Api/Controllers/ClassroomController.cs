using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClassroomManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomRepository _classroomsRepository;
        private readonly IMapper _mapper;
        public ClassroomController(IClassroomRepository classroomRepository, IMapper mapper)
        {
            _classroomsRepository = classroomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var classroomDomains = await _classroomsRepository.GetAllClassroomsAsync();
            var classroomDtos = _mapper.Map<List<ClassroomDto>>(classroomDomains);
            return Ok(classroomDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var classroomDomain = await _classroomsRepository.GetClassroomByIdAsync(id);
            if (classroomDomain is null)
            {
                return NotFound();
            }
            var classroomDto = _mapper.Map<ClassroomDto>(classroomDomain);
            return Ok(classroomDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var classroomDomains = await _classroomsRepository.DeleteClassroomByIdAsync(id);
            if (classroomDomains is null)
            {
                return NotFound();
            }
            var ClassroomDtos = _mapper.Map<List<ClassroomDto>>(classroomDomains);
            return Ok(ClassroomDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateClassroomRequestDto updatedClassroom)
        {
            var classroomDomain = _mapper.Map<Classroom>(updatedClassroom);
            classroomDomain = await _classroomsRepository.UpdateClassroomByIdAsync(id, classroomDomain);
            if (classroomDomain is null)
            {
                return NotFound();
            }
            var classroomDto = _mapper.Map<ClassroomDto>(classroomDomain);
            return Ok(classroomDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddClassroomRequestDto newClassroom)
        {
            var classroomDomain = _mapper.Map<Classroom>(newClassroom);
            classroomDomain = await _classroomsRepository.CreateClassroomAsync(classroomDomain);
            var ClassroomDto = _mapper.Map<ClassroomDto>(classroomDomain);
            return CreatedAtAction(nameof(GetById), new { id = classroomDomain.Id }, ClassroomDto);
        }
    }
}