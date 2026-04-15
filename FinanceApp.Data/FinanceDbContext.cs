using Microsoft.EntityFrameworkCore;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore.Internal;

namespace FinanceApp.Data
{
    public class FinanceDbContext: DbContext
    {
        public FinanceDbContext(DbContextOptions<FinanceDbContext> options) : base(options) { }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Transfer> Transfers => Set<Transfer>();
        public DbSet<Transaction> Transactions => Set<Transaction>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(FinanceDbContext).Assembly);
        }
    }
}
