using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models
{
    public class Classroom
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }

        //Navigation Property
        public List<LevelTermSection>? LevelTermSections { get; set; }
        public List<ClassSchedule>? ClassSchedules { get; set; }
    }
}