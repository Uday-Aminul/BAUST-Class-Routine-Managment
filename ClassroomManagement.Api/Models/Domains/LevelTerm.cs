namespace ClassroomManagement.Api.Models.Domains
{
    public class LevelTerm
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }//must be string bcz 1-II not 1-2
        //Should put section in here later

        //Foreign Key
        public int ClassroomId { get; set; }

        //Navigation Property
        public Classroom Classroom { get; set; }
    }
}