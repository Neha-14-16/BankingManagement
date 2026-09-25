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
            BankingDbContext context = new BankingDbContext();

            IGenericRepository<Customer> customerRepository =
                new GenericRepository<Customer>(context);

            IGenericRepository<User> userRepository =
                new GenericRepository<User>(context);

            _customerBusiness =
                new CustomerBusiness(
                    customerRepository,
                    userRepository
                );
        }

        public ActionResult Index()
        {
            var customers =
                _customerBusiness.GetAllCustomers();

            return View(customers);
        }

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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CustomerDTO customerDTO)
        {
            if (string.IsNullOrWhiteSpace(customerDTO.Username))
            {
                ModelState.AddModelError(
                    "Username",
                    "Username is required."
                );
            }

            if (string.IsNullOrWhiteSpace(customerDTO.Password))
            {
                ModelState.AddModelError(
                    "Password",
                    "Password is required."
                );
            }

            if (ModelState.IsValid)
            {
                _customerBusiness.AddCustomer(customerDTO);

                return RedirectToAction("Index");
            }

            return View(customerDTO);
        }

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
        public ActionResult Edit(CustomerDTO customerDTO)
        {
            if (ModelState.IsValid)
            {
                _customerBusiness.UpdateCustomer(customerDTO);

                return RedirectToAction("Index");
            }

            return View(customerDTO);
        }

        public ActionResult DeleteUser(int id)
        {
            var customer =
                _customerBusiness.GetCustomerById(id);

            if (customer == null)
            {
                return HttpNotFound();
            }

            return View("Delete", customer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _customerBusiness.DeleteCustomer(id);

            return RedirectToAction("Index");
        }
    }
}