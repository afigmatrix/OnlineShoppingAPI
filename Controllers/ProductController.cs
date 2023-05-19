using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingAPI.ActionFIlters;
using OnlineShoppingAPI.DAL;
using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Model.DTO;
using OnlineShoppingAPI.Service.Abstractions;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace OnlineShoppingAPI.Controllers
{
    //[ServiceFilter(typeof(CustomControllerActionFilter))]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        public IMapper _mapper { get; }
        public IGenericRepository<Product> _productRepo { get; }
        public IGenericRepository<Category> _categoryRepo { get; }
        public IProductRepository _baseProdRepo { get; }

        public ProductController(IMapper mapper,
            IGenericRepository<Product> productRepo,
            IGenericRepository<Category> categoryRepo,
            IProductRepository baseProdRepo)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _baseProdRepo = baseProdRepo;
        }

        //[ServiceFilter(typeof(CustomActionBaseActionFilter))]
        [HttpGet]
        public async Task<IActionResult> GetProducts(IFormFile file)
        {
            var myList = _mapper.Map<List<ProductNPDTO>>(await _productRepo.GetAll());
            return StatusCode(StatusCodes.Status200OK, myList);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsWithDeleted()
        {
            var myList = _mapper.Map<List<ProductNPDTO>>(await _productRepo.GetAllTable().IgnoreQueryFilters().ToListAsync());
            return StatusCode(StatusCodes.Status200OK, myList);
        }

        [HttpGet]
        public async Task<IActionResult> GetStaticProductsCount(bool isDeleted)
        {
            var myList = _mapper.Map<List<ProductNPDTO>>(await _productRepo.GetAllTable().IgnoreQueryFilters().ToListAsync());

            var result = myList.Where(m => m.Price > 10).Select(m=>m.Price);
            myList.Add(new ProductNPDTO { Name = "Afiq", Price = 3400 });

            return StatusCode(StatusCodes.Status200OK, result.Count());

        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            ProductDTO productEntity = _mapper.Map<ProductDTO>(await _productRepo.GetAllTable().Include(m=>m.Category).FirstOrDefaultAsync(m => m.Id == id));
            string productCategoryName = productEntity.Category.Name;
            return StatusCode(StatusCodes.Status200OK, productEntity);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct(ProductAddUIDTO addProduct)
        {
            var loginnedUser = User;
            var errorList = new List<string>();

            if (addProduct.CategoryId == 0)
            {
                ModelState.AddModelError("categoryIdIsNotValid", "category Id Is Not Valid");
            }

            if (!ModelState.IsValid)
            {
                foreach (var value in ModelState.Values)
                {
                    foreach (var error in value.Errors)
                    {
                        errorList.Add(error.ErrorMessage);
                    }
                }
            }

            if (errorList.Any())
            {
                return StatusCode(StatusCodes.Status405MethodNotAllowed, errorList);
            }
            var myAddProductEntity = _mapper.Map<Product>(addProduct);

            //await _context.Products.AddAsync(myAddProductEntity);
            //await _context.SaveChangesAsync();
            await _productRepo.AddAndCommit(myAddProductEntity);
            return StatusCode(StatusCodes.Status201Created, myAddProductEntity.Name);
        }


        [HttpGet]
        public async Task<IActionResult> CalculateProductPrice(int id)
        {
            var result = _baseProdRepo.CalculateProductPrice(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
