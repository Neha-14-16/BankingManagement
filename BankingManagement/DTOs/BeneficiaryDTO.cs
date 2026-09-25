using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class BeneficiaryDTO
    {
        public int BeneficiaryId { get; set; }

        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string BeneficiaryName { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(20)]
        public string IFSCCode { get; set; }

        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}