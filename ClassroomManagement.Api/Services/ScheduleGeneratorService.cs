using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClassroomManagement.Api.Data;

namespace ClassroomManagement.Api.Services
{
    public class ScheduleGeneratorService
    {
        private readonly ClassroomManagementDbContext _dbContext;

        public ScheduleGeneratorService(ClassroomManagementDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /*public Task<bool> GenerateScheduleAsync()
        {

        }*/
    }
}