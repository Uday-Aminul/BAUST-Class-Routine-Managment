using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class UpdateTeacherRequestDto
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Designation { get; set; }

        public List<int>? Courses { get; set; }
        public List<int>? AssignedSessionals { get; set; }
    }
}