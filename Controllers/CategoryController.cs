using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingAPI.DAL;
using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Model.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineShoppingAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        public OnlineShoppingDBContext _context { get; }
        public CategoryController(OnlineShoppingDBContext dBContext)
        {
            _context = dBContext;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetCategorys()
        {
            List<Category> categoryList = await _context.Categories.ToListAsync();
            return StatusCode(StatusCodes.Status200OK, categoryList);
        }

        [HttpGet]
        public async Task<IActionResult> GetCategoryById(int id, bool withProducts)
        {
            Category category = await _context.Categories
                .Where(m => m.Id == id)
                .Include(m => m.Products)
                .FirstOrDefaultAsync();

            //if (withProducts)
            //{
            //    foreach (var product in category.Products)//SQL Query Generate
            //    {

            //    }
            //}

            return StatusCode(StatusCodes.Status200OK, category.Name);
        }


        [HttpPost]
        public async Task<IActionResult> AddCategory(CategoryAddUIDTO addCategory)
        {

            Category myAddCategoryEntity = new Category()
            {
                Name = addCategory.Name
            };

            var Products = new List<Product>();

            foreach (var product in addCategory.Products)
            {
                Products.Add(new Product
                {
                    CategoryId = myAddCategoryEntity.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = product.Quantity
                });
            }

            myAddCategoryEntity.Products = Products;

            await _context.Categories.AddAsync(myAddCategoryEntity);
            await _context.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created, addCategory.Name);
        }

        
    }
}
