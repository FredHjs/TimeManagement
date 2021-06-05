using AutoMapper;
using TimeManagement.Entities;

namespace TimeManagement.Models
{
    public class TermProfile : Profile
    {
        public TermProfile()
        {
            CreateMap<Term, TermDto>()
                .ReverseMap();

            CreateMap<TermAddDto, Term>()
                .ReverseMap();

            CreateMap<TermUpdateDto, Term>()
                .ReverseMap();
        }
    }
}
