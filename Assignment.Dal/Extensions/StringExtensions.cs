using Microsoft.EntityFrameworkCore;

namespace Assignment.Dal.Extensions
{
    public static class StringExtensions
    {
        public static DbContextOptions<DbContext> StringToContextOptions(this string connectionString)
        {
            return new DbContextOptionsBuilder<DbContext>().UseMySQL(connectionString).Options;
        }
    }
}
