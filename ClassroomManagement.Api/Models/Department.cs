using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }

        //Navigation Property
        public List<Teacher> Teachers { get; set; }
    }
}