using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class CustomerDashboardController : Controller
    {
        private readonly CustomerDashboardBusiness _dashboardBusiness;

        public CustomerDashboardController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<Customer> customerRepository =
                new GenericRepository<Customer>(context);

            IGenericRepository<Account> accountRepository =
                new GenericRepository<Account>(context);

            IGenericRepository<Beneficiary> beneficiaryRepository =
                new GenericRepository<Beneficiary>(context);

            IGenericRepository<BankingTransaction> transactionRepository =
                new GenericRepository<BankingTransaction>(context);

            IGenericRepository<AccountTransaction> accountTransactionRepository =
                new GenericRepository<AccountTransaction>(context);

            IGenericRepository<PaymentTransaction> paymentRepository =
                new GenericRepository<PaymentTransaction>(context);

            _dashboardBusiness =
                new CustomerDashboardBusiness(
                    customerRepository,
                    accountRepository,
                    beneficiaryRepository,
                    transactionRepository,
                    accountTransactionRepository,
                    paymentRepository
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

            CustomerDashboardDTO dashboard =
                _dashboardBusiness.GetDashboard(userId);

            if (dashboard == null)
            {
                return HttpNotFound();
            }

            return View(dashboard);
        }
    }
}