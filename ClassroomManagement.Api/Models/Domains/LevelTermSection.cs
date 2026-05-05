namespace ClassroomManagement.Api.Models.Domains
{
    public class LevelTermSection
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }

        //Navigation Property
        public List<TeacherAssignment>? AssignedTeachers { get; set; }
        public List<Classroom> Classrooms { get; set; }
    }
}