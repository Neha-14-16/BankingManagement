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

        public ResetPasswordBusiness(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public bool ResetPassword(ResetPasswordDTO resetPasswordDTO)
        {
            var users = _repository.GetAll();

            var user = users.FirstOrDefault(
                u => u.PasswordResetToken == resetPasswordDTO.Token
            );

            if (user == null)
            {
                return false;
            }

            if (!user.PasswordResetTokenExpiry.HasValue)
            {
                return false;
            }

            if (user.PasswordResetTokenExpiry.Value < DateTime.Now)
            {
                return false;
            }

            user.PasswordHash = resetPasswordDTO.NewPassword;

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            _repository.Update(user);
            _repository.Save();

            return true;
        }
    }
}