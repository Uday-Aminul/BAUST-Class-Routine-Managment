using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClassroomManagement.Api.Repositories
{
    public interface ILevelTermSectionRepository
    {
        Task SeedClassroomsForLevelTermSectionsAsync();
    }
}