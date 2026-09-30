using System;
using System.Data.Entity.Infrastructure;
using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Exceptions;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    [AdminAuthorize]
    public class CustomerController : Controller
    {
        private readonly CustomerBusiness _customerBusiness;

        public CustomerController()
        {
            BankingDbContext context =
                new BankingDbContext();


            // =================================================
            // CUSTOMER REPOSITORY
            // =================================================

            IGenericRepository<Customer>
                customerRepository =
                new GenericRepository<Customer>(
                    context
                );


            // =================================================
            // USER REPOSITORY
            // =================================================

            IGenericRepository<User>
                userRepository =
                new GenericRepository<User>(
                    context
                );


            // =================================================
            // ACCOUNT REPOSITORY
            // =================================================

            IGenericRepository<Account>
                accountRepository =
                new GenericRepository<Account>(
                    context
                );


            // =================================================
            // BENEFICIARY REPOSITORY
            // =================================================

            IGenericRepository<Beneficiary>
                beneficiaryRepository =
                new GenericRepository<Beneficiary>(
                    context
                );


            // =================================================
            // BANKING TRANSACTION REPOSITORY
            // =================================================

            IGenericRepository<BankingTransaction>
                bankingTransactionRepository =
                new GenericRepository<BankingTransaction>(
                    context
                );


            // =================================================
            // ACCOUNT TRANSACTION REPOSITORY
            // =================================================

            IGenericRepository<AccountTransaction>
                accountTransactionRepository =
                new GenericRepository<AccountTransaction>(
                    context
                );


            // =================================================
            // PAYMENT TRANSACTION REPOSITORY
            // =================================================

            IGenericRepository<PaymentTransaction>
                paymentTransactionRepository =
                new GenericRepository<PaymentTransaction>(
                    context
                );


            // =================================================
            // BUSINESS
            // =================================================

            _customerBusiness =
                new CustomerBusiness(
                    customerRepository,
                    userRepository,
                    accountRepository,
                    beneficiaryRepository,
                    bankingTransactionRepository,
                    accountTransactionRepository,
                    paymentTransactionRepository
                );
        }


        // =====================================================
        // INDEX
        // =====================================================

        public ActionResult Index()
        {
            var customers =
                _customerBusiness.GetAllCustomers();

            return View(customers);
        }


        // =====================================================
        // DETAILS
        // =====================================================

        public ActionResult Details(int id)
        {
            var customer =
                _customerBusiness.GetCustomerById(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }


        // =====================================================
        // CREATE
        // =====================================================

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            CustomerDTO customerDTO)
        {
            if (string.IsNullOrWhiteSpace(
                customerDTO.Username))
            {
                ModelState.AddModelError(
                    "Username",
                    "Username is required."
                );
            }

            if (string.IsNullOrWhiteSpace(
                customerDTO.Password))
            {
                ModelState.AddModelError(
                    "Password",
                    "Password is required."
                );
            }

            if (ModelState.IsValid)
            {
                _customerBusiness.AddCustomer(
                    customerDTO
                );

                return RedirectToAction(
                    "Index"
                );
            }

            return View(customerDTO);
        }


        // =====================================================
        // EDIT
        // =====================================================

        public ActionResult Edit(int id)
        {
            var customer =
                _customerBusiness.GetCustomerById(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View(customer);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            CustomerDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                _customerBusiness.UpdateCustomer(
                    customerDTO
                );

                return RedirectToAction(
                    "Index"
                );
            }

            return View(customerDTO);
        }


        // =====================================================
        // DIRECT DELETE
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var customer =
                _customerBusiness.GetCustomerById(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            try
            {
                _customerBusiness.DeleteCustomer(id);

                TempData["CustomerSuccess"] =
                    "Customer and all related banking records were deleted successfully.";

                return RedirectToAction(
                    "Index"
                );
            }
            catch (DbUpdateException ex)
            {
                ExceptionLogger.Log(ex);

                TempData["CustomerDeleteError"] =
                    "The customer could not be deleted because related banking records still exist.";

                return RedirectToAction(
                    "Index"
                );
            }
            catch (Exception ex)
            {
                ExceptionLogger.Log(ex);

                TempData["CustomerDeleteError"] =
                    "Unable to delete the customer. Please try again.";

                return RedirectToAction(
                    "Index"
                );
            }
        }
    }
}