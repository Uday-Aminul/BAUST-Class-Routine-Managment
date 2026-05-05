using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.Domains
{
    public class TeacherAssignment
    {
        public int Id { get; set; }

        //Foreign Key
        public int LevelTermSectionId { get; set; }
        public int? CourseId { get; set; }
        public int? SessionalId { get; set; }

        //navigation Property
        public LevelTermSection LevelTermSection { get; set; }
        public Course? Course { get; set; }
        public Sessional? Sessional { get; set; }
        public List<Teacher> Teacher { get; set; }
    }
}