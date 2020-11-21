using System;

namespace SuperMart.Apis.Models
{
    public class ProductOutputModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string StoreName { get; set; }
        public DateTime AddedOn { get; set; }
    }
}
