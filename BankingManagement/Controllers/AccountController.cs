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
    public class AccountController : Controller
    {
        private readonly AccountBusiness _accountBusiness;

        public AccountController()
        {
            BankingDbContext context = new BankingDbContext();

            IGenericRepository<Account> accountRepository =
                new GenericRepository<Account>(context);

            IGenericRepository<Customer> customerRepository =
                new GenericRepository<Customer>(context);

            _accountBusiness =
                new AccountBusiness(
                    accountRepository,
                    customerRepository
                );
        }

        public ActionResult Index()
        {
            var accounts =
                _accountBusiness.GetAllAccounts();

            return View(accounts);
        }

        public ActionResult Details(int id)
        {
            var account =
                _accountBusiness.GetAccountById(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        public ActionResult Create()
        {
            var customers =
                _accountBusiness.GetCustomers();

            ViewBag.Customers = customers;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateAccountDTO createAccountDTO)
        {
            if (ModelState.IsValid)
            {
                _accountBusiness.AddAccount(createAccountDTO);

                return RedirectToAction("Index");
            }

            ViewBag.Customers =
                _accountBusiness.GetCustomers();

            return View(createAccountDTO);
        }

        public ActionResult Edit(int id)
        {
            var account =
                _accountBusiness.GetAccountById(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AccountDTO accountDTO)
        {
            if (ModelState.IsValid)
            {
                _accountBusiness.UpdateAccount(accountDTO);

                return RedirectToAction("Index");
            }

            return View(accountDTO);
        }

        public ActionResult Delete(int id)
        {
            var account =
                _accountBusiness.GetAccountById(id);

            if (account == null)
            {
                return HttpNotFound();
            }

            return View("Delete", account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _accountBusiness.DeleteAccount(id);

            return RedirectToAction("Index");
        }
    }
}