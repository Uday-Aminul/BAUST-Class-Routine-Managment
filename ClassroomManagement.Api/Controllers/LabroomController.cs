using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs.Labroom;
using ClassroomManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagement.Api.Controllers
{
    [Controller]
    [Route("api/[controller]")]
    public class LabroomsController : ControllerBase
    {
        private readonly ILabroomRepository _labroomRepository;
        private readonly IMapper _mapper;
        public LabroomsController(ILabroomRepository labroomRepository, IMapper mapper)
        {
            _labroomRepository = labroomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var labroomDomains = await _labroomRepository.GetAllLabroomsAsync();
            var labroomDtos = _mapper.Map<List<LabroomDto>>(labroomDomains);
            return Ok(labroomDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> UpdateLabroom([FromRoute] int id, [FromBody] LabroomUpdateRequestDto labroomUpdateRequestDto)
        {
            var labroomDomain = await _labroomRepository.UpdateLabroomByIdAsync(id, labroomUpdateRequestDto);
            if (labroomDomain is null)
            {
                return NotFound();
            }
            var labroomDto = _mapper.Map<LabroomDto>(labroomDomain);
            return Ok(labroomDto);
        }

    }
}