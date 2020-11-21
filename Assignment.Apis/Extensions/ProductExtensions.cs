using Assignment.Apis.Models;
using System;

namespace Assignment.Apis.Extensions
{
    public static class ProductExtensions
    {
        public static Dal.Entities.Product ToDalEntity(this ProductInputModel input)
        {
            return new Dal.Entities.Product
            {
                Name = input.Name,
                Description = input.Description,
                Category = input.Category,
                Price = input.Price,
                AddedOn = DateTime.Now
            };
        }

        public static ProductOutputModel ToOutputModel(this Dal.Entities.Product input)
        {
            return new ProductOutputModel
            {
                Name = input.Name,
                Description = input.Description,
                Category = input.Category,
                Price = input.Price,
                AddedOn = DateTime.Now,
                StoreName = input.Store?.StoreName
            };
        }
    }
}
