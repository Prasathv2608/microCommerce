using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Mvc.Controllers
{
    public abstract class WebBaseController : BaseController
    {
        public virtual IActionResult HomePage()
        {
            return RedirectToRoute("HomePage");
        }
    }
}