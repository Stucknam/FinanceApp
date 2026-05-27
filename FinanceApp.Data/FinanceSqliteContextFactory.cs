using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System.IO;

namespace FinanceApp.Data
{
    public class FinanceSqliteDbContextFactory : IDesignTimeDbContextFactory<FinanceSqliteDbContext>
    {
        public FinanceSqliteDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FinanceSqliteDbContext>();

            var dbPath = Path.Combine(Directory.GetCurrentDirectory(), "finance.db");
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new FinanceSqliteDbContext(optionsBuilder.Options);
        }
    }
}

