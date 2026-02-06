using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models
{
    public class ClassSchedule
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        //public string? SessionalStatus { get; set; } Even or Odd for 0.75credit sessionals

        //Foreign Keys
        public int? ClassroomId { get; set; }
        public int? LabroomId { get; set; }
        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }
        public int TeacherId { get; set; }

        //Navigation Properties
        public Classroom? Classroom { get; set; }
        public Labroom? Labroom { get; set; }
        public Course Course { get; set; }
        public Sessional Sessional { get; set; }
        public Teacher Teacher { get; set; }
    }
}