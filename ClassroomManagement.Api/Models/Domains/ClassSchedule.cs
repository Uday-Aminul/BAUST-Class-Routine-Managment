using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class ClassSchedule
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string Section { get; set; }

        //Foreign Keys
        public int ClassroomId { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }

        //Navigation Properties
        public Classroom Classroom { get; set; }
        public Course Course { get; set; }
        public Teacher? Teacher { get; set; }
    }
}