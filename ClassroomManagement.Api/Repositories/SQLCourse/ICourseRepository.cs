using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Models;

namespace ClassroomManagement.Api.Repositories.SQLCourse
{
    public interface ICourseRepository
    {
        Task<List<Course>> GetAllCoursesAsync();
        Task<Course?> GetCourseByIdAsync(int id);
        Task<Course> CreateCourseAsync(Course course);
        Task<Course?> UpdateCourseByIdAsync(int id, Course course);
        Task<List<Course>?> DeleteCourseByIdAsync(int id);
    }
}