using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        //Navigation Property
        public LevelTerm? LevelTerm { get; set; }
        public List<ClassSchedule>? ClassSchedules { get; set; }
    }
}