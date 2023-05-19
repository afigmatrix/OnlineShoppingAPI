using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Service.Abstractions;

namespace OnlineShoppingAPI.Service.Implementations
{
    public class ProductRepository : IProductRepository
    {
        public IGenericRepository<Product> _productRepo { get; }
        public ProductRepository(IGenericRepository<Product> ProductRepo)
        {
            _productRepo = ProductRepo;
        }
        public decimal CalculateProductPrice(int id)
        {
            var productEntity = _productRepo.GetById(id);
            decimal result = productEntity.Price - 5m;
            return result;
        }
    }
}
