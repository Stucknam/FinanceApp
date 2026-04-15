using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        public Task<List<Category>> GetByTypeAsync(CategoryType type);
    }
}
