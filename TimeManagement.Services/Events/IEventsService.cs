using System.Threading.Tasks;
using TimeManagement.Entities;

namespace TimeManagement.Services
{
    public interface IEventsService
    {
        Task AddEventAsync(string courseId, Event @event);
        Task<bool> CourseExists(string id);
        Task<bool> DeleteEvent(string id);
        Task<bool> EventRepeat(Event @event);
        Task<Event> GetEventByIdAsync(string id);
        Task<bool> SaveAsync();
        Task UpdateEventAsync(string courseId, Event @event);
    }
}
