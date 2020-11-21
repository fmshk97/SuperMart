using System;

namespace Assignment.Dal.Repositories
{
    public class Repository : IDisposable
    {
        private readonly SuperMartDbContext _context;
        private bool _disposed;

        public Repository(SuperMartDbContext context)
        {
            _context = context;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                _context.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
