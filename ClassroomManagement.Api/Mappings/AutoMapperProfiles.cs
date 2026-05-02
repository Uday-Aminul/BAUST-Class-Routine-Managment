using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Models.DTOs.Classroom;
using ClassroomManagement.Api.Models.DTOs.ClassSchedules;
using ClassroomManagement.Api.Models.DTOs.Labroom;
using ClassroomManagement.Api.Models.DTOs.Teacher;

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
            CreateMap<ClassSchedule, ClassScheduleDtoForTeacher>();

            CreateMap<Classroom, ClassroomDto>();
            CreateMap<UpdateClassroomRequestDto, Classroom>();
            CreateMap<AddClassroomRequestDto, Classroom>();
            CreateMap<Classroom, ClassroomForClassScheduleDto>();

            CreateMap<Labroom, LabroomDto>();
            CreateMap<Labroom, LabroomForClassScheduleDto>();

            CreateMap<ClassSchedule, ClassScheduleDto>();
            CreateMap<UpdateClassScheduleRequestDto, Classroom>();
            CreateMap<AddClassScheduleRequestDto, Classroom>();
            CreateMap<ClassSchedule, ClassScheduleForClassroomDto>();
            CreateMap<ClassSchedule, ClassScheduleForLabroomDto>();

            CreateMap<LevelTermSection, LevelTermForClassroomDto>();

            CreateMap<Course, CourseForClassroomDto>();
            CreateMap<Course, CourseForClassScheduleDto>();
            CreateMap<Course, CourseForLabroomDto>();

            CreateMap<Sessional, SessionalForClassScheduleDto>();
            CreateMap<Sessional, SessionalForLabroomDto>();
        }
    }
}