using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class ResetPasswordController : Controller
    {
        private readonly ResetPasswordBusiness _resetPasswordBusiness;

        public ResetPasswordController()
        {
            BankingDbContext context = new BankingDbContext();

            IGenericRepository<User> repository =
                new GenericRepository<User>(context);

            _resetPasswordBusiness =
                new ResetPasswordBusiness(repository);
        }

        // GET: ResetPassword
        public ActionResult Index(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                return Content("Invalid or missing password reset token.");
            }

            var resetPasswordDTO = new ResetPasswordDTO
            {
                Token = token
            };

            return View(resetPasswordDTO);
        }

        // POST: ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ResetPasswordDTO resetPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                bool result =
                    _resetPasswordBusiness.ResetPassword(
                        resetPasswordDTO
                    );

                if (result)
                {
                    TempData["Message"] =
                        "Password reset successfully. Please login.";

                    return RedirectToAction("Index", "Login");
                }

                ModelState.AddModelError(
                    "",
                    "Invalid or expired password reset link."
                );
            }

            return View(resetPasswordDTO);
        }
    }
}