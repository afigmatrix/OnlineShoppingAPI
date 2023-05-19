using OnlineShoppingAPI.Entites;
using System.Collections.Generic;

namespace OnlineShoppingAPI.Model.DTO
{
    public class CategoryAddUIDTO
    {
        public string Name { get; set; }
        public List<ProductAddUIDTO> Products { get; set; }
    }

}
