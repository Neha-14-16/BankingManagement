using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class AccountDTO
    {
        public int AccountId { get; set; }

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
    }
}