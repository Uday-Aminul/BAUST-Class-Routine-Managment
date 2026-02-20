using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Services
{
    public class SchedulingState
    {
        public List<Sessional> Sessionals { get; set; }
        public List<Course> Courses { get; set; }
        public List<ClassSchedule> SchedulesToAdd { get; set; }
        public int LabPlacedToday { get; set; }
        public bool DualLabPlacement { get; set; }
    }
}