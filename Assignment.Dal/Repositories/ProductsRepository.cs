using Microsoft.Extensions.Logging;

namespace Assignment.Dal.Repositories
{
    public class ProductsRepository : Repository
    {
        private readonly SuperMartDbContext _context;
        private readonly ILogger<ProductsRepository> _logger;

        public ProductsRepository(SuperMartDbContext context, ILogger<ProductsRepository> logger) : base(context)
        {
            _context = context;
            _logger = logger;
        }
    }
}
