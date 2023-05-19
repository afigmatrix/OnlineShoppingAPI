using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OnlineShoppingAPI.Entites
{
    public class Region
    {
        public int Id { get; set; }
        [Required, StringLength(20, ErrorMessage = "Region length is not valid")]
        public string RegionName { get; set; }
        public ICollection<Customer>  Customers { get; set; }
    }
}
