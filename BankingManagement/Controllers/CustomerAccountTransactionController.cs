using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class CustomerAccountTransactionController : Controller
    {
        private readonly AccountTransactionBusiness
            _transactionBusiness;


        public CustomerAccountTransactionController()
        {
            BankingDbContext context =
                new BankingDbContext();


            IGenericRepository<AccountTransaction>
                transactionRepository =
                new GenericRepository<AccountTransaction>(
                    context
                );


            IGenericRepository<Account>
                accountRepository =
                new GenericRepository<Account>(
                    context
                );


            IGenericRepository<Customer>
                customerRepository =
                new GenericRepository<Customer>(
                    context
                );


            _transactionBusiness =
                new AccountTransactionBusiness(
                    transactionRepository,
                    accountRepository,
                    customerRepository
                );
        }


        // GET: CustomerAccountTransaction/Create
        public ActionResult Create()
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }

            string role =
                Session["Role"] as string;

            if (string.IsNullOrEmpty(role) ||
                role.ToLower() != "customer")
            {
                return new HttpStatusCodeResult(403);
            }

            int userId =
                (int)Session["UserId"];

            ViewBag.Accounts =
                _transactionBusiness.GetCustomerAccounts(
                    userId
                );

            return View();
        }


        // POST: CustomerAccountTransaction/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            CreateAccountTransactionDTO transactionDTO)
        {
            if (Session["UserId"] == null)
            {
                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }

            string role =
                Session["Role"] as string;

            if (string.IsNullOrEmpty(role) ||
                role.ToLower() != "customer")
            {
                return new HttpStatusCodeResult(403);
            }

            if (ModelState.IsValid)
            {
                int userId =
                    (int)Session["UserId"];

                bool result =
                    _transactionBusiness.ProcessCustomerTransaction(
                        userId,
                        transactionDTO
                    );

                if (result)
                {
                    TempData["SuccessMessage"] =
                        "Transaction completed successfully.";

                    return RedirectToAction(
                        "Index",
                        "CustomerDashboard"
                    );
                }

                ModelState.AddModelError(
                    "",
                    "Transaction failed. Please check your account, amount, account status, and available balance."
                );
            }

            int currentUserId =
                (int)Session["UserId"];

            ViewBag.Accounts =
                _transactionBusiness.GetCustomerAccounts(
                    currentUserId
                );

            return View(transactionDTO);
        }
    }
}