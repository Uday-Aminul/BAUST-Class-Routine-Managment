using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Labroom
{
    public class LabroomUpdateRequestDto
    {
        public int RoomNumber { get; set; }
        public string Name { get; set; }
        public List<int> AllowedSessionalIds { get; set; }
    }
}