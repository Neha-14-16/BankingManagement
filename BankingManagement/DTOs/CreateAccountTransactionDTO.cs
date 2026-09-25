using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CreateAccountTransactionDTO
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; }

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "Amount must be greater than 0."
        )]
        public decimal Amount { get; set; }
    }
}