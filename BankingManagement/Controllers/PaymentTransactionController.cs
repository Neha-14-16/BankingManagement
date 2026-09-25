using System.Web.Mvc;
using BankingManagement.Data;
using BankingManagement.Exceptions;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;
using System.Linq;

namespace BankingManagement.Controllers
{
    [AdminAuthorize]
    public class PaymentTransactionController : Controller
    {
        private readonly IGenericRepository<PaymentTransaction>
            _paymentRepository;

        public PaymentTransactionController()
        {
            BankingDbContext context =
                new BankingDbContext();

            _paymentRepository =
                new GenericRepository<PaymentTransaction>(context);
        }

        public ActionResult Index()
        {
            var payments =
                _paymentRepository
                    .GetAll()
                    .OrderByDescending(p => p.PaymentDate)
                    .ToList();

            return View(payments);
        }

        public ActionResult Details(int id)
        {
            var payment =
                _paymentRepository.GetById(id);

            if (payment == null)
            {
                return HttpNotFound();
            }

            return View(payment);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            var payment =
                _paymentRepository.GetById(id);

            if (payment == null)
            {
                return HttpNotFound();
            }

            _paymentRepository.Delete(id);
            _paymentRepository.Save();

            return RedirectToAction("Index");
        }
    }
}