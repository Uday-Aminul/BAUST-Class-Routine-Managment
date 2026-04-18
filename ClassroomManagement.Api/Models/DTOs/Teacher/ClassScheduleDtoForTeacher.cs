using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.DTOs.ClassSchedules;

namespace ClassroomManagement.Api.Models.DTOs.Teacher
{
    public class ClassScheduleDtoForTeacher
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? WeekType { get; set; }

        //Navigation Properties
        public ClassroomForClassScheduleDto? Classroom { get; set; }
        public LabroomForClassScheduleDto? Labroom { get; set; }
        public CourseForClassScheduleDto? Course { get; set; }
        public SessionalForClassScheduleDto? Sessional { get; set; }
    }
}