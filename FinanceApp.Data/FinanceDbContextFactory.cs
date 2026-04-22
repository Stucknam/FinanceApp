using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FinanceApp.Data
{


    namespace FinanceApp.Data
    {
        public class FinanceDbContextFactory : IDesignTimeDbContextFactory<FinanceDbContext>
        {
            public FinanceDbContext CreateDbContext(string[] args)
            {
                var optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();

                // Укажи свой connection string
                optionsBuilder.UseNpgsql("Host=localhost;Database=finance;Username=postgres;Password=1111");

                return new FinanceDbContext(optionsBuilder.Options);
            }
        }
    }

}
