using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs.Sessional;
using Microsoft.AspNetCore.Mvc;
using SessionalManagement.Api.Repositories.SQLSessional;

namespace SessionalManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionalsController : ControllerBase
    {
        private readonly ISessionalRepository _sessionalsRepository;
        private readonly IMapper _mapper;
        public SessionalsController(ISessionalRepository sessionalRepository, IMapper mapper)
        {
            _sessionalsRepository = sessionalRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var sessionalDomains = await _sessionalsRepository.GetAllSessionalsAsync();
            var sessionalDtos = _mapper.Map<List<SessionalDto>>(sessionalDomains);
            return Ok(sessionalDtos);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetByRoomNumber([FromRoute] int id)
        {
            var sessionalDomain = await _sessionalsRepository.GetSessionalByIdAsync(id);
            if (sessionalDomain is null)
            {
                return NotFound();
            }
            var sessionalDto = _mapper.Map<SessionalDto>(sessionalDomain);
            return Ok(sessionalDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var sessionalDomains = await _sessionalsRepository.DeleteSessionalByIdAsync(id);
            if (sessionalDomains is null)
            {
                return NotFound();
            }
            var SessionalDtos = _mapper.Map<List<SessionalDto>>(sessionalDomains);
            return Ok(SessionalDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] SessionalUpdateRequestDto updatedSessional)
        {
            var sessionalDomain = _mapper.Map<Sessional>(updatedSessional);
            sessionalDomain = await _sessionalsRepository.UpdateSessionalByIdAsync(id, sessionalDomain, updatedSessional.LabroomIds);
            if (sessionalDomain is null)
            {
                return NotFound();
            }
            var sessionalDto = _mapper.Map<SessionalDto>(sessionalDomain);
            return Ok(sessionalDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SessionalAddRequestDto newSessional)
        {
            var sessionalDomain = _mapper.Map<Sessional>(newSessional);
            sessionalDomain = await _sessionalsRepository.CreateSessionalAsync(sessionalDomain, newSessional.LabroomIds);
            var SessionalDto = _mapper.Map<SessionalDto>(sessionalDomain);
            return CreatedAtAction(nameof(GetByRoomNumber), new { id = sessionalDomain.Id }, SessionalDto);
        }
    }
}