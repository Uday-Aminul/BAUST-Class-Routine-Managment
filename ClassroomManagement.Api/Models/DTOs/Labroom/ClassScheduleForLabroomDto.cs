using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models.DTOs.Labroom
{
    public class ClassScheduleForLabroomDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }

        public string? WeekType { get; set; }
        public SessionalForLabroomDto Sessional { get; set; }
        public List<TeacherForLabroomDto> Teachers { get; set; }
    }
}