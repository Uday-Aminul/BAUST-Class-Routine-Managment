namespace ClassroomManagement.Api.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? CourseCode { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public float Credit { get; set; }

        //Foreign Key
        public int? TeacherId { get; set; }

        //Navigation Property
        public Teacher? Teacher { get; set; }
    }
}