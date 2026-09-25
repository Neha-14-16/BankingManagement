using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class ForgotPasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}