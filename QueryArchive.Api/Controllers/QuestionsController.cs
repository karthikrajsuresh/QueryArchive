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
    public class QuestionsController : ControllerBase
    {
        private readonly QueryArchiveDbContext _context;

        public QuestionsController(QueryArchiveDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<Question>>> GetQuestions(string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Questions.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(q => q.Title.Contains(searchString) || q.Content.Contains(searchString));
            }

            query = query.OrderByDescending(q => q.UpdatedDate);

            var totalQuestions = await query.CountAsync();
            var questions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<Question>
            {
                Items = questions,
                TotalPages = (int)Math.Ceiling(totalQuestions / (double)pageSize),
                CurrentPage = pageNumber
            };

            return Ok(pagedResult);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Question>> GetQuestion(int id)
        {
            var question = await _context.Questions.FindAsync(id);

            if (question == null)
            {
                return NotFound();
            }

            return question;
        }

        [HttpGet("bytopic/{topicId}")]
        public async Task<ActionResult<PagedResult<Question>>> GetQuestionsByTopic(int topicId, string searchString, int pageNumber = 1, int pageSize = 10)
        {
            var query = _context.Questions.Where(q => q.TopicID == topicId);

            if (!string.IsNullOrEmpty(searchString))
            {
                query = query.Where(q => q.Title.Contains(searchString) || q.Content.Contains(searchString));
            }

            query = query.OrderByDescending(q => q.UpdatedDate);

            var totalQuestions = await query.CountAsync();
            var questions = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var pagedResult = new PagedResult<Question>
            {
                Items = questions,
                TotalPages = (int)Math.Ceiling(totalQuestions / (double)pageSize),
                CurrentPage = pageNumber
            };

            return Ok(pagedResult);
        }

        [HttpPost]
        public async Task<ActionResult<Question>> PostQuestion([FromForm] Question question, IFormFile attachment)
        {
            if (attachment != null)
            {
                using var ms = new MemoryStream();
                await attachment.CopyToAsync(ms);
                var imageArray = ms.ToArray();
                question.Attachment = Convert.ToBase64String(imageArray);
            }

            _context.Questions.Add(question);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetQuestion), new { id = question.QuestionID }, question);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutQuestion(int id, Question question)
        {
            if (id != question.QuestionID)
            {
                return BadRequest();
            }

            _context.Entry(question).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QuestionExists(id))
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

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestionID == id);
        }
    }
}
