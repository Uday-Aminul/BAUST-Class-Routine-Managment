using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class TeacherDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Designation { get; set; }

        public List<Course>? Courses { get; set; }
        public List<ClassSchedule>? Classes { get; set; }
        public Department Department { get; set; }
    }
}