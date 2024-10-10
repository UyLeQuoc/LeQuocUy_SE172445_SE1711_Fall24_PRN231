using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    public class TagsController : ODataController
    {
        private readonly INewsArticleService _newsArticleService;

        public TagsController(INewsArticleService service)
        {
            _newsArticleService = service;
        }

        [EnableQuery]
        public ActionResult<List<Tag>> Get()
        {
            var tags = _newsArticleService.GetTags();
            return Ok(tags);
        }
    }
}
