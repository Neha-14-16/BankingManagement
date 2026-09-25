using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class CustomerBusiness
    {
        private readonly IGenericRepository<Customer> _customerRepository;
        private readonly IGenericRepository<User> _userRepository;

        public CustomerBusiness(
            IGenericRepository<Customer> customerRepository,
            IGenericRepository<User> userRepository)
        {
            _customerRepository = customerRepository;
            _userRepository = userRepository;
        }

        public List<CustomerDTO> GetAllCustomers()
        {
            var customers = _customerRepository.GetAll();

            var customerDTOs = new List<CustomerDTO>();

            foreach (var customer in customers)
            {
                customerDTOs.Add(new CustomerDTO
                {
                    CustomerId = customer.CustomerId,
                    UserId = customer.UserId,
                    FullName = customer.FullName,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    DateOfBirth = customer.DateOfBirth,
                    Address = customer.Address,
                    CreatedDate = customer.CreatedDate
                });
            }

            return customerDTOs;
        }

        public CustomerDTO GetCustomerById(int id)
        {
            var customer = _customerRepository.GetById(id);

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
                CreatedDate = System.DateTime.Now
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
                CreatedDate = System.DateTime.Now
            };

            _customerRepository.Add(customer);
            _customerRepository.Save();
        }

        public void UpdateCustomer(CustomerDTO customerDTO)
        {
            var customer =
                _customerRepository.GetById(customerDTO.CustomerId);

            if (customer == null)
            {
                return;
            }

            customer.FullName = customerDTO.FullName;
            customer.Email = customerDTO.Email;
            customer.PhoneNumber = customerDTO.PhoneNumber;
            customer.DateOfBirth = customerDTO.DateOfBirth;
            customer.Address = customerDTO.Address;

            _customerRepository.Update(customer);
            _customerRepository.Save();
        }

        public void DeleteCustomer(int id)
        {
            _customerRepository.Delete(id);
            _customerRepository.Save();
        }
    }
}