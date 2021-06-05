using System.Collections.Generic;
using System.Threading.Tasks;
using TimeManagement.Entities;

namespace TimeManagement.Services
{
    public interface ICoursesService
    {
        Task AddCourse(string termId, Course course);
        Task<bool> CourseRepeat(Course course);
        Task<bool> DeleteCourseAsync(string id);
        Task<Course> GetCourseByIdAsync(string id);
        Task<IList<Course>> GetCoursesByTermNameAsync(string termName);
        Task<IList<Event>> GetEventsByCourseIdAsync(string courseId);
        Task<bool> SaveAsync();
        Task<bool> TermExists(string id);
        Task UpdateCourseAsync(string termId, Course course);
    }
}
