using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class BankingTransactionDTO
    {
        public int TransactionId { get; set; }

        [Required]
        public int FromAccountId { get; set; }

        [Required]
        public int ToAccountId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [StringLength(50)]
        public string ReferenceNumber { get; set; }
    }
}