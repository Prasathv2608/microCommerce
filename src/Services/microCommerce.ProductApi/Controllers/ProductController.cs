using microCommerce.Dapper;
using microCommerce.Domain.Products;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.ProductApi.Controllers
{
    public class ProductController : ServiceBaseController
    {
        private readonly IDataContext _dataContext;

        public ProductController(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpGet("/products")]
        public virtual async Task<IActionResult> ProductSearch(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var products = await _dataContext.QueryAsync<Product>("SELECT * FROM Product");

            return Json(products);
        }

        [HttpGet("/products/{categoryId:int}")]
        public virtual async Task<IActionResult> CategoryProducts(int categoryId)
        {
            return await Task.FromResult(Json(null));
        }

        [HttpGet("/products/homepage")]
        public virtual async Task<IActionResult> HomePageProducts()
        {
            return await Task.FromResult(Json(null));
        }

        [HttpGet("/products/detail/{Id:int}")]
        public virtual async Task<IActionResult> ProductDetail(int Id)
        {
            return await Task.FromResult(Json(new { Id = 1, Name = "Samsung OLed TV", Price = 1489.90m, OldPrice = 1899.90m }));
        }

        [HttpGet("/products/related/{Id:int}")]
        public virtual async Task<IActionResult> RelatedProducts(int Id)
        {
            return await Task.FromResult(Json(null));
        }
    }
}