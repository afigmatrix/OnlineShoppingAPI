using System;

namespace OnlineShoppingAPI.Entites
{
    public class Log
    {
        public int Id { get; set; }
        public string UserGuid { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
