using System.Linq;
using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;
using BankingManagement.Services;

namespace BankingManagement.Controllers
{
    public class CustomerPaymentController : Controller
    {
        private readonly PaymentBusiness _paymentBusiness;
        private readonly IGenericRepository<Account> _accountRepository;

        public CustomerPaymentController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<Account> accountRepository =
                new GenericRepository<Account>(context);

            IGenericRepository<PaymentTransaction> paymentRepository =
                new GenericRepository<PaymentTransaction>(context);

            RazorpayService razorpayService =
                new RazorpayService();

            _accountRepository =
                accountRepository;

            _paymentBusiness =
                new PaymentBusiness(
                    accountRepository,
                    paymentRepository,
                    razorpayService
                );
        }

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
                role.ToLower() != "customer")
            {
                return new HttpStatusCodeResult(403);
            }

            int userId =
                (int)Session["UserId"];

            var accounts =
                _accountRepository
                    .GetAll()
                    .Where(a =>
                        a.Customer != null &&
                        a.Customer.UserId == userId &&
                        a.Status.ToLower() == "active")
                    .ToList();

            ViewBag.Accounts = accounts;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateOrder(
            CreatePaymentTransactionDTO paymentDTO)
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

            var accounts =
                _accountRepository
                    .GetAll()
                    .Where(a =>
                        a.Customer != null &&
                        a.Customer.UserId == userId &&
                        a.Status.ToLower() == "active")
                    .ToList();

            ViewBag.Accounts = accounts;

            if (!ModelState.IsValid)
            {
                return View(
                    "Index",
                    paymentDTO
                );
            }

            string orderId =
                _paymentBusiness.CreatePaymentOrder(
                    userId,
                    paymentDTO
                );

            if (orderId == null)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to create payment order. Please check your account and balance."
                );

                return View(
                    "Index",
                    paymentDTO
                );
            }

            ViewBag.OrderId = orderId;

            return View(
                "Index",
                paymentDTO
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessSuccessfulPayment(
            PaymentSuccessDTO paymentDTO)
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

            if (!ModelState.IsValid)
            {
                TempData["PaymentMessage"] =
                    "Invalid payment information.";

                return RedirectToAction("Index");
            }

            int userId =
                (int)Session["UserId"];

            bool paymentProcessed =
                _paymentBusiness.ProcessSuccessfulPayment(
                    userId,
                    paymentDTO
                );

            if (!paymentProcessed)
            {
                TempData["PaymentMessage"] =
                    "Payment verification failed. Payment was not recorded.";

                return RedirectToAction("Index");
            }

            TempData["PaymentMessage"] =
                "Payment successful. Amount deducted from your account.";

            return RedirectToAction(
                "Index",
                "CustomerDashboard"
            );
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ProcessPaymentFlag(
            PaymentFlagDTO paymentDTO)
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

            if (!ModelState.IsValid)
            {
                TempData["PaymentMessage"] =
                    "Invalid payment information.";

                return RedirectToAction("Index");
            }

            int userId =
                (int)Session["UserId"];

            bool processed =
                _paymentBusiness.ProcessPaymentFlag(
                    userId,
                    paymentDTO
                );

            if (!processed)
            {
                TempData["PaymentMessage"] =
                    "Payment could not be processed.";

                return RedirectToAction("Index");
            }

            if (paymentDTO.PaymentFlag == "Success")
            {
                TempData["PaymentMessage"] =
                    "Payment successful. Amount deducted from your account.";
            }
            else
            {
                TempData["PaymentMessage"] =
                    "Payment failed. No amount was deducted from your account.";
            }

            return RedirectToAction(
                "Index",
                "CustomerDashboard"
            );
        }
    }
}