using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.Models
{
    public class AccountTransaction
    {
        [Key]
        public int AccountTransactionId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)]
        public string TransactionType { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public virtual Account Account { get; set; }
    }
}