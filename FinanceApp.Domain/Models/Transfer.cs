using System.ComponentModel.DataAnnotations;

namespace FinanceApp.Domain.Models
{
    /// <summary>
    /// Модель описывающая финансовый перевод между двумя счетами, включая его идентификатор, идентификаторы и навигационные свойства для счетов отправителя и получателя, сумму перевода, описание, дату и связанные транзакции. Переводы используются для перемещения средств между счетами и могут быть связаны с одной или несколькими транзакциями для учета в финансовых отчетах.
    /// </summary>
    public class Transfer
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        [Required]
        public Guid AccountFromId {  get; set; } 
        public Account AccountFrom { get; set; } = null!;

        [Required]
        public Guid AccountToId { get; set; }
        public Account AccountTo { get; set; } = null!;
        [Required]
        public decimal Amount { get; set; } = decimal.Zero;
        public string Description { get; set; } = string.Empty;
        public DateTime Date { get; set; } = DateTime.UtcNow;

        // Навигационные свойства
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();

    }
}