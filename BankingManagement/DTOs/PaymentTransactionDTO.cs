using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class PaymentTransactionDTO
    {
        public int PaymentId { get; set; }

        [Required]
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        [Range(
            0.01,
            double.MaxValue,
            ErrorMessage = "Amount must be greater than 0."
        )]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(30)]
        public string PaymentType { get; set; }

        public DateTime PaymentDate { get; set; }

        public string Status { get; set; }

        public string ReferenceNumber { get; set; }
    }
}