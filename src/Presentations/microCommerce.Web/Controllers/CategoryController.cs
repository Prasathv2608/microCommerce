﻿using microCommerce.Mvc.Controllers;
using microCommerce.Web.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace microCommerce.Web.Controllers
{
    public class CategoryController : FrontBaseController
    {
        public virtual IActionResult Detail(int categoryId, CategoryFilterContext filterContext)
        {
            return View();
        }
    }
}