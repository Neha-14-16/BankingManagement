using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(15)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }



        [StringLength(250)]
        public string Address { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual User User { get; set; }
    }
}