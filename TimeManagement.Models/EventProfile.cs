using AutoMapper;
using TimeManagement.Entities;

namespace TimeManagement.Models
{
    public class EventProfile:Profile
    {
        public EventProfile()
        {
            CreateMap<Event, EventDto>()
                .ForMember(dest => dest.RepeatDays, opt => opt.MapFrom(src => src.RepeatDays.ToString()));

            CreateMap<EventAddDto, Event>()
                .ForMember(model => model.RepeatDays, entity => entity.Ignore());

            CreateMap<EventUpdateDto, Event>()
                .ForMember(model => model.RepeatDays, entity => entity.Ignore());
        }
    }
}
