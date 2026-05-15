using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Labroom
{
    public class AddLabroomRequestDto
    {
        public int RoomNumber { get; set; }
        public string? Name { get; set; }
    }
}