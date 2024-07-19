using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QueryArchive.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QueryArchive.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        private readonly QueryArchiveDbContext _context;

        public AnswersController(QueryArchiveDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Answer>>> GetAnswers(int questionId, string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Answers.Where(a => a.QuestionID == questionId);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(a => a.Content.Contains(searchString));
            }

            query = query.OrderByDescending(a => a.UpdatedDate);

            var totalAnswers = await query.CountAsync();
            var answers = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<Answer>
            {
                Items = answers,
                TotalPages = (int)Math.Ceiling(totalAnswers / (double)pageSize),
                CurrentPage = pageNumber
            };

            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Answer>> GetAnswer(int id)
        {
            var answer = await _context.Answers.FindAsync(id);

            if (answer == null)
            {
                return NotFound();
            }

            return answer;
        }

        [HttpPost]
        public async Task<ActionResult<Answer>> PostAnswer([FromForm] Answer answer, IFormFile attachment)
        {
            if (attachment != null)
            {
                using var ms = new MemoryStream();
                await attachment.CopyToAsync(ms);
                var imageArray = ms.ToArray();
                answer.Attachment = Convert.ToBase64String(imageArray);
            }

            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAnswer), new { id = answer.AnswerID }, answer);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnswer(int id, Answer answer)
        {
            if (id != answer.AnswerID)
            {
                return BadRequest();
            }

            _context.Entry(answer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnswerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool AnswerExists(int id)
        {
            return _context.Answers.Any(e => e.AnswerID == id);
        }
    }
}