using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs;
using ClassScheduleManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ClassroomManagement.Api.Controllers
{
    [Route("[controller]")]
    public class ClassScheduleController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IClassScheduleRepository _classSchedulesRepository;

        public ClassScheduleController(IClassScheduleRepository classScheduleRepository, IMapper mapper)
        {
            _classSchedulesRepository = classScheduleRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var classScheduleDomains = await _classSchedulesRepository.GetAllClassSchedulesAsync();
            var classScheduleDtos = _mapper.Map<List<ClassScheduleDto>>(classScheduleDomains);
            return Ok(classScheduleDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            var classScheduleDomain = await _classSchedulesRepository.GetClassScheduleByIdAsync(id);
            if (classScheduleDomain is null)
            {
                return NotFound();
            }
            var classScheduleDto = _mapper.Map<ClassScheduleDto>(classScheduleDomain);
            return Ok(classScheduleDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var classScheduleDomains = await _classSchedulesRepository.DeleteClassScheduleByIdAsync(id);
            if (classScheduleDomains is null)
            {
                return NotFound();
            }
            var classScheduleDtos = _mapper.Map<List<ClassScheduleDto>>(classScheduleDomains);
            return Ok(classScheduleDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateClassScheduleRequestDto updatedclassSchedules)
        {
            var classScheduleDomain = _mapper.Map<ClassSchedule>(updatedclassSchedules);
            classScheduleDomain = await _classSchedulesRepository.UpdateClassScheduleByIdAsync(id, classScheduleDomain);
            if (classScheduleDomain is null)
            {
                return NotFound();
            }
            var classScheduleDto = _mapper.Map<ClassScheduleDto>(classScheduleDomain);
            return Ok(classScheduleDto);
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddClassScheduleRequestDto newClassSchedules)
        {
            var classScheduleDomain = _mapper.Map<ClassSchedule>(newClassSchedules);
            classScheduleDomain = await _classSchedulesRepository.CreateClassScheduleAsync(classScheduleDomain);
            var classScheduleDto = _mapper.Map<ClassScheduleDto>(classScheduleDomain);
            return CreatedAtAction(nameof(GetById), new{id=classScheduleDto.Id}, classScheduleDto);
        }
    }
}