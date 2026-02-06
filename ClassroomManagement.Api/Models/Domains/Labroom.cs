namespace ClassroomManagement.Api.Models.Domains
{
    public class Labroom
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Property
        public List<Sessional>? AllowedSessionals { get; set; }
        public List<ClassSchedule>? ClassSchedules { get; set; }
    }
}