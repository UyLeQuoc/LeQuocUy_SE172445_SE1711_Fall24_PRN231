using BusinessObjects;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleService newsArticleService;

        public NewsArticlesController(INewsArticleService service)
        {
            newsArticleService = service;
        }

        [EnableQuery]
        public ActionResult<IEnumerable<NewsArticle>> Get()
        {
            try
            {
                var articles = newsArticleService.GetNewsArticles();
                return Ok(articles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [EnableQuery]
        public ActionResult<NewsArticle> Get([FromRoute] string key)
        {
            try
            {
                var article = newsArticleService.GetNewsArticleById(key);
                if (article == null)
                {
                    return NotFound();
                }
                return Ok(article);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize("StaffOnly")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] NewsArticleDTO newsArticle)
        {
            try
            {
                var accountId = User.FindFirst("AccountId");
                if (accountId == null)
                {
                    throw new Exception("AccountId claim is missing in the token.");
                }

                short createdById = short.Parse(accountId.Value);
                newsArticle.CreatedById = createdById;
                newsArticleService.CreateNewsArticle(newsArticle);
                return Ok(newsArticle);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize("StaffOnly")]
        [HttpPut("/odata/NewsArticles/{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] NewsArticleDTO newsArticleDTO)
        {
            try
            {
                var existingArticle = newsArticleService.GetNewsArticleById(id);
                if (existingArticle == null)
                {
                    return NotFound($"NewsArticle with ID {id} not found.");
                }

                newsArticleDTO.NewsArticleId = id;
                var accountId = User.FindFirst("AccountId");
                if (accountId == null)
                {
                    throw new Exception("AccountId claim is missing in the token.");
                }

                short updatedById = short.Parse(accountId.Value);
                newsArticleDTO.UpdatedById = updatedById;

                newsArticleService.UpdateNewsArticle(newsArticleDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Authorize("StaffOnly")]
        [HttpDelete("/odata/NewsArticles/{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            try
            {
                var existingArticle = newsArticleService.GetNewsArticleById(id);
                if (existingArticle == null)
                {
                    return NotFound($"NewsArticle with ID {id} not found.");
                }

                newsArticleService.RemoveNewsArticle(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
