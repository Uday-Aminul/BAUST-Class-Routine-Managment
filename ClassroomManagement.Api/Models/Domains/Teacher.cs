using System.ComponentModel.DataAnnotations.Schema;
using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Designation { get; set; }
        [NotMapped]
        public float AssignedCredit => (AssignedCourses?.Sum(c => c.Credit) ?? 0) + (AssignedSessionals?.Sum(s => s.Credit) ?? 0);

        //navigation Property
        public List<Course>? AssignedCourses { get; set; }
        public List<Sessional>? AssignedSessionals { get; set; }
        public List<ClassSchedule>? Classes { get; set; }
    }
}