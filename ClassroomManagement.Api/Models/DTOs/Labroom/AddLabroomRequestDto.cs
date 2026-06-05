using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Labroom
{
    public class AddLabroomRequestDto
    {
        [Required]
        public int RoomNumber { get; set; }
        public string? Name { get; set; }
        public List<int>? AllowedSessionalIds { get; set; }
    }
}