using microCommerce.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class BlogController : WebBaseController
    {
        public virtual IActionResult Index()
        {
            return View();
        }

        public virtual IActionResult Category(int categoryId)
        {
            return View();
        }

        public virtual IActionResult Detail(int postId)
        {
            return View();
        }
    }
}