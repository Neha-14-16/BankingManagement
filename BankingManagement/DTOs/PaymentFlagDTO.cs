using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class PaymentFlagDTO
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Required]
        public string PaymentFlag { get; set; }

        public string RazorpayOrderId { get; set; }
    }
}