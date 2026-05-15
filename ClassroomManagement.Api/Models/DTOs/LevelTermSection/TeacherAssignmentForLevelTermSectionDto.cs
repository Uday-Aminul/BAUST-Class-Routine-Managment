using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.LevelTermSection
{
    public class TeacherAssignmentForLevelTermSectionDto
    {
        public int Id { get; set; }

        //navigation Property
        public CourseForLevelTermSectionDto? Course { get; set; }
        public SessionalForLevelTermSectionDto? Sessional { get; set; }
        public List<TeacherForLevelTermSectionDto> Teachers { get; set; }
    }
}