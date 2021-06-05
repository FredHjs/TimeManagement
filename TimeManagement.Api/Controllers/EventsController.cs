using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TimeManagement.Entities;
using TimeManagement.Models;
using TimeManagement.Services;

namespace TimeManagement.Api.Controllers
{
    [ApiController]
    [Route("api/v1")]
    public class EventsController : ControllerBase
    {
        private readonly IEventsService _eventsService;
        private readonly IMapper _mapper;

        public EventsController(IEventsService eventsService, IMapper mapper)
        {
            _eventsService = eventsService ?? throw new ArgumentNullException(nameof(eventsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("event/{id:required}", Name = nameof(GetEventAsync))]
        public async Task<IActionResult> GetEventAsync(string id)
        {
            var @event = await _eventsService.GetEventByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EventDto>(@event));
        }

        [HttpPost("event")]
        public async Task<IActionResult> CreateEventAsync(string courseId, EventAddDto eventAddDto)
        {
            if (!await _eventsService.CourseExists(courseId))
            {
                return NotFound();
            }

            var @event = _mapper.Map<Event>(eventAddDto);

            @event.RepeatDays = ConvertRepeatDays(eventAddDto.RepeatDays);

            if (await _eventsService.EventRepeat(@event))
            {
                ModelState.AddModelError(nameof(@event.Name), "This event already exists!");
                return ValidationProblem(ModelState);
            }

            try
            {
                await _eventsService.AddEventAsync(courseId, @event);
            }
            catch(ArgumentNullException)
            {
                ModelState.AddModelError(nameof(courseId), "The Course ID can't be enpty!");
                return BadRequest(ModelState);
            }

            if (await _eventsService.SaveAsync())
            {
                return Created(nameof(GetEventAsync), new { @event.ID });
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("event/{id:required}")]
        public async Task<IActionResult> UpdateEventAsync(string id, EventUpdateDto eventUpdateDto) 
        {
            var @event = await _eventsService.GetEventByIdAsync(id);

            if (@event == null)
            {
                return NotFound();
            }

            _mapper.Map(eventUpdateDto, @event);

            @event.RepeatDays = ConvertRepeatDays(eventUpdateDto.RepeatDays);

            try
            {
                await _eventsService.UpdateEventAsync(@event.CourseID, @event);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError(nameof(@event.Name), $"The event {@event.Name} already exists!");
                return ValidationProblem(ModelState);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }

            if (await _eventsService.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("event/{id:required}")]
        public async Task<IActionResult>DeleteEventAsync(string id)
        {
            if (!await _eventsService.DeleteEvent(id))
            {
                return NotFound();
            }

            if (!await _eventsService.SaveAsync())
            {
                return BadRequest();
            }

            return NotFound();
        }

        //从Dto中的string形式的RepeatDays转到entity中的2进制
        private static int ConvertRepeatDays(string repeatDays)
        {
            var i = 0;
            int repeatDay;
            foreach (char c in repeatDays)
            {
                repeatDay = Convert.ToInt32(c) - 48;

                if (repeatDay >= 0 && repeatDay <= 6)
                {
                    i += (int)Math.Pow(2, repeatDay);
                }
            }

            return i;
        }
    }
}
