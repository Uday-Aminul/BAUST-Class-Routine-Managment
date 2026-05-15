using ClassroomManagement.Api.Models.Domains;

namespace ClassroomManagement.Api.Models.DTOs
{
    public class AddTeacherRequestDto
    {
        public string Name { get; set; }
        public string? Code { get; set; }
        public string Designation { get; set; }
    }
}