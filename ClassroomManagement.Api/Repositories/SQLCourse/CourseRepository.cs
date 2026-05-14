using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Repositories.SQLCourse;
using Microsoft.EntityFrameworkCore;

namespace CourseManagement.Api.Repositories.SQLCourse
{
    public class CourseRepository : ICourseRepository
    {
        private readonly ClassroomManagementDbContext _dbContext;
        public CourseRepository(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Course> CreateCourseAsync(Course course)
        {
            await _dbContext.Courses.AddAsync(course);
            await _dbContext.SaveChangesAsync();
            return course;
        }

        public async Task<List<Course>?> DeleteCourseByIdAsync(int id)
        {
            var Course = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (Course is null)
            {
                return null;
            }
            _dbContext.Courses.Remove(Course);
            await _dbContext.SaveChangesAsync();
            return await _dbContext.Courses.ToListAsync();
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            var courseDomains = await _dbContext.Courses.ToListAsync();
            return courseDomains;
        }

        public async Task<Course?> GetCourseByIdAsync(int id)
        {
            var courseDomain = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (courseDomain is null)
            {
                return null;
            }
            return courseDomain;
        }

        public async Task<Course?> UpdateCourseByIdAsync(int id, Course course)
        {
            var existingCourse = await _dbContext.Courses.FirstOrDefaultAsync(x => x.Id == id);
            if (existingCourse is null)
            {
                return null;
            }
            existingCourse.CourseCode = course.CourseCode;
            existingCourse.Name = course.Name;
            existingCourse.Credit = course.Credit;
            existingCourse.Level = course.Level;
            existingCourse.Term = course.Term;
            await _dbContext.SaveChangesAsync();
            return existingCourse;
        }
    }
}