using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class ClassSchedule
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly EndTime { get; set; }
        public string? SessionalStatus { get; set; }  // Even or Odd for 0.75credit sessionals

        //Foreign Keys
        public int ClassroomId { get; set; }
        public int CourseId { get; set; }
        public int TeacherId { get; set; }

        //Navigation Properties
        [JsonIgnore]
        public Classroom Classroom { get; set; }
        [JsonIgnore]
        public Course Course { get; set; }
        [JsonIgnore]
        public Teacher? Teacher { get; set; }
    }
}