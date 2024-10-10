using DTO;
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
        public ActionResult<IEnumerable<NewsArticleDTO>> Get()
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
        public ActionResult<NewsArticleDTO> Get([FromRoute] string key)
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

        [HttpPost]
        public IActionResult Post([FromBody] NewsArticleDTO newsArticleDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                newsArticleService.CreateNewsArticle(newsArticleDTO);
                return Created(newsArticleDTO);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
                newsArticleService.UpdateNewsArticle(newsArticleDTO);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

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
