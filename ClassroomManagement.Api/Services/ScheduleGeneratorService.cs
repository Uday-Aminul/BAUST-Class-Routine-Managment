using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;
using ClassroomManagement.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace ClassroomManagement.Api.Services
{
    public class ScheduleGeneratorService
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public ScheduleGeneratorService(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /*public async Task<bool> GenerateScheduleAsync()
        {
            var classrooms = await _dbContext.Classrooms.ToListAsync();
            var courses = await _dbContext.Courses.ToListAsync();
            var teachers = await _dbContext.Teachers.ToListAsync();
            var classSchedules = new List<Models.ClassSchedule>();
            
            //For Level-1 Term-1
            var coursesForLevel1Term1 = courses.Where(course => course.Level == 1 && course.Term == 1).ToList();
            foreach(var course in coursesForLevel1Term1)
            {
                courseCredit
            }



        }*/
    }
}