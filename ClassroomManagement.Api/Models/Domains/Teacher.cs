using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Designation { get; set; }

        //navigation Property
        [JsonIgnore]
        public List<Course>? Courses { get; set; }
        [JsonIgnore]
        public List<ClassSchedule>? Classes { get; set; }

        //Credit of a teacher is determined by how many courses he is assigned to?      
    }
}