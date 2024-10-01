using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    [Route("odata/Tags")]
    public class TagController : ODataController
    {
        private readonly ITagService tagService;

        public TagController(ITagService service)
        {
            this.tagService = service;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<Tag>> Get()
        {
            var tags = tagService.GetTags().AsQueryable();
            return Ok(tags);
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public ActionResult<Tag> Get([FromRoute] int id)
        {
            var tag = tagService.GetTagById(id);
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tagService.CreateTag(tag);
            return Created(tag);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] Tag tag)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tagService.UpdateTag(tag);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            tagService.RemoveTag(id);
            return NoContent();
        }
    }
}
