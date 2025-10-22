using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class AddTeacherRequestDto
    {
        public string Name { get; set; }
        public string Designation { get; set; }

        public List<Course>? Courses { get; set; }
    }
}