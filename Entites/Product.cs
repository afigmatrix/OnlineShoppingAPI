using Newtonsoft.Json;

namespace OnlineShoppingAPI.Entites
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Color { get; set; }
        public bool IsDeleted { get; set; }
        public string FilePath { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }

  
}
