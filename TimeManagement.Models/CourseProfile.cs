using AutoMapper;
using TimeManagement.Entities;

namespace TimeManagement.Models
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>()
                .ReverseMap();

            CreateMap<CourseAddDto, Course>()
                .ReverseMap();

            CreateMap<CourseUpdateDto, Course>()
                .ReverseMap();
        }
    }
}
