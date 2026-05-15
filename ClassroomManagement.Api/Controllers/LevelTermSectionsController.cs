using AutoMapper;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs.LevelTermSection;
using LevelTermSectionManagement.Api.Repositories;
using LevelTermSectionManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace LevelTermSectionManagement.Api.Controllers
{
    [Route("[controller]")]
    public class LevelTermSectionsController : ControllerBase
    {
        private readonly ILevelTermSectionRepository _levelTermSectionsRepository;
        private readonly IMapper _mapper;

        public LevelTermSectionsController(ILevelTermSectionRepository levelTermSectionRepository, IMapper mapper)
        {
            _levelTermSectionsRepository = levelTermSectionRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //[Authorize(Roles = "Writer")]
        public async Task<IActionResult> GetAll()
        {
            var levelTermSectionDomains = await _levelTermSectionsRepository.GetAllLevelTermSectionsAsync();
            var levelTermSectionDtos = _mapper.Map<List<LevelTermSectionDto>>(levelTermSectionDomains);
            return Ok(levelTermSectionDtos);
        }

        [HttpGet]
        public async Task<IActionResult> GetByRoomNumber([FromQuery] int level, [FromQuery] int term, [FromQuery] string section)
        {
            var levelTermSectionDomain = await _levelTermSectionsRepository.GetLevelTermSectionByLevelTermSectionAsync(level, term, section);
            if (levelTermSectionDomain is null)
            {
                return NotFound();
            }
            var levelTermSectionDto = _mapper.Map<LevelTermSectionDto>(levelTermSectionDomain);
            return Ok(levelTermSectionDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var levelTermSectionDomains = await _levelTermSectionsRepository.DeleteLevelTermSectionByIdAsync(id);
            if (levelTermSectionDomains is null)
            {
                return NotFound();
            }
            var LevelTermSectionDtos = _mapper.Map<List<LevelTermSectionDto>>(levelTermSectionDomains);
            return Ok(LevelTermSectionDtos);
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] LevelTermSectionUpdateRequestDto updatedLevelTermSection)
        {
            var levelTermSectionDomain = _mapper.Map<LevelTermSection>(updatedLevelTermSection);
            levelTermSectionDomain = await _levelTermSectionsRepository.UpdateLevelTermSectionByIdAsync(id, levelTermSectionDomain);
            if (levelTermSectionDomain is null)
            {
                return NotFound();
            }
            var levelTermSectionDto = _mapper.Map<LevelTermSectionDto>(levelTermSectionDomain);
            return Ok(levelTermSectionDto);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LevelTermSectionAddRequestDto newLevelTermSection)
        {
            var levelTermSectionDomain = _mapper.Map<LevelTermSection>(newLevelTermSection);
            levelTermSectionDomain = await _levelTermSectionsRepository.CreateLevelTermSectionAsync(levelTermSectionDomain);
            var LevelTermSectionDto = _mapper.Map<LevelTermSectionDto>(levelTermSectionDomain);
            return CreatedAtAction(nameof(GetByRoomNumber), new { id = levelTermSectionDomain.Id }, LevelTermSectionDto);
        }

        [HttpPost]
        [Route("AssignClassrooms")]
        public async Task<IActionResult> AssignClassrooms()
        {
            await _levelTermSectionsRepository.SeedClassroomsForLevelTermSectionsAsync();
            return Ok("Classrooms assigned to Sections successfully.");
        }

        [HttpPost]
        [Route("AssignTeachers")]
        public async Task<IActionResult> AssignTeachers()
        {
            await _levelTermSectionsRepository.SeedTeacherAssignmentsAsync();
            return Ok("Teachers assigned to Sections successfully.");
        }
    }
}