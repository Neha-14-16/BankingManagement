using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingManagement.Models
{
    public class PaymentTransaction
    {
        [Key]
        [Column("PaymentTransactionId")]
        public int PaymentId { get; set; }

        [Required]
        public int AccountId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(30)]
        [Column("PaymentMethod")]
        public string PaymentType { get; set; }

        public DateTime PaymentDate { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        [Required]
        [StringLength(50)]
        public string ReferenceNumber { get; set; }

        public virtual Account Account { get; set; }
    }
}