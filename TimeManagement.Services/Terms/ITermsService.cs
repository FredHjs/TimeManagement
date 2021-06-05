using System.Collections.Generic;
using System.Threading.Tasks;
using TimeManagement.Entities;

namespace TimeManagement.Services
{
    public interface ITermsService
    {
        Task AddTermAsync(Term term);
        Task<bool> DeleteTerm(string id);
        Task<IList<Course>> GetCoursesByTermIdAsync(string termId);
        Task<Term> GetTermByIdAsync(string id);
        Task<IList<Term>> GetTerms();
        Task<bool> SaveAsync();
        Task<bool> TermRepeat(Term term);
        Task UpdateTermAsync(Term term);
    }
}
