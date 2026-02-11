using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Designation { get; set; }
        public float AssignedCredit { get; set; }

        //Navigation Property
        public List<Course>? AssignedCourses { get; set; }
        public List<Sessional>? AssignedSessionals { get; set; }
        public List<ClassSchedule>? Classes { get; set; }
    }
}