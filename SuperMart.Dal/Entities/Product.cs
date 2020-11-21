using System;

namespace SuperMart.Dal.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public DateTime AddedOn { get; set; }
        public Store Store { get; set; }
    }
}
