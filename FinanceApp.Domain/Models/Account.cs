using System.ComponentModel.DataAnnotations;
using FinanceApp.Domain.Defaults;

namespace FinanceApp.Domain.Models
{
    /// <summary>
    /// Модель описывающая финансовый счёт пользователя, включая его идентификатор, название, баланс, иконку, цвет и описание.
    /// </summary>
    public record class Account
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public string Name { get; set; } = "Новый счёт";
        public decimal Amount { get; set; } = decimal.Zero;
        public string? IconId { get; set; } = string.Empty; // Default icon ID
        public string Color { get; set; } = DefaultColors.DefaultColor; // Default color
        public string Description { get; set; } = string.Empty;

        // Навигационные свойства

        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

        public ICollection<Transfer> TransfersFrom { get; set; } = new List<Transfer>();
        public ICollection<Transfer> TransfersTo { get; set; } = new List<Transfer>();

    }
}
