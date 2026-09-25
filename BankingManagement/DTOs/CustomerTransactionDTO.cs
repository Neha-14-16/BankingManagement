using System;

namespace BankingManagement.DTOs
{
    public class CustomerTransactionDTO
    {
        public int TransactionId { get; set; }

        public string FromAccountNumber { get; set; }

        public string ToAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public DateTime TransactionDate { get; set; }

        public string Status { get; set; }

        public string ReferenceNumber { get; set; }
    }
}