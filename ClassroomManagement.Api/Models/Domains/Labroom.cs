using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.Domains
{
    public class Labroom
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Property
        public List<Sessional>? Sessionals { get; set; }
        public List<ClassSchedule>? ClassSchedules { get; set; }
    }
}