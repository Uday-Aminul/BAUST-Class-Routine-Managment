using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Sessional
{
    public class LabroomForSessionalDto
    {
        public int Id { get; set; }
        public int RoomNumber { get; set; }
        public string? Name { get; set; }
    }
}