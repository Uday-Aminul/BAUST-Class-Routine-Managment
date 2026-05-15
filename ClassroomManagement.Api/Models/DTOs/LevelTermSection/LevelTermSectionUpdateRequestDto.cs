using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.LevelTermSection
{
    public class LevelTermSectionUpdateRequestDto
    {
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }

        public List<ClassroomForLevelTermSectionDto> Classrooms { get; set; }
    }
}