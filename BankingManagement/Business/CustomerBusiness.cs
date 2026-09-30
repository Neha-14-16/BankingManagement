using System;
using System.Collections.Generic;
using System.Linq;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class CustomerBusiness
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Account> _accountRepository;
        private readonly IGenericRepository<Beneficiary> _beneficiaryRepository;
        private readonly IGenericRepository<BankingTransaction> _bankingTransactionRepository;
        private readonly IGenericRepository<AccountTransaction> _accountTransactionRepository;
        private readonly IGenericRepository<PaymentTransaction> _paymentTransactionRepository;

        public CustomerBusiness(
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<User> userRepository,
            IGenericRepository<Account> accountRepository,
            IGenericRepository<Beneficiary> beneficiaryRepository,
            IGenericRepository<BankingTransaction> bankingTransactionRepository,
            IGenericRepository<AccountTransaction> accountTransactionRepository,
            IGenericRepository<PaymentTransaction> paymentTransactionRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _bankingTransactionRepository = bankingTransactionRepository;
            _accountTransactionRepository = accountTransactionRepository;
            _paymentTransactionRepository = paymentTransactionRepository;
        }


        // =====================================================
        // GET ALL CUSTOMERS
        // =====================================================

        public List<CustomerDTO> GetAllCustomers()
        {
            var customers =
                _customerRepository.GetAll();

            var customerDTOs =
                new List<CustomerDTO>();

            foreach (var customer in customers)
            {
                customerDTOs.Add(
                    new CustomerDTO
                    {
                        CustomerId = customer.CustomerId,
                        UserId = customer.UserId,
                        FullName = customer.FullName,
                        Email = customer.Email,
                        PhoneNumber = customer.PhoneNumber,
                        DateOfBirth = customer.DateOfBirth,
                        Address = customer.Address,
                        CreatedDate = customer.CreatedDate
                    }
                );
            }

            return customerDTOs;
        }


        // =====================================================
        // GET CUSTOMER BY ID
        // =====================================================

        public CustomerDTO GetCustomerById(int id)
        {
            var customer =
                _customerRepository.GetById(id);

            if (customer == null)
            {
                return null;
            }

            return new CustomerDTO
            {
                CustomerId = customer.CustomerId,
                UserId = customer.UserId,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                DateOfBirth = customer.DateOfBirth,
                Address = customer.Address,
                CreatedDate = customer.CreatedDate
            };
        }


        // =====================================================
        // ADD CUSTOMER
        // =====================================================

        public void AddCustomer(CustomerDTO customerDTO)
        {
            // Create User account first

            var user = new User
            {
                Username = customerDTO.Username,
                Email = customerDTO.Email,
                PasswordHash = customerDTO.Password,
                Role = "customer",
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            _userRepository.Add(user);

            _userRepository.Save();


            // Create Customer using generated UserId

            var customer = new Customer
            {
                UserId = user.UserId,
                FullName = customerDTO.FullName,
                Email = customerDTO.Email,
                PhoneNumber = customerDTO.PhoneNumber,
                DateOfBirth = customerDTO.DateOfBirth,
                Address = customerDTO.Address,
                CreatedDate = DateTime.Now
            };

            _customerRepository.Add(customer);

            _customerRepository.Save();
        }


        // =====================================================
        // UPDATE CUSTOMER
        // =====================================================

        public void UpdateCustomer(CustomerDTO customerDTO)
        {
            var customer =
                _customerRepository.GetById(
                    customerDTO.CustomerId
                );

            if (customer == null)
            {
                return;
            }

            customer.FullName =
                customerDTO.FullName;

            customer.Email =
                customerDTO.Email;

            customer.PhoneNumber =
                customerDTO.PhoneNumber;

            customer.DateOfBirth =
                customerDTO.DateOfBirth;

            customer.Address =
                customerDTO.Address;

            _customerRepository.Update(customer);

            _customerRepository.Save();
        }


        // =====================================================
        // DELETE CUSTOMER + ALL RELATED RECORDS
        // =====================================================

        public void DeleteCustomer(int id)
        {
            // Get customer first

            var customer =
                _customerRepository.GetById(id);

            if (customer == null)
            {
                return;
            }


            // =================================================
            // GET CUSTOMER ACCOUNTS
            // =================================================

            var accounts =
                _accountRepository
                    .GetAll()
                    .Where(a => a.CustomerId == id)
                    .ToList();


            // =================================================
            // DELETE BANKING TRANSACTIONS
            //
            // A transaction can reference the customer's
            // account as either FromAccount or ToAccount.
            // =================================================

            var bankingTransactions =
                _bankingTransactionRepository
                    .GetAll()
                    .ToList();

            foreach (var transaction in bankingTransactions)
            {
                bool usesCustomerAccount = false;

                foreach (var account in accounts)
                {
                    if (transaction.FromAccountId == account.AccountId ||
                        transaction.ToAccountId == account.AccountId)
                    {
                        usesCustomerAccount = true;
                        break;
                    }
                }

                if (usesCustomerAccount)
                {
                    _bankingTransactionRepository.Delete(
                        transaction.TransactionId
                    );
                }
            }


            // =================================================
            // DELETE PAYMENT TRANSACTIONS
            // =================================================

            var paymentTransactions =
                _paymentTransactionRepository
                    .GetAll()
                    .ToList();

            foreach (var payment in paymentTransactions)
            {
                bool usesCustomerAccount = false;

                foreach (var account in accounts)
                {
                    if (payment.AccountId == account.AccountId)
                    {
                        usesCustomerAccount = true;
                        break;
                    }
                }

                if (usesCustomerAccount)
                {
                    _paymentTransactionRepository.Delete(
                        payment.PaymentId
                    );
                }
            }


            // =================================================
            // DELETE ACCOUNT TRANSACTIONS
            // =================================================

            var accountTransactions =
                _accountTransactionRepository
                    .GetAll()
                    .ToList();

            foreach (var transaction in accountTransactions)
            {
                bool usesCustomerAccount = false;

                foreach (var account in accounts)
                {
                    if (transaction.AccountId == account.AccountId)
                    {
                        usesCustomerAccount = true;
                        break;
                    }
                }

                if (usesCustomerAccount)
                {
                    _accountTransactionRepository.Delete(
                        transaction.AccountTransactionId
                    );
                }
            }


            // =================================================
            // DELETE BENEFICIARIES
            // =================================================

            var beneficiaries =
                _beneficiaryRepository
                    .GetAll()
                    .Where(b => b.CustomerId == id)
                    .ToList();

            foreach (var beneficiary in beneficiaries)
            {
                _beneficiaryRepository.Delete(
                    beneficiary.BeneficiaryId
                );
            }


            // =================================================
            // DELETE ACCOUNTS
            // =================================================

            foreach (var account in accounts)
            {
                _accountRepository.Delete(
                    account.AccountId
                );
            }


            // =================================================
            // DELETE CUSTOMER
            // =================================================

            _customerRepository.Delete(id);


            // =================================================
            // DELETE LOGIN USER
            // =================================================

            if (customer.UserId > 0)
            {
                _userRepository.Delete(
                    customer.UserId
                );
            }


            // =================================================
            // SAVE EVERYTHING
            // =================================================

            _customerRepository.Save();
        }
    }
}