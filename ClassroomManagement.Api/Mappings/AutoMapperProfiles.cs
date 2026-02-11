using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Models.DTOs.Classroom;
using ClassroomManagement.Api.Models.DTOs.ClassSchedules;

namespace ClassroomManagement.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Teacher, TeacherDto>();
            CreateMap<AddTeacherRequestDto, Teacher>();
            CreateMap<UpdateTeacherRequestDto, Teacher>();
            CreateMap<Teacher, TeacherForClassroomDto>();
            CreateMap<Teacher, TeacherForClassScheduleDto>();

            CreateMap<Classroom, ClassroomDto>();
            //CreateMap<UpdateClassroomRequestDto, Classroom>();
            CreateMap<AddClassroomRequestDto, Classroom>();
            CreateMap<Classroom, ClassroomForClassScheduleDto>();

            CreateMap<Labroom, LabroomForLabroomDto>();

            CreateMap<ClassSchedule, ClassScheduleDto>();
            CreateMap<UpdateClassScheduleRequestDto, Classroom>();
            CreateMap<AddClassScheduleRequestDto, Classroom>();
            CreateMap<ClassSchedule, ClassScheduleForClassroomDto>();


            CreateMap<LevelTerm, LevelTermForClassroomDto>();

            CreateMap<Course, CourseForClassroomDto>();
            CreateMap<Course, CourseForClassScheduleDto>();
            CreateMap<Sessional, SessionalForClassScheduleDto>();
        }
    }
}