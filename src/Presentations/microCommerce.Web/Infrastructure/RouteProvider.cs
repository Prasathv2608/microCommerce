using microCommerce.Mvc.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace microCommerce.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {
            routeBuilder.MapRoute(
                name: "Login",
                template: "Customer/Login",
                defaults: new { controller = "Customer", action = "Login" });

            routeBuilder.MapRoute(
                name: "Register",
                template: "Customer/Register",
                defaults: new { controller = "Customer", action = "Register" });

            routeBuilder.MapRoute(
                name: "Product",
                template: "Product",
                defaults: new { controller = "Product", action = "Index" });

            routeBuilder.MapRoute(
                name: "ProductDetail",
                template: "Product/Detail/{Id:int}",
                defaults: new { controller = "Product", action = "Detail" });

            routeBuilder.MapRoute(
                name: "HomePage",
                template: "");
        }

        public int Priority
        {
            get
            {
                return int.MaxValue;
            }
        }
    }
}