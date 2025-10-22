using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class AddClassScheduleRequestDto
    {
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Section { get; set; }

        public int ClassroomId { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }
    }
}