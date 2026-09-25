using System;
using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class BankingTransactionBusiness
    {
        private readonly IGenericRepository<BankingTransaction>
            _transactionRepository;

        private readonly IGenericRepository<Account>
            _accountRepository;

        public BankingTransactionBusiness(
            IGenericRepository<BankingTransaction> transactionRepository,
            IGenericRepository<Account> accountRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
        }

        public List<BankingTransactionDTO> GetAllTransactions()
        {
            var transactions =
                _transactionRepository.GetAll();

            var transactionDTOs =
                new List<BankingTransactionDTO>();

            foreach (var transaction in transactions)
            {
                transactionDTOs.Add(new BankingTransactionDTO
                {
                    TransactionId = transaction.TransactionId,
                    FromAccountId = transaction.FromAccountId,
                    ToAccountId = transaction.ToAccountId,
                    Amount = transaction.Amount,
                    TransactionDate = transaction.TransactionDate,
                    Status = transaction.Status,
                    ReferenceNumber = transaction.ReferenceNumber
                });
            }

            return transactionDTOs;
        }

        public BankingTransactionDTO GetTransactionById(int id)
        {
            var transaction =
                _transactionRepository.GetById(id);

            if (transaction == null)
            {
                return null;
            }

            return new BankingTransactionDTO
            {
                TransactionId = transaction.TransactionId,
                FromAccountId = transaction.FromAccountId,
                ToAccountId = transaction.ToAccountId,
                Amount = transaction.Amount,
                TransactionDate = transaction.TransactionDate,
                Status = transaction.Status,
                ReferenceNumber = transaction.ReferenceNumber
            };
        }

        public List<AccountDTO> GetAccounts()
        {
            var accounts =
                _accountRepository.GetAll();

            var accountDTOs =
                new List<AccountDTO>();

            foreach (var account in accounts)
            {
                accountDTOs.Add(new AccountDTO
                {
                    AccountId = account.AccountId,
                    CustomerId = account.CustomerId,
                    AccountNumber = account.AccountNumber,
                    AccountType = account.AccountType,
                    Balance = account.Balance,
                    Status = account.Status,
                    CreatedDate = account.CreatedDate
                });
            }

            return accountDTOs;
        }

        public bool Transfer(
            CreateBankingTransactionDTO transactionDTO)
        {
            if (transactionDTO.Amount <= 0)
            {
                return false;
            }

            if (transactionDTO.FromAccountId ==
                transactionDTO.ToAccountId)
            {
                return false;
            }

            var fromAccount =
                _accountRepository.GetById(
                    transactionDTO.FromAccountId);

            var toAccount =
                _accountRepository.GetById(
                    transactionDTO.ToAccountId);

            if (fromAccount == null ||
                toAccount == null)
            {
                return false;
            }

            if (fromAccount.Status.ToLower() != "active" ||
                toAccount.Status.ToLower() != "active")
            {
                return false;
            }

            if (fromAccount.Balance <
                transactionDTO.Amount)
            {
                return false;
            }

            fromAccount.Balance -=
                transactionDTO.Amount;

            toAccount.Balance +=
                transactionDTO.Amount;

            _accountRepository.Update(fromAccount);
            _accountRepository.Update(toAccount);

            string referenceNumber =
                "TXN" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmssfff");

            var transaction = new BankingTransaction
            {
                FromAccountId =
                    transactionDTO.FromAccountId,

                ToAccountId =
                    transactionDTO.ToAccountId,

                Amount =
                    transactionDTO.Amount,

                TransactionDate =
                    DateTime.Now,

                Status = "Success",

                ReferenceNumber =
                    referenceNumber
            };

            _transactionRepository.Add(transaction);

            _accountRepository.Save();
            _transactionRepository.Save();

            return true;
        }

        public void DeleteTransaction(int id)
        {
            _transactionRepository.Delete(id);
            _transactionRepository.Save();
        }
    }
}