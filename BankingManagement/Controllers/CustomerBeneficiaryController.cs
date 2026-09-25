using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class CustomerBeneficiaryController : Controller
    {
        private readonly CustomerBeneficiaryBusiness
            _beneficiaryBusiness;

        public CustomerBeneficiaryController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<Customer>
                customerRepository =
                new GenericRepository<Customer>(
                    context
                );

            IGenericRepository<Beneficiary>
                beneficiaryRepository =
                new GenericRepository<Beneficiary>(
                    context
                );

            IGenericRepository<BankBranch>
                bankBranchRepository =
                new GenericRepository<BankBranch>(
                    context
                );

            _beneficiaryBusiness =
                new CustomerBeneficiaryBusiness(
                    customerRepository,
                    beneficiaryRepository,
                    bankBranchRepository
                );
        }

        // GET: CustomerBeneficiary
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

            var beneficiaries =
                _beneficiaryBusiness.GetBeneficiaries(
                    userId
                );

            return View(beneficiaries);
        }

        // GET: CustomerBeneficiary/Create
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

            ViewBag.BankBranches =
                _beneficiaryBusiness.GetBankBranches();

            return View();
        }

        // POST: CustomerBeneficiary/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            CustomerBeneficiaryDTO beneficiaryDTO)
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
                    _beneficiaryBusiness.AddBeneficiary(
                        userId,
                        beneficiaryDTO
                    );

                if (result)
                {
                    return RedirectToAction(
                        "Index"
                    );
                }

                ModelState.AddModelError(
                    "",
                    "Unable to add beneficiary."
                );
            }

            ViewBag.BankBranches =
                _beneficiaryBusiness.GetBankBranches();

            return View(beneficiaryDTO);
        }

        // GET: CustomerBeneficiary/Delete
        public ActionResult Delete(int id)
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

            var beneficiaries =
                _beneficiaryBusiness.GetBeneficiaries(
                    userId
                );

            CustomerBeneficiaryDTO beneficiary =
                null;

            foreach (var item in beneficiaries)
            {
                if (item.BeneficiaryId == id)
                {
                    beneficiary = item;
                    break;
                }
            }

            if (beneficiary == null)
            {
                return HttpNotFound();
            }

            return View(beneficiary);
        }

        // POST: CustomerBeneficiary/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
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

            bool result =
                _beneficiaryBusiness.DeleteBeneficiary(
                    userId,
                    id
                );

            if (!result)
            {
                return HttpNotFound();
            }

            return RedirectToAction(
                "Index"
            );
        }
    }
}