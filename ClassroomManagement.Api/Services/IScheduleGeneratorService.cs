using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Services
{
    public interface IScheduleGeneratorService
    {
        Task<string> GenerateScheduleAsync();
    }
}