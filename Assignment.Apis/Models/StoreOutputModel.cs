using System;
using System.Collections.Generic;

namespace Assignment.Apis.Models
{
    public class StoreOutputModel
    {
        public string StoreName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public int Pin { get; set; }
        public DateTime JoinedOn { get; set; }
        public IEnumerable<ProductOutputModel> Products { get; set; }
    }
}
