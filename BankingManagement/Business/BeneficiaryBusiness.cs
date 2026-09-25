using System;
using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class BeneficiaryBusiness
    {
        private readonly IGenericRepository<Beneficiary> _repository;
        private readonly IGenericRepository<Customer> _customerRepository;

        public BeneficiaryBusiness(
            IGenericRepository<Beneficiary> repository,
            IGenericRepository<Customer> customerRepository)
        {
            _repository = repository;
            _customerRepository = customerRepository;
        }

        public List<BeneficiaryDTO> GetAllBeneficiaries()
        {
            var beneficiaries = _repository.GetAll();

            var beneficiaryDTOs =
                new List<BeneficiaryDTO>();

            foreach (var beneficiary in beneficiaries)
            {
                beneficiaryDTOs.Add(new BeneficiaryDTO
                {
                    BeneficiaryId = beneficiary.BeneficiaryId,
                    CustomerId = beneficiary.CustomerId,
                    BeneficiaryName = beneficiary.BeneficiaryName,
                    AccountNumber = beneficiary.AccountNumber,
                    IFSCCode = beneficiary.IFSCCode,
                    BankName = beneficiary.BankName,
                    CreatedDate = beneficiary.CreatedDate
                });
            }

            return beneficiaryDTOs;
        }

        public BeneficiaryDTO GetBeneficiaryById(int id)
        {
            var beneficiary =
                _repository.GetById(id);

            if (beneficiary == null)
            {
                return null;
            }

            return new BeneficiaryDTO
            {
                BeneficiaryId = beneficiary.BeneficiaryId,
                CustomerId = beneficiary.CustomerId,
                BeneficiaryName = beneficiary.BeneficiaryName,
                AccountNumber = beneficiary.AccountNumber,
                IFSCCode = beneficiary.IFSCCode,
                BankName = beneficiary.BankName,
                CreatedDate = beneficiary.CreatedDate
            };
        }

        public List<CustomerDTO> GetCustomers()
        {
            var customers =
                _customerRepository.GetAll();

            var customerDTOs =
                new List<CustomerDTO>();

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

        public void AddBeneficiary(
            BeneficiaryDTO beneficiaryDTO)
        {
            var beneficiary = new Beneficiary
            {
                CustomerId = beneficiaryDTO.CustomerId,
                BeneficiaryName =
                    beneficiaryDTO.BeneficiaryName,
                AccountNumber =
                    beneficiaryDTO.AccountNumber,
                IFSCCode =
                    beneficiaryDTO.IFSCCode,
                BankName =
                    beneficiaryDTO.BankName,
                CreatedDate = DateTime.Now
            };

            _repository.Add(beneficiary);
            _repository.Save();
        }

        public void UpdateBeneficiary(
            BeneficiaryDTO beneficiaryDTO)
        {
            var beneficiary =
                _repository.GetById(
                    beneficiaryDTO.BeneficiaryId);

            if (beneficiary == null)
            {
                return;
            }

            beneficiary.BeneficiaryName =
                beneficiaryDTO.BeneficiaryName;

            beneficiary.AccountNumber =
                beneficiaryDTO.AccountNumber;

            beneficiary.IFSCCode =
                beneficiaryDTO.IFSCCode;

            beneficiary.BankName =
                beneficiaryDTO.BankName;

            _repository.Update(beneficiary);
            _repository.Save();
        }

        public void DeleteBeneficiary(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
    }
}