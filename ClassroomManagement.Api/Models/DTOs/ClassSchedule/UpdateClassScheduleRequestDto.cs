using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.DTOs.Classroom;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class UpdateClassScheduleRequestDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }

        public string? WeekType { get; set; }

        //Foreign Keys
        public int? ClassroomId { get; set; }
        public int? LabroomId { get; set; }
        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }

        public List<int> TeacherIds { get; set; }
    }
}