using System;
using System.ComponentModel.DataAnnotations;

namespace BankingManagement.Models
{
    public class Beneficiary
    {
        [Key]
        public int BeneficiaryId { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(100)]
        public string BeneficiaryName { get; set; }

        [Required]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required]
        public int BankBranchId { get; set; }

        [Required]
        [StringLength(20)]
        public string IFSCCode { get; set; }

        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual BankBranch BankBranch { get; set; }
    }
}