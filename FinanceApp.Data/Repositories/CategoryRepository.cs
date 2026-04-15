using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Interfaces.Repositories;
using FinanceApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Data.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(FinanceDbContext context) : base(context) { }

        public async Task<List<Category>> GetByTypeAsync(CategoryType type)
        {
            return await _dbSet
                .Where(c => c.Type == type)
                .ToListAsync();
        }
    }
}
