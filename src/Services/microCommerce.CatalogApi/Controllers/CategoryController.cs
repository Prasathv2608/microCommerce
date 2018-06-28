using microCommerce.Dapper;
using microCommerce.Domain.Categories;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace microCommerce.CatalogApi.Controllers
{
    public class CategoryController : ServiceBaseController
    {
        private readonly IDataContext _dataContext;

        public CategoryController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("/categories")]
        public virtual async Task<IActionResult> SearchCategories(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var products = await _dataContext.QueryAsync<Category>("SELECT * FROM Category");

            return Json(products);
        }

        [HttpGet("/categories/{Id:int}")]
        public virtual async Task<IActionResult> FindCategoryById(int Id)
        {
            if (Id == 0)
                return BadRequest();

            var product = await _dataContext.FirstAsync<Category>("SELECT * FROM Category WHERE Id = @Id LIMIT 1", new { Id });
            if (product == null)
                return NotFound();

            return Json(product);
        }

        [HttpGet("/categories/homepage")]
        public virtual async Task<IActionResult> HomeCategories()
        {
            var products = await _dataContext.QueryAsync<Category>("SELECT c.* FROM Category c Inner Join HomeCategory hc on c.Id = hc.ProductId");

            return Json(products);
        }
    }
}