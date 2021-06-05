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
    [Route("api/v1")]
    [ApiController]
    public class TermsController : ControllerBase
    {
        private readonly ITermsService _termsService;
        private readonly IMapper _mapper;

        public TermsController(ITermsService termsService, IMapper mapper)
        {
            _termsService = termsService ?? throw new ArgumentNullException(nameof(termsService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("term/{id:required}", Name = nameof(GetTermByIdAsync))]
        public async Task<IActionResult> GetTermByIdAsync(string id)
        {
            var term = await _termsService.GetTermByIdAsync(id);

            if (term == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<Term, TermDto>(term));
        }

        [HttpGet("terms")]
        public async Task<IActionResult> GetTermsAsync()
        {
            return Ok(_mapper.Map<IList<TermDto>>(await _termsService.GetTerms()));
        }



        [HttpPost("term")]
        public async Task<IActionResult> CreateTermAsync(TermAddDto termAddDto)
        {
            Term term = _mapper.Map<Term>(termAddDto);

            if (await _termsService.TermRepeat(term))
            {
                ModelState.AddModelError(nameof(term.Name), $"The term {term.Name} already exists!");

                return ValidationProblem(ModelState);
            }

            await _termsService.AddTermAsync(term);

            if (await _termsService.SaveAsync())
            {
                return Created(nameof(GetTermByIdAsync), new {term.ID});
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpDelete("term/{id:required}")]
        public async Task<IActionResult> DeleteTermAsync(string id)
        {
            _ = _termsService.GetCoursesByTermIdAsync(id);

            if (!await _termsService.DeleteTerm(id))
            {
                return NotFound();
            }

            await _termsService.SaveAsync();

            return NoContent();
        }

        [HttpPut("term/{id:required}")]
        public async Task<IActionResult> UpdateTermAsync(string id, TermUpdateDto termUpdateDto)
        {
            var term = await _termsService.GetTermByIdAsync(id);

            if (term == null)
            {
                return NotFound();
            }

            var termToUpdate = _mapper.Map(termUpdateDto, term);

            try
            {
                await _termsService.UpdateTermAsync(termToUpdate);
            }
            catch(ArgumentException)
            {
                ModelState.AddModelError(nameof(termToUpdate.Name), $"The {nameof(termToUpdate.Name)} already exists!");
                return ValidationProblem(ModelState);
            }

            await _termsService.SaveAsync();

            return NoContent();
        }
    }
}
