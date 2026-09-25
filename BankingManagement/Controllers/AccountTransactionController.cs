using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class AccountTransactionController : Controller
    {
        private readonly AccountTransactionBusiness
            _transactionBusiness;


        public AccountTransactionController()
        {
            BankingDbContext context =
                new BankingDbContext();


            // Account Transaction Repository
            IGenericRepository<AccountTransaction>
                transactionRepository =
                new GenericRepository<AccountTransaction>(
                    context
                );


            // Account Repository
            IGenericRepository<Account>
                accountRepository =
                new GenericRepository<Account>(
                    context
                );


            // Customer Repository
            IGenericRepository<Customer>
                customerRepository =
                new GenericRepository<Customer>(
                    context
                );


            // Business Layer
            _transactionBusiness =
                new AccountTransactionBusiness(
                    transactionRepository,
                    accountRepository,
                    customerRepository
                );
        }


        // GET: AccountTransaction
        public ActionResult Index()
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
                role.ToLower() != "admin")
            {
                return new HttpStatusCodeResult(403);
            }

            var transactions =
                _transactionBusiness.GetAllTransactions();

            return View(transactions);
        }


        // GET: AccountTransaction/Create
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
                role.ToLower() != "admin")
            {
                return new HttpStatusCodeResult(403);
            }

            ViewBag.Accounts =
                _transactionBusiness.GetAccounts();

            return View();
        }


        // POST: AccountTransaction/Create
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
                role.ToLower() != "admin")
            {
                return new HttpStatusCodeResult(403);
            }

            if (ModelState.IsValid)
            {
                bool result =
                    _transactionBusiness.ProcessTransaction(
                        transactionDTO
                    );

                if (result)
                {
                    return RedirectToAction(
                        "Index"
                    );
                }

                ModelState.AddModelError(
                    "",
                    "Transaction failed. Please check the account, amount, account status, and available balance."
                );
            }

            ViewBag.Accounts =
                _transactionBusiness.GetAccounts();

            return View(transactionDTO);
        }


        // GET: AccountTransaction/Details
        public ActionResult Details(int? id)
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
                role.ToLower() != "admin")
            {
                return new HttpStatusCodeResult(403);
            }

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var transaction =
                _transactionBusiness.GetTransactionById(
                    id.Value
                );

            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }


        // GET: AccountTransaction/Delete
        public ActionResult Delete(int? id)
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
                role.ToLower() != "admin")
            {
                return new HttpStatusCodeResult(403);
            }

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var transaction =
                _transactionBusiness.GetTransactionById(
                    id.Value
                );

            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }


        // POST: AccountTransaction/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
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
                role.ToLower() != "admin")
            {
                return new HttpStatusCodeResult(403);
            }

            if (!id.HasValue)
            {
                return RedirectToAction("Index");
            }

            var transaction =
                _transactionBusiness.GetTransactionById(
                    id.Value
                );

            if (transaction == null)
            {
                return HttpNotFound();
            }

            _transactionBusiness.DeleteTransaction(
                id.Value
            );

            return RedirectToAction(
                "Index"
            );
        }
    }
}