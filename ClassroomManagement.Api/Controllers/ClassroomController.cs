using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClassroomManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClassroomController : ControllerBase
    {
        private readonly IClassroomRepository _classroomRepository;
        private readonly IMapper _mapper;
        public ClassroomController(IClassroomRepository classroomRepository, IMapper mapper)
        {
            _classroomRepository = classroomRepository;
            _mapper = mapper;
        }

       [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var ClassroomsDomain = await _classroomRepository.GetAllClassroomsAsync();
            var ClassroomsDto = _mapper.Map<List<ClassroomDto>>(ClassroomsDomain);
            return Ok(ClassroomsDto);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var ClassroomDomain = await _classroomRepository.GetClassroomByIdAsync(id);
            if (ClassroomDomain is null)
            {
                return NotFound();
            }
            var ClassroomDto = _mapper.Map<ClassroomDto>(ClassroomDomain);
            return Ok(ClassroomDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var ClassroomsDomain = await _classroomRepository.DeleteClassroomAsync(id);
            if (ClassroomsDomain is null)
            {
                return NotFound();
            }
            var ClassroomsDto = _mapper.Map<List<ClassroomDto>>(ClassroomsDomain);
            return Ok(ClassroomsDto);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] ClassroomDto updatedClassroom)
        {
            var ClassroomDomain = _mapper.Map<Classroom>(updatedClassroom);
            ClassroomDomain = await _classroomRepository.UpdateClassroomAsync(id, ClassroomDomain);
            if (ClassroomDomain is null)
            {
                return NotFound();
            }
            var ClassroomDto = _mapper.Map<ClassroomDto>(ClassroomDomain);
            return CreatedAtAction(nameof(GetById), new{id=ClassroomDto.Id}, ClassroomDto);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] ClassroomDto newClassroom)
        {
            var ClassroomDomain = _mapper.Map<Classroom>(newClassroom);
            ClassroomDomain = await _classroomRepository.CreateClassroomAsync(ClassroomDomain);
            var ClassroomDto = _mapper.Map<ClassroomDto>(ClassroomDomain);
            return CreatedAtAction(nameof(GetById), new{id=ClassroomDto.Id}, ClassroomDto);
        }
    }
}