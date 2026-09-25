using System.ComponentModel.DataAnnotations;

namespace BankingManagement.Models
{
    public class BankBranch
    {
        [Key]
        public int BankBranchId { get; set; }

        [Required]
        [StringLength(100)]
        public string BankName { get; set; }

        [Required]
        [StringLength(100)]
        public string BranchName { get; set; }

        [Required]
        [StringLength(11)]
        public string IFSCCode { get; set; }
    }
}