using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Classroom
{
    public class TeacherForClassroomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Designation { get; set; }
        public float AssignedCredit { get; set; }
    }
}