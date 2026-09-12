using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs.ClassSchedule;

namespace ClassScheduleManagement.Api.Repositories
{
    public interface IClassScheduleRepository
    {
        Task<List<ClassSchedule>> GetAllClassSchedulesAsync(int? level, int? term, string? section);
        Task<List<ClassSchedule>> GetAllClassSchedulesByDayAsync(int level, int term, string section, DayOfWeek day);
        Task<ClassSchedule?> GetClassScheduleByIdAsync(int id);
        Task<ClassSchedule> CreateClassScheduleAsync(ClassSchedule ClassSchedule, List<int>? teacherIds);
        Task<ClassSchedule?> UpdateClassScheduleByIdAsync(int id, ClassSchedule ClassSchedule, List<int>? teacherIds);
        Task<bool> DeleteAllClassSchedulesAsync();
        Task<List<String>> InputClassSchedulesAsync(Stream stream);
    }
}