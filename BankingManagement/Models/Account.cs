using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(30)]
        public string AccountType { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Balance { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}