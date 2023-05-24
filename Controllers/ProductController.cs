using AutoMapper;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using OnlineShoppingAPI.ActionFIlters;
using OnlineShoppingAPI.DAL;
using OnlineShoppingAPI.Entites;
using OnlineShoppingAPI.Model.DTO;
using OnlineShoppingAPI.Service.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
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
        public IWebHostEnvironment _env { get; }
        //public ILogger<ProductController> _productLogger { get; }

        public ProductController(IMapper mapper,
            IGenericRepository<Product> productRepo,
            IGenericRepository<Category> categoryRepo,
            IProductRepository baseProdRepo,
            //ILogger<ProductController> productLogger,
            IWebHostEnvironment env)
        {
            _mapper = mapper;
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _baseProdRepo = baseProdRepo;
            //_productLogger = productLogger;
            _env = env;
        }

        //[ServiceFilter(typeof(CustomActionBaseActionFilter))]
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            //HttpContext.Response.Headers.Add("MyCustomHeaderKey", "asjkdhaksdhgkuashdbkasugdhb");

            try
            {
                //throw new NullReferenceException("Object is null");
                Serilog.Log.Information("Custom Log Logged");
                //_productLogger.LogInformation("Custom Log Logged");
                var myList = _mapper.Map<List<ProductNPDTO>>(await _productRepo.GetAll());
                return StatusCode(StatusCodes.Status200OK, myList);
            }
            catch (Exception ex)
            {
                //_productLogger.LogError(ex, "Error occured when getting datas");
                Serilog.Log.Error(ex, "Error occured when getting datas");
                return BadRequest();
            }
         
        }

        [HttpGet]
        public async Task<IActionResult> GetProductsWithDeleted()
        {
            //var myData = HttpContext.Request.Headers.TryGetValue("MyCustomHeaderKey",out StringValues strings);
            var myList = _mapper.Map<List<ProductNPDTO>>(await _productRepo.GetAllTable().IgnoreQueryFilters().ToListAsync());
            return StatusCode(StatusCodes.Status200OK, myList);
        }

        [HttpGet]
        public async Task<IActionResult> GetStaticProductsCount(bool isDeleted)
        {
            var myList = _mapper.Map<List<ProductNPDTO>>(await _productRepo.GetAllTable().IgnoreQueryFilters().ToListAsync());

            var result = myList.Where(m => m.Price > 10).Select(m => m.Price);
            myList.Add(new ProductNPDTO { Name = "Afiq", Price = 3400 });

            return StatusCode(StatusCodes.Status200OK, result.Count());

        }

        [HttpGet]
        public async Task<IActionResult> GetProductById(int id)
        {
            ProductDTO productEntity = _mapper.Map<ProductDTO>(await _productRepo.GetAllTable().Include(m => m.Category).FirstOrDefaultAsync(m => m.Id == id));
            string productCategoryName = productEntity.Category.Name;
            return StatusCode(StatusCodes.Status200OK, productEntity);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddProduct([FromForm] ProductAddUIDTO addProduct)
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
            var fileUniqueName = string.Concat(Guid.NewGuid().ToString(), addProduct.File.FileName);
            var folderPath = Path.Combine(_env.WebRootPath, "Product");
            var fullPath = Path.Combine(folderPath, fileUniqueName);
            Directory.CreateDirectory(folderPath);
            using (FileStream fs = new FileStream(fullPath, FileMode.Create))
            {
                addProduct.File.CopyTo(fs);
            }
            myAddProductEntity.FilePath = fileUniqueName;

            await _productRepo.AddAndCommit(myAddProductEntity);
            return StatusCode(StatusCodes.Status201Created, myAddProductEntity.Name);
        }


        [HttpGet]
        public async Task<IActionResult> CalculateProductPrice(int id)
        {
            var result = _baseProdRepo.CalculateProductPrice(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetProductFile(int id)
        {
            var result = await _baseProdRepo.GetProductFileBase64(id);
            return StatusCode(StatusCodes.Status200OK, result);
        }
    }
}
