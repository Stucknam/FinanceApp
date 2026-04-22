using System;
using System.Collections.Generic;
using System.Text;

namespace FinanceApp.Domain.Interfaces.Repositories
{
    /// <summary>
    /// Базовый интерфейс репозитория, предоставляющий стандартный набор
    /// операций для работы с сущностями доменной модели.
    /// </summary>
    /// <typeparam name="T">
    /// Тип сущности, с которой работает репозиторий. Должен быть ссылочным типом.
    /// </typeparam>
    public interface IRepository<T> where T : class
    {
        Task<T?> GetByIdAsync(Guid id);
        Task<List<T>> GetAllAsync();
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        IQueryable<T> Query();
        Task SaveChangesAsync();
    }
}
