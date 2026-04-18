using ClassroomManagement.Api.Models.DTOs.Classroom;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class ClassScheduleForClassroomDto
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? WeekType { get; set; }

        public CourseForClassroomDto? Course { get; set; }
        public SessionalForClassroomDto? Sessional { get; set; }
        public List<TeacherForClassroomDto> Teacher { get; set; }
    }
}