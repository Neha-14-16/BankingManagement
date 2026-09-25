using System;
using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class CustomerTransferBusiness
    {
        private readonly IGenericRepository<Customer>
            _customerRepository;

        private readonly IGenericRepository<Account>
            _accountRepository;

        private readonly IGenericRepository<Beneficiary>
            _beneficiaryRepository;

        private readonly IGenericRepository<BankingTransaction>
            _transactionRepository;

        public CustomerTransferBusiness(
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Account> accountRepository,
            IGenericRepository<Beneficiary> beneficiaryRepository,
            IGenericRepository<BankingTransaction> transactionRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _transactionRepository = transactionRepository;
        }

        public List<AccountDTO> GetCustomerAccounts(
            int userId)
        {
            Customer customer =
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


        public List<CustomerBeneficiaryDTO>
            GetCustomerBeneficiaries(int userId)
        {
            Customer customer =
                GetCustomerByUserId(userId);

            var beneficiaryDTOs =
                new List<CustomerBeneficiaryDTO>();

            if (customer == null)
            {
                return beneficiaryDTOs;
            }

            var beneficiaries =
                _beneficiaryRepository.GetAll();

            foreach (var beneficiary in beneficiaries)
            {
                if (beneficiary.CustomerId ==
                    customer.CustomerId)
                {
                    beneficiaryDTOs.Add(
                        new CustomerBeneficiaryDTO
                        {
                            BeneficiaryId =
                                beneficiary.BeneficiaryId,

                            CustomerId =
                                beneficiary.CustomerId,

                            BeneficiaryName =
                                beneficiary.BeneficiaryName,

                            AccountNumber =
                                beneficiary.AccountNumber,

                            BankBranchId =
                                beneficiary.BankBranchId,

                            BankName =
                                beneficiary.BankName,

                            IFSCCode =
                                beneficiary.IFSCCode
                        }
                    );
                }
            }

            return beneficiaryDTOs;
        }


        public bool Transfer(
            int userId,
            CustomerTransferDTO transferDTO)
        {
            if (transferDTO == null)
            {
                return false;
            }

            if (transferDTO.Amount <= 0)
            {
                return false;
            }

            Customer customer =
                GetCustomerByUserId(userId);

            if (customer == null)
            {
                return false;
            }


            // Get customer's source account

            var fromAccount =
                _accountRepository.GetById(
                    transferDTO.FromAccountId
                );

            if (fromAccount == null)
            {
                return false;
            }


            // Make sure source account
            // belongs to logged-in customer

            if (fromAccount.CustomerId !=
                customer.CustomerId)
            {
                return false;
            }


            // Get selected beneficiary

            var beneficiary =
                _beneficiaryRepository.GetById(
                    transferDTO.BeneficiaryId
                );

            if (beneficiary == null)
            {
                return false;
            }


            // Make sure beneficiary
            // belongs to logged-in customer

            if (beneficiary.CustomerId !=
                customer.CustomerId)
            {
                return false;
            }


            // Find destination account
            // using beneficiary account number

            var accounts =
                _accountRepository.GetAll();

            Account toAccount = null;

            foreach (var account in accounts)
            {
                if (account.AccountNumber ==
                    beneficiary.AccountNumber)
                {
                    toAccount = account;
                    break;
                }
            }

            if (toAccount == null)
            {
                return false;
            }


            // Cannot transfer to own account

            if (fromAccount.AccountId ==
                toAccount.AccountId)
            {
                return false;
            }


            // Check account status

            if (fromAccount.Status.ToLower() !=
                "active")
            {
                return false;
            }

            if (toAccount.Status.ToLower() !=
                "active")
            {
                return false;
            }


            // Check balance

            if (fromAccount.Balance <
                transferDTO.Amount)
            {
                return false;
            }


            // Debit source account

            fromAccount.Balance -=
                transferDTO.Amount;


            // Credit destination account

            toAccount.Balance +=
                transferDTO.Amount;


            _accountRepository.Update(
                fromAccount
            );

            _accountRepository.Update(
                toAccount
            );


            // Generate transaction reference

            string referenceNumber =
                "TXN" +
                DateTime.Now.ToString(
                    "yyyyMMddHHmmssfff"
                );


            // Create transaction record

            var transaction =
                new BankingTransaction
                {
                    FromAccountId =
                        fromAccount.AccountId,

                    ToAccountId =
                        toAccount.AccountId,

                    Amount =
                        transferDTO.Amount,

                    TransactionDate =
                        DateTime.Now,

                    Status =
                        "Success",

                    ReferenceNumber =
                        referenceNumber
                };


            _transactionRepository.Add(
                transaction
            );


            _accountRepository.Save();

            _transactionRepository.Save();


            return true;
        }


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