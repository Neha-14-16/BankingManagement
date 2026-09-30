using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}