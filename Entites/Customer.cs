using System.Collections;
using System.Collections.Generic;

namespace OnlineShoppingAPI.Entites
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public int Balans { get; set; }

        public ICollection<Region> Regions { get; set; }
    }
}
