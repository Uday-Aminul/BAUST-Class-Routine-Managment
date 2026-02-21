namespace ClassroomManagement.Api.Models.Domains
{
    public class Sessional
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SessionalCode { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public float Credit { get; set; }

        //Navigation Property
        public List<Teacher>? Teacher { get; set; }
        public List<Labroom>? Labrooms { get; set; }
    }
}