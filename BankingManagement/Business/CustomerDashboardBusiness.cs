using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class CustomerDashboardBusiness
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Beneficiary> _beneficiaryRepository;
        private readonly IGenericRepository<BankingTransaction> _transactionRepository;
        private readonly IGenericRepository<AccountTransaction> _accountTransactionRepository;
        private readonly IGenericRepository<PaymentTransaction> _paymentRepository;

        public CustomerDashboardBusiness(
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Account> accountRepository,
            IGenericRepository<Beneficiary> beneficiaryRepository,
            IGenericRepository<BankingTransaction> transactionRepository,
            IGenericRepository<AccountTransaction> accountTransactionRepository,
            IGenericRepository<PaymentTransaction> paymentRepository)
        {
            _customerRepository = customerRepository;
            _accountRepository = accountRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _transactionRepository = transactionRepository;
            _accountTransactionRepository = accountTransactionRepository;
            _paymentRepository = paymentRepository;
        }

        public CustomerDashboardDTO GetDashboard(int userId)
        {
            var customers = _customerRepository.GetAll();

            Customer customer = null;

            foreach (var item in customers)
            {
                if (item.UserId == userId)
                {
                    customer = item;
                    break;
                }
            }

            if (customer == null)
            {
                return null;
            }

            var dashboard = new CustomerDashboardDTO
            {
                CustomerId = customer.CustomerId,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber
            };

            var accounts = _accountRepository.GetAll();

            foreach (var account in accounts)
            {
                if (account.CustomerId == customer.CustomerId)
                {
                    dashboard.Accounts.Add(
                        new AccountDTO
                        {
                            AccountId = account.AccountId,
                            CustomerId = account.CustomerId,
                            AccountNumber = account.AccountNumber,
                            AccountType = account.AccountType,
                            Balance = account.Balance,
                            Status = account.Status,
                            CreatedDate = account.CreatedDate
                        }
                    );
                }
            }

            var beneficiaries = _beneficiaryRepository.GetAll();

            foreach (var beneficiary in beneficiaries)
            {
                if (beneficiary.CustomerId == customer.CustomerId)
                {
                    dashboard.Beneficiaries.Add(
                        new BeneficiaryDTO
                        {
                            BeneficiaryId = beneficiary.BeneficiaryId,
                            CustomerId = beneficiary.CustomerId,
                            BeneficiaryName = beneficiary.BeneficiaryName,
                            AccountNumber = beneficiary.AccountNumber,
                            IFSCCode = beneficiary.IFSCCode,
                            BankName = beneficiary.BankName,
                            CreatedDate = beneficiary.CreatedDate
                        }
                    );
                }
            }

            var transactions = _transactionRepository.GetAll();

            foreach (var transaction in transactions)
            {
                bool fromAccountBelongsToCustomer = false;
                bool toAccountBelongsToCustomer = false;

                foreach (var account in accounts)
                {
                    if (account.CustomerId == customer.CustomerId)
                    {
                        if (account.AccountId == transaction.FromAccountId)
                        {
                            fromAccountBelongsToCustomer = true;
                        }

                        if (account.AccountId == transaction.ToAccountId)
                        {
                            toAccountBelongsToCustomer = true;
                        }
                    }
                }

                if (fromAccountBelongsToCustomer ||
                    toAccountBelongsToCustomer)
                {
                    string fromAccountNumber = "";
                    string toAccountNumber = "";

                    foreach (var account in accounts)
                    {
                        if (account.AccountId == transaction.FromAccountId)
                        {
                            fromAccountNumber = account.AccountNumber;
                        }

                        if (account.AccountId == transaction.ToAccountId)
                        {
                            toAccountNumber = account.AccountNumber;
                        }
                    }

                    dashboard.Transactions.Add(
                        new CustomerTransactionDTO
                        {
                            TransactionId = transaction.TransactionId,
                            FromAccountNumber = fromAccountNumber,
                            ToAccountNumber = toAccountNumber,
                            Amount = transaction.Amount,
                            TransactionDate = transaction.TransactionDate,
                            Status = transaction.Status,
                            ReferenceNumber = transaction.ReferenceNumber
                        }
                    );
                }
            }

            var accountTransactions =
                _accountTransactionRepository.GetAll();

            foreach (var accountTransaction in accountTransactions)
            {
                Account customerAccount = null;

                foreach (var account in accounts)
                {
                    if (account.AccountId ==
                            accountTransaction.AccountId &&
                        account.CustomerId ==
                            customer.CustomerId)
                    {
                        customerAccount = account;
                        break;
                    }
                }

                if (customerAccount != null)
                {
                    dashboard.Transactions.Add(
                        new CustomerTransactionDTO
                        {
                            TransactionId =
                                accountTransaction.AccountTransactionId,

                            FromAccountNumber =
                                customerAccount.AccountNumber,

                            ToAccountNumber =
                                accountTransaction.TransactionType,

                            Amount =
                                accountTransaction.Amount,

                            TransactionDate =
                                accountTransaction.TransactionDate,

                            Status =
                                accountTransaction.Status,

                            ReferenceNumber =
                                accountTransaction.ReferenceNumber
                        }
                    );
                }
            }

            var payments = _paymentRepository.GetAll();

            foreach (var payment in payments)
            {
                Account customerAccount = null;

                foreach (var account in accounts)
                {
                    if (account.AccountId == payment.AccountId &&
                        account.CustomerId == customer.CustomerId)
                    {
                        customerAccount = account;
                        break;
                    }
                }

                if (customerAccount != null)
                {
                    dashboard.Transactions.Add(
                        new CustomerTransactionDTO
                        {
                            TransactionId = payment.PaymentId,

                            FromAccountNumber =
                                customerAccount.AccountNumber,

                            ToAccountNumber =
                                "Payment - " + payment.PaymentType,

                            Amount = payment.Amount,

                            TransactionDate =
                                payment.PaymentDate,

                            Status = payment.Status,

                            ReferenceNumber =
                                payment.ReferenceNumber
                        }
                    );
                }
            }

            return dashboard;
        }
    }
}