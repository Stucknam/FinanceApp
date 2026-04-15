using FinanceApp.Domain.Defaults;
using FinanceApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;


namespace FinanceApp.Domain.Models
{
    /// <summary>
    /// Модель описывающая категорию финансовых транзакций, включая её идентификатор, название, иконку, цвет и тип (доход или расход). Категории используются для классификации транзакций и могут быть связаны с несколькими транзакциями.
    /// </summary>
    public record class Category
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = "Новая категория";
        public string IconId { get; set; } = string.Empty;
        public string? ColorId { get; set; } = DefaultColors.DefaultColor;
        [Required]
        public CategoryType Type { get; set; } // income | expence

        // Навигационные свойства
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
