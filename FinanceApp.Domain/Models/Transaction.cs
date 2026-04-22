using FinanceApp.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Domain.Models
{
    /// <summary>
    /// Модель описывающая финансовую транзакцию, включая её идентификатор, тип (доход, расход или перевод), сумму, дату, описание и связи с аккаунтом и категорией.
    /// </summary>
    public record class Transaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public CategoryType Type { get; set; } // income | expence | transfer
        [Required]
        public Guid AccountId { get; set; }
        public Account Account { get; set; } = null!;

        public Guid? CategoryId { get; set; }
        public Category? Category { get; set; }

        public decimal Amount { get; set; } = decimal.Zero;
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public Guid? TransferId { get; set; }
        public Transfer? Transfer { get; set; }

    }
}