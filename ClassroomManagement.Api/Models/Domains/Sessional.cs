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

        //Foreign Key
        public int? TeacherId { get; set; }

        //Navigation Property
        public Teacher? Teacher { get; set; }
        public List<Labroom>? Labrooms { get; set; }
    }
}