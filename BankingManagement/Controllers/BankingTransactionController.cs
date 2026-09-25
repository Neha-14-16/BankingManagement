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
    public class BankingTransactionController : Controller
    {
        private readonly BankingTransactionBusiness
            _transactionBusiness;

        public BankingTransactionController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<BankingTransaction>
                transactionRepository =
                new GenericRepository<BankingTransaction>(
                    context
                );

            IGenericRepository<Account>
                accountRepository =
                new GenericRepository<Account>(
                    context
                );

            _transactionBusiness =
                new BankingTransactionBusiness(
                    transactionRepository,
                    accountRepository
                );
        }

        public ActionResult Index()
        {
            var transactions =
                _transactionBusiness.GetAllTransactions();

            return View(transactions);
        }

        public ActionResult Details(int id)
        {
            var transaction =
                _transactionBusiness.GetTransactionById(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View(transaction);
        }

        public ActionResult Create()
        {
            var accounts =
                _transactionBusiness.GetAccounts();

            ViewBag.Accounts = accounts;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            CreateBankingTransactionDTO transactionDTO)
        {
            if (ModelState.IsValid)
            {
                bool result =
                    _transactionBusiness.Transfer(
                        transactionDTO
                    );

                if (result)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(
                    "",
                    "Transfer failed. Please check the accounts, balance, status, and amount."
                );
            }

            ViewBag.Accounts =
                _transactionBusiness.GetAccounts();

            return View(transactionDTO);
        }

        public ActionResult Delete(int id)
        {
            var transaction =
                _transactionBusiness.GetTransactionById(id);

            if (transaction == null)
            {
                return HttpNotFound();
            }

            return View("Delete", transaction);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _transactionBusiness.DeleteTransaction(id);

            return RedirectToAction("Index");
        }
    }
}