using OnlineShoppingAPI.Entites;

namespace OnlineShoppingAPI.Model.DTO
{
    public class ProductDTO
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public Category Category { get; set; }
    }

}
