using System;
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
        private readonly ResetPasswordBusiness
            _resetPasswordBusiness;


        public ResetPasswordController()
        {
            BankingDbContext context =
                new BankingDbContext();


            IGenericRepository<User>
                repository =
                new GenericRepository<User>(
                    context
                );


            _resetPasswordBusiness =
                new ResetPasswordBusiness(
                    repository
                );
        }


        // =====================================================
        // GET: ResetPassword
        // =====================================================

        public ActionResult Index(
            string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                TempData["ResetError"] =
                    "Invalid or missing password reset link.";

                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }


            var resetPasswordDTO =
                new ResetPasswordDTO
                {
                    Token = token
                };


            return View(
                resetPasswordDTO
            );
        }


        // =====================================================
        // POST: ResetPassword
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(
            ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO == null)
            {
                TempData["ResetError"] =
                    "Invalid password reset request.";

                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }


            if (!ModelState.IsValid)
            {
                return View(
                    resetPasswordDTO
                );
            }


            try
            {
                bool result =
                    _resetPasswordBusiness
                        .ResetPassword(
                            resetPasswordDTO
                        );


                if (!result)
                {
                    ModelState.AddModelError(
                        "",
                        "This password reset link is invalid " +
                        "or has expired. Please request a new link."
                    );

                    return View(
                        resetPasswordDTO
                    );
                }


                // =================================================
                // SUCCESS
                // =================================================

                TempData["PasswordResetSuccess"] =
                    "Password reset successfully. " +
                    "Please login with your new password.";


                return RedirectToAction(
                    "Index",
                    "Login"
                );
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "Unable to reset your password. " +
                    "Please try again."
                );

                return View(
                    resetPasswordDTO
                );
            }
        }
    }
}