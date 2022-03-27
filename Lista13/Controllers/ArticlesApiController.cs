using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Lista13.Data;
using Lista13.Models;
using Microsoft.AspNetCore.Authorization;

namespace Lista13.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/articles")]
    [ApiController]
    public class ArticlesApiController : ControllerBase
    {
        private readonly Lab13Context _context;

        public ArticlesApiController(Lab13Context context)
        {
            _context = context;
        }

        // GET: api/ArticlesApi
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticle()
        {
            return await _context.Article.ToListAsync();
        }

        // GET: api/ArticlesApi
        [AllowAnonymous]
        [HttpGet("page/{page}/{categoryId}/{elements}")]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticleByPage(int page = 0, int categoryId = -1, int elements = 0)
        {
            if(categoryId < 0)
                return await _context.Article.Include(ar => ar.Category)
                    .Skip(page * 5 + elements)
                    .Take(5 - elements)
                    .ToListAsync();
            else
                return await _context.Article.Include(ar => ar.Category)
                    .Where((ar) => ar.CategoryId == categoryId)
                    .Skip(page * 5 + elements)
                    .Take(5 - elements)
                    .ToListAsync();
        }

        // GET: api/ArticlesApi/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Article.FindAsync(id);

            if (article == null)
            {
                return NotFound();
            }

            return article;
        }

        // PUT: api/ArticlesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
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

        // POST: api/ArticlesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Article.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/ArticlesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Article.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Article.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.Id == id);
        }
    }
}
