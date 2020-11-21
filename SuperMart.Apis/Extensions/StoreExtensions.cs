using SuperMart.Apis.Models;
using System;
using System.Linq;

namespace SuperMart.Apis.Extensions
{
    public static class StoreExtensions
    {
        public static Dal.Entities.Store ToDalEntity(this StoreInputModel input)
        {
            return new Dal.Entities.Store
            {
                StoreName = input.StoreName,
                City = input.City,
                Country = input.Country,
                JoinedOn = DateTime.Now,
                Pin = input.Pin
            };
        }

        public static StoreOutputModel ToOutputModel(this Dal.Entities.Store input)
        {
            return new StoreOutputModel
            {
                StoreName = input.StoreName,
                City = input.City,
                Country = input.Country,
                JoinedOn = DateTime.Now,
                Pin = input.Pin,
                Products = input.Products?.Select(product => product.ToOutputModel())
            };
        }
    }
}
