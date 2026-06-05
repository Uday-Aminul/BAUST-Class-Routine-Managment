using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.TeacherAssignment
{
    public class TeacherAssignmentUpdateRequestDto
    {
        //Foreign Key
        [Required]
        public int LevelTermSectionId { get; set; }

        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }

        //For navigation Property
        [Required]
        public List<int> TeacherIds { get; set; }
    }
}