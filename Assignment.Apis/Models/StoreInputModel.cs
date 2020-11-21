using System.Collections.Generic;

namespace Assignment.Apis.Models
{
    public class StoreInputModel
    {
        public string StoreName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Pin { get; set; }
    }
}
