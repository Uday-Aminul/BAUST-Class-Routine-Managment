using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SessionalManagement.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SessionalsController
    {
        private readonly ISessionalRepository _classroomsRepository;
        private readonly IMapper _mapper;
        public SessionalController(ISessionalRepository classroomRepository, IMapper mapper)
        {
            _classroomsRepository = classroomRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var classroomDomains = await _classroomsRepository.GetAllSessionalsAsync();
            var classroomDtos = _mapper.Map<List<SessionalDto>>(classroomDomains);
            return Ok(classroomDtos);
        }

        [HttpGet]
        [Route("{roomNumber:int}")]
        public async Task<IActionResult> GetByRoomNumber([FromRoute] int roomNumber)
        {
            var classroomDomain = await _classroomsRepository.GetSessionalByRoomNumberAsync(roomNumber);
            if (classroomDomain is null)
            {
                return NotFound();
            }
            var classroomDto = _mapper.Map<SessionalDto>(classroomDomain);
            return Ok(classroomDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var classroomDomains = await _classroomsRepository.DeleteSessionalByIdAsync(id);
            if (classroomDomains is null)
            {
                return NotFound();
            }
            var SessionalDtos = _mapper.Map<List<SessionalDto>>(classroomDomains);
            return Ok(SessionalDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateSessionalRequestDto updatedSessional)
        {
            var classroomDomain = _mapper.Map<Sessional>(updatedSessional);
            classroomDomain = await _classroomsRepository.UpdateSessionalByIdAsync(id, classroomDomain);
            if (classroomDomain is null)
            {
                return NotFound();
            }
            var classroomDto = _mapper.Map<SessionalDto>(classroomDomain);
            return Ok(classroomDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddSessionalRequestDto newSessional)
        {
            var classroomDomain = _mapper.Map<Sessional>(newSessional);
            classroomDomain = await _classroomsRepository.CreateSessionalAsync(classroomDomain);
            var SessionalDto = _mapper.Map<SessionalDto>(classroomDomain);
            return CreatedAtAction(nameof(GetByRoomNumber), new { id = classroomDomain.Id }, SessionalDto);
        }
    }
}