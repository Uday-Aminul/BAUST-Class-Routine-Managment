namespace ClassroomManagement.Api.Models.Domains
{
    public class LevelTermSection
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }//must be string bcz 1-II not 1-2
        public string? Section { get; set; }

        //Navigation Property
        public List<Classroom> Classrooms { get; set; }
    }
}