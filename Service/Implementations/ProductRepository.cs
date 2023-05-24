using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Service.Abstractions;
using System.IO;
using System;
using Microsoft.AspNetCore.Hosting;
using System.Threading.Tasks;

namespace OnlineShoppingAPI.Service.Implementations
{
    public class ProductRepository : IProductRepository
    {

        public IGenericRepository<Product> _productRepo { get; }
        public IWebHostEnvironment _env { get; }

        public ProductRepository(IGenericRepository<Product> ProductRepo, IWebHostEnvironment env)
        {
            _productRepo = ProductRepo;
            _env = env;
        }
        public decimal CalculateProductPrice(int id)
        {
            var productEntity = _productRepo.GetById(id);
            decimal result = productEntity.Price - 4m;
            return result;
        }

        public async Task<string> GetProductFileBase64(int id)
        {
            var product = _productRepo.GetById(id);
            if (string.IsNullOrEmpty(product.FilePath))
            {
                return string.Empty;
            }

            var folderPath = Path.Combine(_env.WebRootPath, "Product");
            var fullPath = Path.Combine(folderPath, product.FilePath);
            byte[] fileBytes = default(byte[]);
            using (var fs = new FileStream(fullPath, FileMode.Open))
            {
                fileBytes = new byte[fs.Length];
                await fs.ReadAsync(fileBytes, 0, fileBytes.Length);
            }
            string fileBase64 = Convert.ToBase64String(fileBytes);
            return fileBase64;
        }
    }
}
