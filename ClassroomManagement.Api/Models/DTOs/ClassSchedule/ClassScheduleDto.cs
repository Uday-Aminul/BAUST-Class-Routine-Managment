using ClassroomManagement.Api.Models.DTOs.ClassSchedules;

namespace ClassroomManagement.Api.Models.DTOs.ClassSchedules
{
    public class ClassScheduleDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }

        public string? WeekType { get; set; }

        //Navigation Properties
        public ClassroomForClassScheduleDto? Classroom { get; set; }
        public LabroomForClassScheduleDto? Labroom { get; set; }
        public CourseForClassScheduleDto? Course { get; set; }
        public SessionalForClassScheduleDto? Sessional { get; set; }
        public List<TeacherForClassScheduleDto> Teachers { get; set; }
    }
}