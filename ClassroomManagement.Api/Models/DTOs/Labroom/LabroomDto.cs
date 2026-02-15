using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models.DTOs.Labroom
{
    public class LabroomDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Property
        public List<SessionalForLabroomDto>? AllowedSessionals { get; set; }
        public List<ClassScheduleForLabroomDto>? ClassSchedules { get; set; }
    }
}