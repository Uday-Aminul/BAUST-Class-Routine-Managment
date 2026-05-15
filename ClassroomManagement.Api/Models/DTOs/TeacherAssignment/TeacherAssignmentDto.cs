using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.TeacherAssignment
{
    public class TeacherAssignmentDto
    {
        public int Id { get; set; }

        //navigation Property
        public LevelTermSectionForTeacherAssignmentDto LevelTermSection { get; set; }
        public CourseForTeacherAssignmentDto? Course { get; set; }
        public SessionalForTeacherAssignmentDto? Sessional { get; set; }
        public List<TeacherForTeacherAssignmentDto> Teachers { get; set; }
    }
}