using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeManagement.Entities;
using TimeManagement.EntityFrameworkCore;

namespace TimeManagement.Services
{
    public class EventsService : IEventsService
    {
        private readonly TimeManagementDbContext _dbContext;

        public EventsService(TimeManagementDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> CourseExists(string id)
        {
            return await _dbContext.Courses.AnyAsync(x => x.ID == id);
        }

        public async Task<Event> GetEventByIdAsync (string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _dbContext.Events.SingleOrDefaultAsync(x => x.ID == id);
        }

        public async Task AddEventAsync(string courseId, Event @event)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            if (!await CourseExists(courseId))
            {
                throw new KeyNotFoundException($"the Term with ID {courseId} is not found");
            }
            if (await EventRepeat(@event))
            {
                throw new ArgumentException($"the Course Doesn't allow duplicates!");
            }

            @event.ID = Guid.NewGuid().ToString();

            @event.CourseID = courseId;

            await _dbContext.Events.AddAsync(@event);
        }

        public async Task<bool> EventRepeat(Event @event)
        {
            return await _dbContext.Events
                .AnyAsync(x => x.ID != @event.ID 
                            && x.CourseID == @event.CourseID
                            && x.Name == @event.Name );
        }

        public async Task UpdateEventAsync(string courseId, Event @event)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                throw new ArgumentNullException(nameof(courseId));
            }
            if (@event is null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
            if (!await CourseExists(courseId))
            {
                throw new KeyNotFoundException($"the Term with ID {courseId} is not found");
            }
            if (await EventRepeat(@event))
            {
                throw new ArgumentException($"the Course Doesn't allow duplicates!");
            }

            _dbContext.Events.Update(@event);
        }

        public async Task<bool> DeleteEvent(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var @event = await GetEventByIdAsync(id);

            if (@event == null)
            {
                return false;
            }
            else
            {
                _dbContext.Events.Remove(@event);
                return true;
            }
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
