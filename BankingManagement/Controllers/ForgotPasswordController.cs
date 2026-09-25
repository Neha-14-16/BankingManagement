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
    public class ForgotPasswordController : Controller
    {
        private readonly ForgotPasswordBusiness _forgotPasswordBusiness;
        private readonly EmailService _emailService;

        public ForgotPasswordController()
        {
            BankingDbContext context = new BankingDbContext();

            IGenericRepository<User> repository =
                new GenericRepository<User>(context);

            _forgotPasswordBusiness =
                new ForgotPasswordBusiness(repository);

            _emailService = new EmailService();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                string token =
                    _forgotPasswordBusiness.GenerateResetToken(
                        forgotPasswordDTO
                    );

                if (token != null)
                {
                    string resetLink = Url.Action(
                        "Index",
                        "ResetPassword",
                        new { token = token },
                        Request.Url.Scheme
                    );

                    _emailService.SendPasswordResetEmail(
                        forgotPasswordDTO.Email,
                        resetLink
                    );

                    ViewBag.Message =
                        "Password reset link has been sent to your email.";
                }
                else
                {
                    ModelState.AddModelError(
                        "",
                        "Email not found or user is inactive."
                    );
                }
            }

            return View(forgotPasswordDTO);
        }
    }
}