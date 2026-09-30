using System;
using System.Linq;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class ResetPasswordBusiness
    {
        private readonly IGenericRepository<User> _repository;

        public ResetPasswordBusiness(
            IGenericRepository<User> repository)
        {
            _repository = repository;
        }


        // =====================================================
        // RESET PASSWORD
        // =====================================================

        public bool ResetPassword(
            ResetPasswordDTO resetPasswordDTO)
        {
            if (resetPasswordDTO == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                resetPasswordDTO.Token))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(
                resetPasswordDTO.NewPassword))
            {
                return false;
            }


            // =================================================
            // FIND USER
            // =================================================

            var users =
                _repository.GetAll();

            var user =
                users.FirstOrDefault(
                    u => u.PasswordResetToken ==
                         resetPasswordDTO.Token
                );


            if (user == null)
            {
                return false;
            }


            // =================================================
            // ACTIVE USER CHECK
            // =================================================

            if (!user.IsActive)
            {
                return false;
            }


            // =================================================
            // TOKEN EXPIRY CHECK
            // =================================================

            if (!user.PasswordResetTokenExpiry.HasValue)
            {
                return false;
            }

            if (user.PasswordResetTokenExpiry.Value <=
                DateTime.Now)
            {
                return false;
            }


            // =================================================
            // UPDATE PASSWORD
            // =================================================

            user.PasswordHash =
                resetPasswordDTO.NewPassword;


            // =================================================
            // CLEAR TOKEN
            // =================================================

            user.PasswordResetToken = null;

            user.PasswordResetTokenExpiry = null;


            _repository.Update(user);

            _repository.Save();


            return true;
        }
    }
}