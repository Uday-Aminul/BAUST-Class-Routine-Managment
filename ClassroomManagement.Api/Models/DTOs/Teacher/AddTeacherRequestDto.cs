using System.ComponentModel.DataAnnotations;
using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class AddTeacherRequestDto
    {
        [Required]
        public string Name { get; set; }

        public string? Code { get; set; }

        [Required]
        public string Designation { get; set; }
    }
}