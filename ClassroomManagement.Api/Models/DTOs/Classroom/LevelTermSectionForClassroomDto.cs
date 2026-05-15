using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class LevelTermSectionForClassroomDto
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public string Section { get; set; }
    }
}