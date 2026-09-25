using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class PaymentSuccessDTO
    {
        [Required]
        public int AccountId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string PaymentType { get; set; }

        [Required]
        public string RazorpayPaymentId { get; set; }

        [Required]
        public string RazorpayOrderId { get; set; }

        [Required]
        public string RazorpaySignature { get; set; }
    }
}