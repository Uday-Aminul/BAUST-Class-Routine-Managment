using AutoMapper;
using ClassroomManagement.Api.Models;
using ClassroomManagement.Api.Models.Domains;
using ClassroomManagement.Api.Models.DTOs;
using ClassroomManagement.Api.Models.DTOs.Classroom;
using ClassroomManagement.Api.Models.DTOs.ClassSchedules;
using ClassroomManagement.Api.Models.DTOs.Course;
using ClassroomManagement.Api.Models.DTOs.Labroom;
using ClassroomManagement.Api.Models.DTOs.LevelTermSection;
using ClassroomManagement.Api.Models.DTOs.Sessional;
using ClassroomManagement.Api.Models.DTOs.Teacher;
using ClassroomManagement.Api.Models.DTOs.TeacherAssignment;

namespace ClassroomManagement.Api.Mappings
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            //Classroom mappings
            CreateMap<Classroom, ClassroomDto>();
            CreateMap<UpdateClassroomRequestDto, Classroom>();
            CreateMap<AddClassroomRequestDto, Classroom>();
            CreateMap<Teacher, TeacherForClassroomDto>();
            CreateMap<ClassSchedule, ClassScheduleForClassroomDto>();
            CreateMap<LevelTermSection, LevelTermSectionForClassroomDto>();
            CreateMap<Course, CourseForClassroomDto>();

            //Labroom mappings
            CreateMap<Labroom, LabroomDto>();
            CreateMap<Teacher, TeacherForLabroomDto>();
            CreateMap<ClassSchedule, ClassScheduleForLabroomDto>();
            CreateMap<Sessional, SessionalForLabroomDto>();

            //Course mappings
            CreateMap<Course, CourseDto>();

            //Sessional mappings
            CreateMap<Sessional, SessionalDto>();
            CreateMap<Labroom, LabroomForSessionalDto>();

            //Teacher mappings
            CreateMap<Teacher, TeacherDto>();
            CreateMap<AddTeacherRequestDto, Teacher>();
            CreateMap<UpdateTeacherRequestDto, Teacher>();
            CreateMap<TeacherAssignment, TeacherAssignmentForTeacherDto>();
            CreateMap<Course, CourseForTeacherDto>();
            CreateMap<Sessional, SessionalForTeacherDto>();
            CreateMap<LevelTermSection, LevelTermSectionForTeacherDto>();
            CreateMap<ClassSchedule, ClassScheduleForTeacherDto>();

            //ClassSchedule mappings
            CreateMap<ClassSchedule, ClassScheduleDto>();
            CreateMap<Teacher, TeacherForClassScheduleDto>();
            CreateMap<Classroom, ClassroomForClassScheduleDto>();
            CreateMap<Labroom, LabroomForClassScheduleDto>();
            CreateMap<Course, CourseForClassScheduleDto>();
            CreateMap<Sessional, SessionalForClassScheduleDto>();

            //TeacherAssignment mappings 
            CreateMap<TeacherAssignment, TeacherAssignmentDto>();
            CreateMap<Teacher, TeacherForTeacherAssignmentDto>();
            CreateMap<Course, CourseForTeacherAssignmentDto>();
            CreateMap<Sessional, SessionalForTeacherAssignmentDto>();
            CreateMap<LevelTermSection, LevelTermSectionForTeacherAssignmentDto>();

            //LevelTermSection mappings
            CreateMap<LevelTermSection, LevelTermSectionDto>();
            CreateMap<Teacher, TeacherForLevelTermSectionDto>();
            CreateMap<Classroom, ClassroomForLevelTermSectionDto>();
            CreateMap<TeacherAssignment, TeacherAssignmentForLevelTermSectionDto>();
            CreateMap<TeacherAssignment, TeacherAssignmentForLevelTermSectionDto>();
        }
    }
}