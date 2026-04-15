using FinanceApp.Domain.Enums;
using FinanceApp.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Srevices
{
    public interface ICategoryService
    {
        public Task<List<Category>> GetAllAsync();
        public Task<Category?> GetByIdAsync(Guid id);

        public Task<List<Category>?> GetByTypeAsync(CategoryType type);

        public Task<Category> CreateAsync(Category category);
        public Task UpdateAsync(Category category);
        public Task DeleteAsync(Guid id);

    }
}
