using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class ClassroomDto
    {
        public int Id { get; set; }

        //Navigation Property
        public List<ClassScheduleDto>? ClassSchedules { get; set; }
    }
}