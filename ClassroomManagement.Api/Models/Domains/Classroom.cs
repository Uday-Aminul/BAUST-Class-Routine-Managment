using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class Classroom
    {
        public int Id { get; set; }

        //Navigation Property
        [JsonIgnore]
        public List<ClassSchedule>? ClassSchedules { get; set; }

        //What to do with other labs that are on the other departments?
    }
}