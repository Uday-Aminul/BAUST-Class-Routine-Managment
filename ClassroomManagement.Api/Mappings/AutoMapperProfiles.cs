using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.DTOs;

namespace ClassroomManagement.Api.Mappings
{
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Teacher, TeacherDto>();
            CreateMap<AddTeacherRequestDto, Teacher>();
            CreateMap<UpdateTeacherRequestDto, Teacher>();

            CreateMap<Classroom, ClassroomDto>();
            CreateMap<UpdateClassroomRequestDto, Classroom>();
            CreateMap<AddClassroomRequestDto, Classroom>();

            CreateMap<ClassSchedule, ClassScheduleDto>();
            CreateMap<UpdateClassScheduleRequestDto, Classroom>();
            CreateMap<AddClassScheduleRequestDto, Classroom>();
        }
    }
}