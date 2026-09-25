using System.Collections.Generic;

namespace BankingManagement.DTOs
{
    public class CustomerDashboardDTO
    {
        public int CustomerId { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<AccountDTO> Accounts { get; set; }

        public List<BeneficiaryDTO> Beneficiaries { get; set; }

        public List<CustomerTransactionDTO> Transactions { get; set; }

        public CustomerDashboardDTO()
        {
            Accounts = new List<AccountDTO>();

            Beneficiaries =
                new List<BeneficiaryDTO>();

            Transactions =
                new List<CustomerTransactionDTO>();
        }
    }
}