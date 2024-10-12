using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly ICategoryService categoryService;

        public CategoriesController(ICategoryService service)
        {
            categoryService = service;
        }

        [EnableQuery]
        [Authorize("StaffOnly")]
        public ActionResult<IEnumerable<Category>> Get()
        {
            var categories = categoryService.GetCategories();
            return Ok(categories);
        }

        [EnableQuery]
        [Authorize("StaffOnly")]
        public ActionResult<Category> Get([FromRoute] short key)
        {
            var category = categoryService.GetCategoryById(key);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [Authorize("StaffOnly")]
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                categoryService.CreateCategory(category);
                return Created(category);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize("StaffOnly")]
        [HttpPut("/odata/Categories/{id}")]
        public IActionResult Put([FromRoute] short id, [FromBody] Category category)
        {
            try
            {
                categoryService.UpdateCategory(category);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize("StaffOnly")]
        [HttpDelete("/odata/Categories/{id}")]
        public IActionResult Delete([FromRoute] short id)
        {
            try
            {
                categoryService.RemoveCategory(id);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
