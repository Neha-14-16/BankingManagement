using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CreateAccountDTO
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(30)]
        public string AccountType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }
    }
}