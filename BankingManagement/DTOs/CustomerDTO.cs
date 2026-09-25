using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CustomerDTO
    {
        public int CustomerId { get; set; }

        public int UserId { get; set; }


        [StringLength(50)]
        public string Username { get; set; }

    
        [DataType(DataType.Password)]
        public string Password { get; set; }

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
    }
}