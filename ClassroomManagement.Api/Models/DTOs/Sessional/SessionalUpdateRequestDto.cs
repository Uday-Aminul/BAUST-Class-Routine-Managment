using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Sessional
{
    public class SessionalUpdateRequestDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string SessionalCode { get; set; }

        [Required]
        [Range(1, 4, ErrorMessage = "Level must be between 1 and 4.")]
        public int Level { get; set; }

        [Required]
        [Range(1, 2, ErrorMessage = "Term must be either 1 or 2.")]
        public int Term { get; set; }

        [Required]
        public float Credit { get; set; }

        //For Navigation Property
        public List<int>? LabroomIds { get; set; }
    }
}