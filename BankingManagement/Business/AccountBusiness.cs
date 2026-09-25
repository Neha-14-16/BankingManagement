using System;
using System.Collections.Generic;
using System.Linq;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class AccountBusiness
    {
        private readonly IGenericRepository<Account> _repository;
        private readonly IGenericRepository<Customer> _customerRepository;

        public AccountBusiness(
            IGenericRepository<Account> repository,
            IGenericRepository<Customer> customerRepository)
        {
            _repository = repository;
            _customerRepository = customerRepository;
        }

        public List<AccountDTO> GetAllAccounts()
        {
            var accounts = _repository.GetAll();

            var accountDTOs = new List<AccountDTO>();

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

        public AccountDTO GetAccountById(int id)
        {
            var account = _repository.GetById(id);

            if (account == null)
            {
                return null;
            }

            return new AccountDTO
            {
                AccountId = account.AccountId,
                CustomerId = account.CustomerId,
                AccountNumber = account.AccountNumber,
                AccountType = account.AccountType,
                Balance = account.Balance,
                Status = account.Status,
                CreatedDate = account.CreatedDate
            };
        }

        public List<CustomerDTO> GetCustomers()
        {
            var customers = _customerRepository.GetAll();

            var customerDTOs = new List<CustomerDTO>();

            foreach (var customer in customers)
            {
                customerDTOs.Add(new CustomerDTO
                {
                    CustomerId = customer.CustomerId,
                    FullName = customer.FullName,
                    Email = customer.Email
                });
            }

            return customerDTOs;
        }

        public void AddAccount(CreateAccountDTO accountDTO)
        {
            string accountNumber =
                "ACC" +
                System.DateTime.Now.ToString("yyyyMMddHHmmssfff");

            var account = new Account
            {
                CustomerId = accountDTO.CustomerId,
                AccountNumber = accountNumber,
                AccountType = accountDTO.AccountType,
                Balance = accountDTO.Balance,
                Status = "Active",
                CreatedDate = System.DateTime.Now
            };

            _repository.Add(account);
            _repository.Save();
        }

        public void UpdateAccount(AccountDTO accountDTO)
        {
            var account =
                _repository.GetById(accountDTO.AccountId);

            if (account == null)
            {
                return;
            }

            account.AccountType = accountDTO.AccountType;

            account.Status = accountDTO.Status;

            _repository.Update(account);
            _repository.Save();
        }

        public void DeleteAccount(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
    }
}