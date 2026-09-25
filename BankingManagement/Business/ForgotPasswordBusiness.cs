using System;
using System.Linq;
using System.Security.Cryptography;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class ForgotPasswordBusiness
    {
        private readonly IGenericRepository<User> _repository;

        public ForgotPasswordBusiness(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public string GenerateResetToken(ForgotPasswordDTO forgotPasswordDTO)
        {
            var users = _repository.GetAll();

            var user = users.FirstOrDefault(
                u => u.Email == forgotPasswordDTO.Email
            );

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            byte[] tokenBytes = new byte[32];

            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(tokenBytes);
            }

            string token = Convert.ToBase64String(tokenBytes);

            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.Now.AddMinutes(30);

            _repository.Update(user);
            _repository.Save();

            return token;
        }
    }
}