using System;
using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class AccountTransactionBusiness
    {
        private readonly IGenericRepository<AccountTransaction>
            _transactionRepository;

        private readonly IGenericRepository<Account>
            _accountRepository;

        private readonly IGenericRepository<Customer>
            _customerRepository;


        public AccountTransactionBusiness(
            IGenericRepository<AccountTransaction> transactionRepository,
            IGenericRepository<Account> accountRepository,
            IGenericRepository<Customer> customerRepository)
        {
            _transactionRepository = transactionRepository;
            _accountRepository = accountRepository;
            _customerRepository = customerRepository;
        }


        // Get all Deposit / Withdrawal transactions
        public List<AccountTransactionDTO>
            GetAllTransactions()
        {
            var transactions =
                _transactionRepository.GetAll();

            var transactionDTOs =
                new List<AccountTransactionDTO>();

            foreach (var transaction in transactions)
            {
                var account =
                    _accountRepository.GetById(
                        transaction.AccountId
                    );

                transactionDTOs.Add(
                    new AccountTransactionDTO
                    {
                        AccountTransactionId =
                            transaction.AccountTransactionId,

                        AccountId =
                            transaction.AccountId,

                        AccountNumber =
                            account != null
                                ? account.AccountNumber
                                : "",

                        TransactionType =
                            transaction.TransactionType,

                        Amount =
                            transaction.Amount,

                        TransactionDate =
                            transaction.TransactionDate,

                        Status =
                            transaction.Status,

                        ReferenceNumber =
                            transaction.ReferenceNumber
                    }
                );
            }

            return transactionDTOs;
        }


        // Get transaction by ID
        public AccountTransactionDTO
            GetTransactionById(int id)
        {
            var transaction =
                _transactionRepository.GetById(id);

            if (transaction == null)
            {
                return null;
            }

            var account =
                _accountRepository.GetById(
                    transaction.AccountId
                );

            return new AccountTransactionDTO
            {
                AccountTransactionId =
                    transaction.AccountTransactionId,

                AccountId =
                    transaction.AccountId,

                AccountNumber =
                    account != null
                        ? account.AccountNumber
                        : "",

                TransactionType =
                    transaction.TransactionType,

                Amount =
                    transaction.Amount,

                TransactionDate =
                    transaction.TransactionDate,

                Status =
                    transaction.Status,

                ReferenceNumber =
                    transaction.ReferenceNumber
            };
        }


        // Get all accounts
        public List<AccountDTO> GetAccounts()
        {
            var accounts =
                _accountRepository.GetAll();

            var accountDTOs =
                new List<AccountDTO>();

            foreach (var account in accounts)
            {
                accountDTOs.Add(
                    new AccountDTO
                    {
                        AccountId =
                            account.AccountId,

                        CustomerId =
                            account.CustomerId,

                        AccountNumber =
                            account.AccountNumber,

                        AccountType =
                            account.AccountType,

                        Balance =
                            account.Balance,

                        Status =
                            account.Status,

                        CreatedDate =
                            account.CreatedDate
                    }
                );
            }

            return accountDTOs;
        }


        // Get accounts belonging to a customer
        public List<AccountDTO>
            GetCustomerAccounts(int userId)
        {
            var customer =
                GetCustomerByUserId(userId);

            var accountDTOs =
                new List<AccountDTO>();

            if (customer == null)
            {
                return accountDTOs;
            }

            var accounts =
                _accountRepository.GetAll();

            foreach (var account in accounts)
            {
                if (account.CustomerId ==
                    customer.CustomerId)
                {
                    accountDTOs.Add(
                        new AccountDTO
                        {
                            AccountId =
                                account.AccountId,

                            CustomerId =
                                account.CustomerId,

                            AccountNumber =
                                account.AccountNumber,

                            AccountType =
                                account.AccountType,

                            Balance =
                                account.Balance,

                            Status =
                                account.Status,

                            CreatedDate =
                                account.CreatedDate
                        }
                    );
                }
            }

            return accountDTOs;
        }


        // Admin Deposit / Withdrawal
        public bool ProcessTransaction(
            CreateAccountTransactionDTO transactionDTO)
        {
            return ProcessTransactionInternal(
                transactionDTO,
                null
            );
        }


        // Customer Deposit / Withdrawal
        public bool ProcessCustomerTransaction(
            int userId,
            CreateAccountTransactionDTO transactionDTO)
        {
            return ProcessTransactionInternal(
                transactionDTO,
                userId
            );
        }


        // Common Deposit / Withdrawal logic
        private bool ProcessTransactionInternal(
            CreateAccountTransactionDTO transactionDTO,
            int? userId)
        {
            if (transactionDTO == null)
            {
                return false;
            }

            if (transactionDTO.Amount <= 0)
            {
                return false;
            }

            if (transactionDTO.TransactionType != "Deposit" &&
                transactionDTO.TransactionType != "Withdrawal")
            {
                return false;
            }


            var account =
                _accountRepository.GetById(
                    transactionDTO.AccountId
                );

            if (account == null)
            {
                return false;
            }


            // Customer ownership check
            if (userId.HasValue)
            {
                var customer =
                    GetCustomerByUserId(
                        userId.Value
                    );

                if (customer == null)
                {
                    return false;
                }

                if (account.CustomerId !=
                    customer.CustomerId)
                {
                    return false;
                }
            }


            // Account must be active
            if (string.IsNullOrEmpty(account.Status) ||
                account.Status.ToLower() != "active")
            {
                return false;
            }


            // Deposit
            if (transactionDTO.TransactionType ==
                "Deposit")
            {
                account.Balance +=
                    transactionDTO.Amount;
            }


            // Withdrawal
            if (transactionDTO.TransactionType ==
                "Withdrawal")
            {
                if (account.Balance <
                    transactionDTO.Amount)
                {
                    return false;
                }

                account.Balance -=
                    transactionDTO.Amount;
            }


            // Update account balance
            _accountRepository.Update(account);


            // Generate reference number
            string prefix =
                transactionDTO.TransactionType ==
                "Deposit"
                    ? "DEP"
                    : "WDL";

            string referenceNumber =
                prefix +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmssfff"
                );


            // Create transaction record
            var transaction =
                new AccountTransaction
                {
                    AccountId =
                        transactionDTO.AccountId,

                    TransactionType =
                        transactionDTO.TransactionType,

                    Amount =
                        transactionDTO.Amount,

                    TransactionDate =
                        DateTime.Now,

                    Status =
                        "Success",

                    ReferenceNumber =
                        referenceNumber
                };


            // Add transaction
            _transactionRepository.Add(
                transaction
            );


            // Save changes
            _accountRepository.Save();

            _transactionRepository.Save();

            return true;
        }


        // Delete transaction
        public void DeleteTransaction(int id)
        {
            _transactionRepository.Delete(id);

            _transactionRepository.Save();
        }


        // Find customer using UserId
        private Customer GetCustomerByUserId(
            int userId)
        {
            var customers =
                _customerRepository.GetAll();

            foreach (var customer in customers)
            {
                if (customer.UserId == userId)
                {
                    return customer;
                }
            }

            return null;
        }
    }
}