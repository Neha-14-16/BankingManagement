using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class AccountTransactionDTO
    {
        public int AccountTransactionId { get; set; }

        [Required]
        public int AccountId { get; set; }

        public string AccountNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public string ReferenceNumber { get; set; }
    }
}