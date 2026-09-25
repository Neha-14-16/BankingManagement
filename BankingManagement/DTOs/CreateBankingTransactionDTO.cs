using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CreateBankingTransactionDTO
    {
        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
    }
}