using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TimeManagement.Entities;
using TimeManagement.EntityFrameworkCore;

namespace TimeManagement.Services
{
    public class TermsService : ITermsService
    {
        private readonly TimeManagementDbContext _dbContext;
        //private readonly ILogger<TermsService> _logger;

        public TermsService(TimeManagementDbContext dbContext, ILogger<TermsService> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            //_logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Term> GetTermByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _dbContext.Terms.SingleOrDefaultAsync(x => x.ID == id);
        }

        public async Task<IList<Term>> GetTerms()
        {
            var terms = await _dbContext.Terms
                .Include(x => x.Courses)
                .ThenInclude(x => x.Events)
                .OrderBy(x => x.TermNumber).ToListAsync();

            foreach(var term in terms)
            {
                term.Courses = term.Courses?.OrderBy(x => x.Name).ToList();

                foreach(var course in term.Courses)
                {
                    course.Events = course.Events?.OrderBy(x => x.Time).ToList();
                }
            }

            return terms;
        }

        public async Task AddTermAsync(Term term)
        {
            if (term is null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            if (await TermRepeat(term))
            {
                throw new ArgumentException($"The {nameof(term)} doesn't allow duplicate!");
            }

            term.ID = Guid.NewGuid().ToString();

            await _dbContext.Terms.AddAsync(term);
        }

        public async Task UpdateTermAsync(Term term)
        {
            if (term is null)
            {
                throw new ArgumentNullException(nameof(term));
            }

            if (await TermRepeat(term))
            {
                throw new ArgumentException($"The {nameof(term)} doesn't allow duplicate!");
            }

            _dbContext.Terms.Update(term);
        }

        public async Task<bool> DeleteTerm(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var term = await GetTermByIdAsync(id);

            if (term == null)
            {
                return false;
            }
            else
            {
                _dbContext.Terms.Remove(term);

                return true;
            }
        }

        public async Task<IList<Course>> GetCoursesByTermIdAsync(string termId)
        {
            if (string.IsNullOrEmpty(termId))
            {
                throw new ArgumentNullException(nameof(termId));
            }

            var term = await GetTermByIdAsync(termId);

            //if (term == null)
            //{
            //    throw new ArgumentException("The term doesn't exist!");
            //}

            return term?.Courses;
        }

        public async Task<bool> TermRepeat(Term term)
        {
            return await _dbContext.Terms
                .AnyAsync(x => x.ID != term.ID && x.Name == term.Name);
        }

        public async Task<bool> SaveAsync()
        {
            return await _dbContext.SaveChangesAsync() >= 0;
        }
    }
}
