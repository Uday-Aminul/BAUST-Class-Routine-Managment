using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class UpdateTeacherRequestDto
    {
        [Required]
        public string Name { get; set; }

        public string? Code { get; set; }

        [Required]
        public string Designation { get; set; }
    }
}