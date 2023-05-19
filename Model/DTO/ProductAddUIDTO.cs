using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingAPI.Model.DTO
{
    public class ProductAddUIDTO
    {
        [MinLength(4,ErrorMessage ="Product length is not valid")]
        public string Name { get; set; }
        public decimal Price { get; set; }
        [Range(1,200,ErrorMessage ="Quantity length is not valid")]
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public string ColorInfo { get; set; }
        public IFormFile File { get; set; }
    }

}
