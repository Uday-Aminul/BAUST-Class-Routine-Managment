using ClassroomManagement.Api.Models.DTOs.ClassSchedules;

namespace ClassroomManagement.Api.Models.DTOs.ClassSchedules
{
    public class ClassScheduleDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }

        //Navigation Properties
        public ClassroomForClassScheduleDto? Classroom { get; set; }
        public LabroomForLabroomDto? Labroom { get; set; }
        public CourseForClassScheduleDto Course { get; set; }
        public SessionalForClassScheduleDto Sessional { get; set; }
        public TeacherForClassScheduleDto Teacher { get; set; }
    }
}