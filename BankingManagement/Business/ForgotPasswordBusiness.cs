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

        public ForgotPasswordBusiness(
            IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public string GenerateResetToken(
            ForgotPasswordDTO forgotPasswordDTO)
        {
            if (forgotPasswordDTO == null)
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(
                forgotPasswordDTO.Username))
            {
                return null;
            }

            if (string.IsNullOrWhiteSpace(
                forgotPasswordDTO.Email))
            {
                return null;
            }

            string username =
                forgotPasswordDTO.Username.Trim();

            string email =
                forgotPasswordDTO.Email.Trim();

            var users = _repository.GetAll();

            var user = users.FirstOrDefault(
                u =>
                    !string.IsNullOrEmpty(u.Username) &&
                    !string.IsNullOrEmpty(u.Email) &&
                    u.Username.Equals(
                        username,
                        StringComparison.OrdinalIgnoreCase) &&
                    u.Email.Equals(
                        email,
                        StringComparison.OrdinalIgnoreCase)
            );

            if (user == null)
            {
                return null;
            }

            if (!user.IsActive)
            {
                return null;
            }

            // Generate secure token
            byte[] tokenBytes = new byte[32];

            using (var generator =
                   RandomNumberGenerator.Create())
            {
                generator.GetBytes(tokenBytes);
            }

            string token =
                BitConverter
                    .ToString(tokenBytes)
                    .Replace("-", "")
                    .ToLowerInvariant();

            user.PasswordResetToken = token;

            user.PasswordResetTokenExpiry =
                DateTime.Now.AddMinutes(30);

            _repository.Update(user);
            _repository.Save();

            return token;
        }
    }
}