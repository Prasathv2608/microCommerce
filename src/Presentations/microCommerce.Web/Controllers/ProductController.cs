using microCommerce.Domain.Products;
using microCommerce.Mvc.Controllers;
using microCommerce.Mvc.HttpProviders;
using microCommerce.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace microCommerce.Web.Controllers
{
    public class ProductController : FrontBaseController
    {
        private readonly IHttpClient _httpClient;

        public ProductController(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _httpClient.GetAsync<IList<Product>>("http://localhost:51829/products/");

            var models = products.Select(p => new ProductViewModel
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.FullDescription,
                Price = p.Price,
                OldPrice = p.OldPrice
            }).ToList();

            return View(models);
        }

        public async Task<IActionResult> Detail(int Id)
        {
            var model = await _httpClient.GetAsync<ProductViewModel>("http://localhost:51829/products/detail/1");

            return View(model);
        }
    }
}