using microCommerce.Dapper;
using microCommerce.Domain.Products;
using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;
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
        public virtual async Task<IActionResult> SearchProducts(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var products = await _dataContext.QueryAsync<Product>("SELECT * FROM Product");

            return Json(products);
        }

        [HttpGet("/products/{Id:int}")]
        public virtual async Task<IActionResult> FindProductById(int Id)
        {
            if (Id == 0)
                return BadRequest();

            var product = await _dataContext.FirstAsync<Product>("SELECT * FROM Product WHERE Id = @Id LIMIT 1", new { Id });
            if (product == null)
                return NotFound();

            return Json(product);
        }

        [HttpGet("/products/homepage")]
        public virtual async Task<IActionResult> HomeProducts()
        {
            var products = await _dataContext.QueryAsync<Product>("SELECT p.* FROM Product p Inner Join HomeProduct hp on p.Id = hp.ProductId");

            return Json(products);
        }

        [HttpGet("/products/related/{Id:int}")]
        public virtual async Task<IActionResult> RelatedProducts(int Id)
        {
            if (Id == 0)
                return BadRequest();

            var relatedProducts = await _dataContext.QueryAsync<Product>("SELECT p.* FROM Product p Inner Join RelatedProduct rp on p.Id = rp.ProductId WHERE p.Id = @Id", new { Id });

            return Json(relatedProducts);
        }
    }
}