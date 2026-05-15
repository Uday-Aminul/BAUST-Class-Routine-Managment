using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.DTOs.ClassSchedules;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class AddClassScheduleRequestDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }

        public string? WeekType { get; set; }

        public int? ClassroomId { get; set; }
        public int? LabroomId { get; set; }
        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }

        //Navigation Properties
        public List<int> TeacherIds { get; set; }
    }
}