using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class ClassroomDto
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        //Navigation Property
        public List<LevelTermSectionForClassroomDto>? LevelTermSections { get; set; }
        public List<ClassScheduleForClassroomDto>? ClassSchedules { get; set; }
    }
}