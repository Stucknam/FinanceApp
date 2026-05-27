using Microsoft.EntityFrameworkCore;
using FinanceApp.Domain.Models;

namespace FinanceApp.Data
{
    public class FinanceSqliteDbContext : DbContext
    {
        public FinanceSqliteDbContext(DbContextOptions<FinanceSqliteDbContext> options)
            : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Transfer> Transfers => Set<Transfer>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceSqliteDbContext).Assembly);
        }
    }
}

