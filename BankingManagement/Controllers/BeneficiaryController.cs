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
    public class BeneficiaryController : Controller
    {
        private readonly BeneficiaryBusiness _beneficiaryBusiness;

        public BeneficiaryController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<Beneficiary> beneficiaryRepository =
                new GenericRepository<Beneficiary>(context);

            IGenericRepository<Customer> customerRepository =
                new GenericRepository<Customer>(context);

            _beneficiaryBusiness =
                new BeneficiaryBusiness(
                    beneficiaryRepository,
                    customerRepository
                );
        }

        public ActionResult Index()
        {
            var beneficiaries =
                _beneficiaryBusiness.GetAllBeneficiaries();

            return View(beneficiaries);
        }

        public ActionResult Details(int id)
        {
            var beneficiary =
                _beneficiaryBusiness.GetBeneficiaryById(id);

            if (beneficiary == null)
            {
                return HttpNotFound();
            }

            return View(beneficiary);
        }

        public ActionResult Create()
        {
            var customers =
                _beneficiaryBusiness.GetCustomers();

            ViewBag.Customers = customers;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            BeneficiaryDTO beneficiaryDTO)
        {
            if (ModelState.IsValid)
            {
                _beneficiaryBusiness.AddBeneficiary(
                    beneficiaryDTO
                );

                return RedirectToAction("Index");
            }

            ViewBag.Customers =
                _beneficiaryBusiness.GetCustomers();

            return View(beneficiaryDTO);
        }

        public ActionResult Edit(int id)
        {
            var beneficiary =
                _beneficiaryBusiness.GetBeneficiaryById(id);

            if (beneficiary == null)
            {
                return HttpNotFound();
            }

            return View(beneficiary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            BeneficiaryDTO beneficiaryDTO)
        {
            if (ModelState.IsValid)
            {
                _beneficiaryBusiness.UpdateBeneficiary(
                    beneficiaryDTO
                );

                return RedirectToAction("Index");
            }

            return View(beneficiaryDTO);
        }

        public ActionResult Delete(int id)
        {
            var beneficiary =
                _beneficiaryBusiness.GetBeneficiaryById(id);

            if (beneficiary == null)
            {
                return HttpNotFound();
            }

            return View("Delete", beneficiary);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _beneficiaryBusiness.DeleteBeneficiary(id);

            return RedirectToAction("Index");
        }
    }
}