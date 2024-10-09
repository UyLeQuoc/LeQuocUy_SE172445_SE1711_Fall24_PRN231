using BusinessObjects;
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
            this.categoryService = service;
        }

        [EnableQuery]
        public ActionResult<IQueryable<Category>> Get()
        {
            var categories = categoryService.GetCategories().AsQueryable();
            return Ok(categories);
        }

        [EnableQuery]
        public ActionResult<Category> Get([FromRoute] short key)
        {
            var category = categoryService.GetCategoryById(key);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }

        //[HttpPost]
        //public IActionResult Post([FromBody] Category category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    categoryService.CreateCategory(category);
        //    return Created(category);
        //}

        //[HttpPut]
        //public IActionResult Put([FromRoute] short id, [FromBody] Category category)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    categoryService.UpdateCategory(category);
        //    return NoContent();
        //}

        //[HttpDelete]
        //public IActionResult Delete([FromRoute] short id)
        //{
        //    categoryService.RemoveCategory(id);
        //    return NoContent();
        //}
    }
}
