using Assignment.Apis.Models;
using System;

namespace Assignment.Apis.Extensions
{
    public static class StoreInputModelExtensions
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
    }
}
