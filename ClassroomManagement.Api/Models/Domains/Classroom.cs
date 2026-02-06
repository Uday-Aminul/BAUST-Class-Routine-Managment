namespace ClassroomManagement.Api.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        //Navigation Property
        public List<ClassSchedule>? ClassSchedules { get; set; }
    }
}