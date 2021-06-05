using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeManagement.Entities;
using TimeManagement.EntityFrameworkCore;

namespace TimeManagement.Services
{
    public class CoursesService : ICoursesService
    {
        private readonly TimeManagementDbContext _dbContext;

        public CoursesService(TimeManagementDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<bool> TermExists(string id)
        {
            return await _dbContext.Terms.AnyAsync(x => x.ID == id);
        }

        public async Task<Course> GetCourseByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var query = await _dbContext.Courses.SingleOrDefaultAsync(x => x.ID == id);

            var events = await _dbContext.Events
                .Where(x => x.CourseID == id)
                .OrderBy(x => x.Time)
                .ToListAsync();

            query.Events = events;

            return query;
        }

        public async Task<IList<Course>> GetCoursesByTermNameAsync(string termName)
        {
            if (string.IsNullOrEmpty(termName))
            {
                throw new ArgumentNullException(nameof(termName));
            }

            var courses = await _dbContext.Courses
                .Where(x => x.Term.Name == termName)
                .Include(x => x.Events)
                .OrderBy(x => x.Name)
                .ToListAsync();

            foreach(var course in courses)
            {
                course.Events = course.Events?.OrderBy(x => x.Time).ToList();
            }

            return courses;
        }

        public async Task AddCourse(string termId, Course course)
        {
            if (string.IsNullOrEmpty(termId))
            {
                throw new ArgumentNullException(nameof(termId));
            }
            if (course is null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (!await TermExists(termId))
            {
                throw new KeyNotFoundException($"the Term with ID {termId} is not found");
            }
            if (await CourseRepeat(course))
            {
                throw new ArgumentException($"the Course Doesn't allow duplicates!");
            }

            course.ID = Guid.NewGuid().ToString();
            course.TermID = termId;

            await _dbContext.Courses.AddAsync(course);
        }

        public async Task UpdateCourseAsync(string termId, Course course)
        {
            if (string.IsNullOrEmpty(termId))
            {
                throw new ArgumentNullException(nameof(termId));
            }
            if (course is null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            if (!await TermExists(termId))
            {
                throw new KeyNotFoundException($"the Term with ID {termId} is not found");
            }
            if (await CourseRepeat(course))
            {
                throw new ArgumentException($"the Course Doesn't allow duplicates!");
            }

            _dbContext.Courses.Update(course);
        }

        public async Task<bool> DeleteCourseAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var course = await GetCourseByIdAsync(id);

            if (course is null)
            {
                return false;
            }
            else
            {
                _dbContext.Courses.Remove(course);
                return true;
            }
        }

        public async Task<bool> CourseRepeat(Course course)
        {
            return await _dbContext.Courses
                .AnyAsync(x => x.ID != course.ID
                          && x.TermID == course.TermID
                          && x.Name == course.Name);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }

        public async Task<IList<Event>> GetEventsByCourseIdAsync(string courseId)
        {
            if (string.IsNullOrEmpty(courseId))
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            var course = await GetCourseByIdAsync(courseId);

            if (course == null)
            {
                throw new ArgumentException();
            }

            return course?.Events;
        }
    }
}
