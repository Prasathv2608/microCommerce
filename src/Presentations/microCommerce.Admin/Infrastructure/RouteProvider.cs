using microCommerce.Mvc.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace microCommerce.Admin.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public void RegisterRoutes(IRouteBuilder routeBuilder)
        {

        }

        public int Priority => int.MaxValue;
    }
}