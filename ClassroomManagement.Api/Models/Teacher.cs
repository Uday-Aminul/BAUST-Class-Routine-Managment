using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }

        //Foreign Key
        public int DepartmentId { get; set; }

        //navigation Property
        public List<Course> Courses { get; set; }
        public List<ClassSchedule> Classes { get; set; }
        public Department Department { get; set; }        
    }
}