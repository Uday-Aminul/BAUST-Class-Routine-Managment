using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Models.DTOs.Labroom
{
    public class LabroomUpdateRequestDto
    {
        public string Name { get; set; }
        public List<string> AllowedSessionalCodes { get; set; }
    }
}