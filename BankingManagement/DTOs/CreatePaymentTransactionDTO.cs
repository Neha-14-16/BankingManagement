using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CreatePaymentTransactionDTO
    {
        [Required]
        public int AccountId { get; set; }

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "Amount must be greater than 0."
        )]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentType { get; set; }
    }
}