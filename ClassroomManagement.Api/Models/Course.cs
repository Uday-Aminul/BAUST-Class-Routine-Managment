using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public int Credit { get; set; }

        //Foreign Key
        public int? DepartmentId { get; set; }

        //Navigation Property
        public List<Teacher>? Teachers { get; set; }
        public Department? Department { get; set; }
    }
}