using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs.TeacherAssignment;
using Microsoft.AspNetCore.Mvc;
using TeacherAssignmentManagement.Api.Repositories.SQLTeacherAssignment;

namespace TeacherAssignmentManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeacherAssignmentsController : ControllerBase
    {
        private readonly ITeacherAssignmentRepository _teacherAssignmentsRepository;
        private readonly IMapper _mapper;
        public TeacherAssignmentsController(ITeacherAssignmentRepository teacherAssignmentRepository, IMapper mapper)
        {
            _teacherAssignmentsRepository = teacherAssignmentRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var teacherAssignmentDomains = await _teacherAssignmentsRepository.GetAllTeacherAssignmentsAsync();
            var teacherAssignmentDtos = _mapper.Map<List<TeacherAssignmentDto>>(teacherAssignmentDomains);
            return Ok(teacherAssignmentDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var teacherAssignmentDomain = await _teacherAssignmentsRepository.GetTeacherAssignmentByIdAsync(id);
            if (teacherAssignmentDomain is null)
            {
                return NotFound();
            }
            var teacherAssignmentDto = _mapper.Map<TeacherAssignmentDto>(teacherAssignmentDomain);
            return Ok(teacherAssignmentDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var teacherAssignmentDomains = await _teacherAssignmentsRepository.DeleteTeacherAssignmentByIdAsync(id);
            if (teacherAssignmentDomains is null)
            {
                return NotFound();
            }
            var TeacherAssignmentDtos = _mapper.Map<List<TeacherAssignmentDto>>(teacherAssignmentDomains);
            return Ok(TeacherAssignmentDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] TeacherAssignmentUpdateRequestDto updatedTeacherAssignment)
        {
            var teacherAssignmentDomain = _mapper.Map<TeacherAssignment>(updatedTeacherAssignment);
            teacherAssignmentDomain = await _teacherAssignmentsRepository.UpdateTeacherAssignmentByIdAsync(id, teacherAssignmentDomain);
            if (teacherAssignmentDomain is null)
            {
                return NotFound();
            }
            var teacherAssignmentDto = _mapper.Map<TeacherAssignmentDto>(teacherAssignmentDomain);
            return Ok(teacherAssignmentDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] TeacherAssignmentAddRequestDto newTeacherAssignment)
        {
            var teacherAssignmentDomain = _mapper.Map<TeacherAssignment>(newTeacherAssignment);
            teacherAssignmentDomain = await _teacherAssignmentsRepository.CreateTeacherAssignmentAsync(teacherAssignmentDomain);
            var TeacherAssignmentDto = _mapper.Map<TeacherAssignmentDto>(teacherAssignmentDomain);
            return CreatedAtAction(nameof(GetById), new { id = teacherAssignmentDomain.Id }, TeacherAssignmentDto);
        }
    }
}