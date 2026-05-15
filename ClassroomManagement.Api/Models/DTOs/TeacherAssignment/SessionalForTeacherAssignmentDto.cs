using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.TeacherAssignment
{
    public class SessionalForTeacherAssignmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string SessionalCode { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public float Credit { get; set; }
    }
}