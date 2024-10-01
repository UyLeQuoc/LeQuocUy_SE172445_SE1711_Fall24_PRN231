using BusinessObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace OdataAPI.Controllers
{
    [Route("odata/Categories")]
    public class CategoryController : ODataController
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService service)
        {
            this.categoryService = service;
        }

        [EnableQuery]
        [HttpGet]
        public ActionResult<IQueryable<Category>> Get()
        {
            var categories = categoryService.GetCategories().AsQueryable();
            return Ok(categories);
        }

        [EnableQuery]
        [HttpGet("{id}")]
        public ActionResult<Category> Get([FromRoute] short id)
        {
            var category = categoryService.GetCategoryById(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            categoryService.CreateCategory(category);
            return Created(category);
        }

        [HttpPut("{id}")]
        public IActionResult Put([FromRoute] short id, [FromBody] Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            categoryService.UpdateCategory(category);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] short id)
        {
            categoryService.RemoveCategory(id);
            return NoContent();
        }
    }
}
