using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    [Route("odata/NewsArticles")]
    public class NewsArticleController : ODataController
    {
        private readonly INewsArticleService newsArticleService;

        public NewsArticleController(INewsArticleService service)
        {
            this.newsArticleService = service;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<NewsArticle>> Get()
        {
            var newsArticles = newsArticleService.GetNewsArticles().AsQueryable();
            return Ok(newsArticles);
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public ActionResult<NewsArticle> Get([FromRoute] string id)
        {
            var article = newsArticleService.GetNewsArticleById(id);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpPost]
        public IActionResult Post([FromBody] NewsArticle newsArticle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            newsArticleService.CreateNewsArticle(newsArticle);
            return Created(newsArticle);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] string id, [FromBody] NewsArticle newsArticle)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            newsArticleService.UpdateNewsArticle(newsArticle);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            newsArticleService.RemoveNewsArticle(id);
            return NoContent();
        }
    }
}
