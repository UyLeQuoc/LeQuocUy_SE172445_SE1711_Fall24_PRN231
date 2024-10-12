using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize("StaffOnly")]
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
        [Authorize("StaffOnly")]
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
        [Authorize("StaffOnly")]
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
        [Authorize("StaffOnly")]
        [HttpPut("/odata/Tags/{id}")]
        public IActionResult Put([FromRoute] int id, [FromBody] Tag tag)
        {
            try
            {
                if (id != tag.TagId)
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
        [Authorize("StaffOnly")]
        [HttpDelete("/odata/Tags/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            try
            {
                tagService.RemoveTag(id);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
