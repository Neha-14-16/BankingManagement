using System.ComponentModel.DataAnnotations;

namespace BankingManagement.DTOs
{
    public class CustomerBeneficiaryDTO
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
        public int BankBranchId { get; set; }

        public string BankName { get; set; }

        public string BranchName { get; set; }

        public string IFSCCode { get; set; }
    }
}