using System.ComponentModel.DataAnnotations;

namespace BudgetApp.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        [Required]
        public required string Description { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        [Required]
        public required string Category { get; set; }
        public bool IsIncome { get; set; }
    }
}
