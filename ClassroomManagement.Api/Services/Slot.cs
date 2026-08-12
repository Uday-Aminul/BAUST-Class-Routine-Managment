using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Services
{
    public class Slot
    {
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
    }
}