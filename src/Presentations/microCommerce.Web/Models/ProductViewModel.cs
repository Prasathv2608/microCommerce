using microCommerce.Mvc.Models;

namespace microCommerce.Web.Models
{
    public class ProductViewModel : BaseEntityModel
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }
    }
}