using FinanceApp.Application.Services;
using FinanceApp.Data;
using FinanceApp.Data.Repositories;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Interfaces.Srevices;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace FinanceApp.Api.DependencyInjection
{
    public static class ApiServiceRegistration
    {
        public static IServiceCollection AddData(this IServiceCollection services, IConfiguration config)
        {

            var connectionString = config.GetConnectionString("DefaultConnection");

            services.AddDbContext<FinanceDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();


            return services;
        }

        public static IServiceCollection AddDataSqlite(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<FinanceDbContext>(options =>
            {
                options.UseSqlite("Data Source=finance.db");
            });

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITransactionRepository, TransactionRepository>();
            services.AddScoped<ITransferRepository, TransferRepository>();


            return services;
        }

        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IStatisticsService, StatisticsService>();

            return services;
        }
    }
}
