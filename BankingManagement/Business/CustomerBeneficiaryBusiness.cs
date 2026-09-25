using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class CustomerBeneficiaryBusiness
    {
        private readonly IGenericRepository<Customer>
            _customerRepository;

        private readonly IGenericRepository<Beneficiary>
            _beneficiaryRepository;

        private readonly IGenericRepository<BankBranch>
            _bankBranchRepository;

        public CustomerBeneficiaryBusiness(
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<Beneficiary> beneficiaryRepository,
            IGenericRepository<BankBranch> bankBranchRepository)
        {
            _customerRepository = customerRepository;
            _beneficiaryRepository = beneficiaryRepository;
            _bankBranchRepository = bankBranchRepository;
        }

        public List<CustomerBeneficiaryDTO> GetBeneficiaries(
            int userId)
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

            var bankBranches =
                _bankBranchRepository.GetAll();

            foreach (var beneficiary in beneficiaries)
            {
                if (beneficiary.CustomerId ==
                    customer.CustomerId)
                {
                    BankBranch bankBranch = null;

                    foreach (var branch in bankBranches)
                    {
                        if (branch.BankBranchId ==
                            beneficiary.BankBranchId)
                        {
                            bankBranch = branch;
                            break;
                        }
                    }

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
                                bankBranch != null
                                    ? bankBranch.BankName
                                    : beneficiary.BankName,

                            BranchName =
                                bankBranch != null
                                    ? bankBranch.BranchName
                                    : "",

                            IFSCCode =
                                bankBranch != null
                                    ? bankBranch.IFSCCode
                                    : beneficiary.IFSCCode
                        }
                    );
                }
            }

            return beneficiaryDTOs;
        }

        public List<BankBranch> GetBankBranches()
        {
            return new List<BankBranch>(
                _bankBranchRepository.GetAll()
            );
        }

        public bool AddBeneficiary(
            int userId,
            CustomerBeneficiaryDTO beneficiaryDTO)
        {
            if (beneficiaryDTO == null)
            {
                return false;
            }

            Customer customer =
                GetCustomerByUserId(userId);

            if (customer == null)
            {
                return false;
            }

            BankBranch bankBranch =
                _bankBranchRepository.GetById(
                    beneficiaryDTO.BankBranchId
                );

            if (bankBranch == null)
            {
                return false;
            }

            var beneficiary =
                new Beneficiary
                {
                    CustomerId =
                        customer.CustomerId,

                    BeneficiaryName =
                        beneficiaryDTO.BeneficiaryName,

                    AccountNumber =
                        beneficiaryDTO.AccountNumber,

                    BankBranchId =
                        bankBranch.BankBranchId,

                    BankName =
                        bankBranch.BankName,

                    IFSCCode =
                        bankBranch.IFSCCode,

                    CreatedDate =
                        System.DateTime.Now
                };

            _beneficiaryRepository.Add(
                beneficiary
            );

            _beneficiaryRepository.Save();

            return true;
        }

        public bool DeleteBeneficiary(
            int userId,
            int beneficiaryId)
        {
            Customer customer =
                GetCustomerByUserId(userId);

            if (customer == null)
            {
                return false;
            }

            var beneficiary =
                _beneficiaryRepository.GetById(
                    beneficiaryId
                );

            if (beneficiary == null)
            {
                return false;
            }

            if (beneficiary.CustomerId !=
                customer.CustomerId)
            {
                return false;
            }

            _beneficiaryRepository.Delete(
                beneficiaryId
            );

            _beneficiaryRepository.Save();

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