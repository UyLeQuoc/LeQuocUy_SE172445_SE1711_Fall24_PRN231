using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    public class TagsController : ODataController
    {
        private readonly ITagService tagService;

        public TagsController(ITagService service)
        {
            tagService = service;
        }

        [EnableQuery]
        public ActionResult<List<Tag>> Get()
        {
            try
            {
                var tags = tagService.GetTags();
                return Ok(tags);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [EnableQuery]
        public ActionResult<Tag> Get([FromRoute] int key)
        {
            try
            {
                var tag = tagService.GetTagById(key);
                if (tag == null)
                {
                    return NotFound();
                }
                return Ok(tag);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] Tag tag)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                tagService.CreateTag(tag);
                return Created(tag);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("/odata/Tags/{key}")]
        public IActionResult Put([FromRoute] int key, [FromBody] Tag tag)
        {
            try
            {
                if (key != tag.TagId)
                {
                    return BadRequest("Tag ID mismatch.");
                }

                tagService.UpdateTag(tag);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("/odata/Tags/{key}")]
        public IActionResult Delete([FromRoute] int key)
        {
            try
            {
                tagService.RemoveTag(key);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
