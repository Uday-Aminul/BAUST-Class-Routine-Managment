using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Teacher
{
    public class TeacherAssignmentForTeacherDto
    {
        public int Id { get; set; }

        //navigation Property
        public LevelTermSectionForTeacherDto LevelTermSection { get; set; }
        public CourseForTeacherDto? Course { get; set; }
        public SessionalForTeacherDto? Sessional { get; set; }
    }
}