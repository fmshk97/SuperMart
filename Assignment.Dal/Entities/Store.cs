using System;
using System.Collections.Generic;

namespace Assignment.Dal.Entities
{
    public class Store
    {
        public int Id { get; set; }
        public string StoreName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTime JoinedOn { get; set; }
        public int Pin { get; set; }
        public List<Product> Products { get; set; }
    }
}
