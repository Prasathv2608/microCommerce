using microCommerce.Mvc.Controllers;
using microCommerce.Web.Models.Products;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class ProductController : FrontBaseController
    {
        public virtual IActionResult Detail(int productId)
        {
            var model = new ProductDetailViewModel();

            return View(model);
        }
    }
}