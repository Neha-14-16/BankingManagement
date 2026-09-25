using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CustomerTransferDTO
    {
        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int BeneficiaryId { get; set; }

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "Amount must be greater than 0."
        )]
        public decimal Amount { get; set; }
    }
}