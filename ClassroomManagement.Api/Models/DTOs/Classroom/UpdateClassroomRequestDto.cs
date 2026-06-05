using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Classroom
{
    public class UpdateClassroomRequestDto
    {
        [Required]
        public int RoomNumber { get; set; }
    }
}