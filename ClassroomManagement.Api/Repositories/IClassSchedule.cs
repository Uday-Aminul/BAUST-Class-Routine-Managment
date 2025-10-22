using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;

namespace ClassScheduleManagement.Api.Repositories
{
    public interface IClassScheduleRepository
    {
        Task<List<ClassSchedule>> GetAllClassSchedulesAsync();
        Task<ClassSchedule?> GetClassScheduleByIdAsync(int id);
        Task<ClassSchedule> CreateClassScheduleAsync(ClassSchedule ClassSchedule);
        Task<ClassSchedule?> UpdateClassScheduleByIdAsync(int id, ClassSchedule ClassSchedule);
        Task<List<ClassSchedule>?> DeleteClassScheduleByIdAsync(int id);
    }
}