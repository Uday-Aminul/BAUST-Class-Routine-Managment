using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.TeacherAssignment
{
    public class TeacherAssignmentAddRequestDto
    {
        //Foreign Key
        public int LevelTermSectionId { get; set; }
        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }

        //navigation Property
        public List<int> TeacherIds { get; set; }
    }
}