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
    public class CoursesController : ControllerBase
    {
        private readonly ICoursesService _coursesService;
        private readonly IMapper _mapper;

        public CoursesController(ICoursesService coursesService, IMapper mapper)
        {
            _coursesService = coursesService ?? throw new ArgumentNullException(nameof(coursesService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("course/{id:required}", Name = nameof(GetCourseAsync))]
        public async Task<IActionResult> GetCourseAsync(string id)
        {
            var course = await _coursesService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<CourseDto>(course));
        }

        [HttpGet("course/term-name/{termName:required}")]
        public async Task<IActionResult> GetCourseByTermNameAsync(string termName)
        {
            var courses = await _coursesService.GetCoursesByTermNameAsync(termName);

            if (courses == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IList<CourseDto>>(courses));
        }

        [HttpGet("GetEventsByCourseId/{courseId:required}")]
        public async Task<IActionResult> GetCoursesByTermIdAsync(string courseId)
        {
            try
            {
                var events = await _coursesService.GetEventsByCourseIdAsync(courseId);

                return Ok(_mapper.Map<IList<EventDto>>(events));
            }
            catch (ArgumentNullException)
            {
                return BadRequest();
            }
            catch (ArgumentException)
            {
                return NotFound();
            }
        }

        [HttpPost("course")]
        public async Task<IActionResult> CreateCourseAsync(string termId, CourseAddDto courseAddDto)
        {
            if (!await _coursesService.TermExists(termId))
            {
                return NotFound();
            }

            var course = _mapper.Map<Course>(courseAddDto);

            if (await _coursesService.CourseRepeat(course))
            {
                ModelState.AddModelError(nameof(course.Name), $"The course {course.Name} already exists!");
                return ValidationProblem(ModelState);
            }

            try
            {
                await _coursesService.AddCourse(termId, course);
            }
            catch(ArgumentNullException)
            {
                return BadRequest();
            }

            if(await _coursesService.SaveAsync())
            {
                return Created(nameof(GetCourseAsync), new { course.ID});
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPut("course/{id:required}")]
        public async Task<IActionResult> UpdateCourseAsync(string id, CourseUpdateDto courseUpdateDto)
        {
            var course = await _coursesService.GetCourseByIdAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            _mapper.Map(courseUpdateDto, course);

            try
            {
                await _coursesService.UpdateCourseAsync(course.TermID, course);
            }
            catch (ArgumentException)
            {
                ModelState.AddModelError(nameof(course.Name), $"The course {course.Name} already exists!");
                return ValidationProblem(ModelState);
            }
            catch(KeyNotFoundException)
            {
                return NotFound();
            }
            

            if (await _coursesService.SaveAsync())
            {
                return NoContent();
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("course/{id:required}")]
        public async Task<IActionResult> DeleteCourseAsync(string id)
        {
            _ = await _coursesService.GetEventsByCourseIdAsync(id);

            if (!await _coursesService.DeleteCourseAsync(id))
            {
                return NotFound();
            }

            if (!await _coursesService.SaveAsync())
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
