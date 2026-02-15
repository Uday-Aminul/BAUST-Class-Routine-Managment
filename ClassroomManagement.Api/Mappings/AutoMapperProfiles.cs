using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Models.DTOs.Classroom;
using ClassroomManagement.Api.Models.DTOs.ClassSchedules;
using ClassroomManagement.Api.Models.DTOs.Labroom;

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
            CreateMap<Teacher, TeacherForLabroomDto>();

            CreateMap<Classroom, ClassroomDto>();
            //CreateMap<UpdateClassroomRequestDto, Classroom>();
            CreateMap<AddClassroomRequestDto, Classroom>();
            CreateMap<Classroom, ClassroomForClassScheduleDto>();

            CreateMap<Labroom, LabroomDto>();
            CreateMap<Labroom, LabroomForLabroomDto>();

            CreateMap<ClassSchedule, ClassScheduleDto>();
            CreateMap<UpdateClassScheduleRequestDto, Classroom>();
            CreateMap<AddClassScheduleRequestDto, Classroom>();
            CreateMap<ClassSchedule, ClassScheduleForClassroomDto>();
            CreateMap<ClassSchedule, ClassScheduleForLabroomDto>();

            CreateMap<LevelTerm, LevelTermForClassroomDto>();

            CreateMap<Course, CourseForClassroomDto>();
            CreateMap<Course, CourseForClassScheduleDto>();
            CreateMap<Course, CourseForLabroomDto>();

            CreateMap<Sessional, SessionalForClassScheduleDto>();
            CreateMap<Sessional, SessionalForLabroomDto>();
        }
    }
}