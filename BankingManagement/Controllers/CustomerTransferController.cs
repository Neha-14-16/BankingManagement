using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class CustomerTransferController : Controller
    {
        private readonly CustomerTransferBusiness
            _transferBusiness;

        public CustomerTransferController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<Customer>
                customerRepository =
                new GenericRepository<Customer>(
                    context
                );

            IGenericRepository<Account>
                accountRepository =
                new GenericRepository<Account>(
                    context
                );

            IGenericRepository<Beneficiary>
                beneficiaryRepository =
                new GenericRepository<Beneficiary>(
                    context
                );

            IGenericRepository<BankingTransaction>
                transactionRepository =
                new GenericRepository<BankingTransaction>(
                    context
                );

            _transferBusiness =
                new CustomerTransferBusiness(
                    customerRepository,
                    accountRepository,
                    beneficiaryRepository,
                    transactionRepository
                );
        }


        // GET: CustomerTransfer/Create
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
                _transferBusiness.GetCustomerAccounts(
                    userId
                );

            ViewBag.Beneficiaries =
                _transferBusiness.GetCustomerBeneficiaries(
                    userId
                );

            return View();
        }


        // POST: CustomerTransfer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            CustomerTransferDTO transferDTO)
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
                    _transferBusiness.Transfer(
                        userId,
                        transferDTO
                    );

                if (result)
                {
                    TempData["SuccessMessage"] =
                        "Money transferred successfully.";

                    return RedirectToAction(
                        "Index",
                        "CustomerDashboard"
                    );
                }

                ModelState.AddModelError(
                    "",
                    "Transfer failed. Please check your account, beneficiary, balance, amount, and account status."
                );
            }

            int currentUserId =
                (int)Session["UserId"];

            ViewBag.Accounts =
                _transferBusiness.GetCustomerAccounts(
                    currentUserId
                );

            ViewBag.Beneficiaries =
                _transferBusiness.GetCustomerBeneficiaries(
                    currentUserId
                );

            return View(transferDTO);
        }
    }
}