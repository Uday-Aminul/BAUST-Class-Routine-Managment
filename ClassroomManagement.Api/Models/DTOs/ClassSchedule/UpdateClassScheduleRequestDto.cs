using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.DTOs.Classroom;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class UpdateClassScheduleRequestDto
    {
        [Required]
        [EnumDataType(typeof(DayOfWeek))]
        public DayOfWeek Day { get; set; }

        [Required]
        public TimeOnly StartTime { get; set; }

        [Required]
        public TimeOnly EndTime { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Level must be between 1 and 4.")]
        public int Level { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Term must be either 1 or 2.")]
        public int Term { get; set; }

        [Required]
        public string Section { get; set; }

        public string? WeekType { get; set; }

        public int? ClassroomId { get; set; }
        public int? LabroomId { get; set; }
        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }

        //For Navigation Properties
        [Required]
        [MinLength(1, ErrorMessage = "At least one teacher is required.")]
        public List<int> TeacherIds { get; set; }
    }
}