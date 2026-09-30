using System;
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
        private readonly ForgotPasswordBusiness
            _forgotPasswordBusiness;

        private readonly EmailService
            _emailService;

        public ForgotPasswordController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<User> repository =
                new GenericRepository<User>(context);

            _forgotPasswordBusiness =
                new ForgotPasswordBusiness(repository);

            _emailService =
                new EmailService();
        }

        // GET: ForgotPassword
        public ActionResult Index()
        {
            return View(
                new ForgotPasswordDTO()
            );
        }

        // POST: ForgotPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(
            ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return View(forgotPasswordDTO);
            }

            try
            {
                string token =
                    _forgotPasswordBusiness
                        .GenerateResetToken(
                            forgotPasswordDTO
                        );

                if (token == null)
                {
                    ModelState.AddModelError(
                        "",
                        "The username and email address do not match a registered account."
                    );

                    return View(forgotPasswordDTO);
                }

                string resetLink =
                    Url.Action(
                        "Index",
                        "ResetPassword",
                        new
                        {
                            token = token
                        },
                        Request.Url.Scheme
                    );

                _emailService.SendPasswordResetEmail(
                    forgotPasswordDTO.Email.Trim(),
                    resetLink
                );

                ViewBag.Message =
                    "A password reset link has been sent to your registered email address.";
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to process your password reset request. Please try again."
                );
            }

            return View(forgotPasswordDTO);
        }
    }
}