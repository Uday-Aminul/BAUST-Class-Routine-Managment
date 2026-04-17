using AutoMapper;
using ClassroomManagement.Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace ClassroomManagement.Api.Controllers
{
    [Route("[controller]")]
    public class LevelTermSectionsController : ControllerBase
    {
        private readonly ILevelTermSectionRepository _levelTermSectionRepository;
        private readonly IMapper _mapper;

        public LevelTermSectionsController(ILevelTermSectionRepository labroomRepository, IMapper mapper)
        {
            _levelTermSectionRepository = labroomRepository;
        }

        [HttpPost]
        [Route("AssignClassrooms")]
        public async Task<IActionResult> AssignClassrooms()
        {
            await _levelTermSectionRepository.SeedClassroomsForLevelTermSectionsAsync();
            return Ok("Classrooms assigned to Sections successfully.");
        }

    }
}