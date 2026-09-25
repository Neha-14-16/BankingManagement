using System.Collections.Generic;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class UserBusiness
    {
        private readonly IGenericRepository<User> _repository;

        public UserBusiness(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public List<UserDTO> GetAllUsers()
        {
            var users = _repository.GetAll();

            var userDTOs = new List<UserDTO>();

            foreach (var user in users)
            {
                userDTOs.Add(new UserDTO
                {
                    UserId = user.UserId,
                    Username = user.Username,
                    Email = user.Email,
                    PasswordHash = user.PasswordHash,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedDate = user.CreatedDate
                });
            }

            return userDTOs;
        }

        public UserDTO GetUserById(int id)
        {
            var user = _repository.GetById(id);

            if (user == null)
            {
                return null;
            }

            return new UserDTO
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                Role = user.Role,
                IsActive = user.IsActive,
                CreatedDate = user.CreatedDate
            };
        }

        public void AddUser(UserDTO userDTO)
        {
            var user = new User
            {
                Username = userDTO.Username,
                Email = userDTO.Email,
                PasswordHash = userDTO.PasswordHash,
                Role = userDTO.Role,
                IsActive = userDTO.IsActive,
                CreatedDate = System.DateTime.Now
            };

            _repository.Add(user);
            _repository.Save();
        }

        public void UpdateUser(UserDTO userDTO)
        {
            var user = _repository.GetById(userDTO.UserId);

            if (user == null)
            {
                return;
            }

            user.Username = userDTO.Username;
            user.Email = userDTO.Email;
            user.Role = userDTO.Role;
            user.IsActive = userDTO.IsActive;

            // Change password only if a new password was entered
            if (!string.IsNullOrWhiteSpace(userDTO.PasswordHash))
            {
                user.PasswordHash = userDTO.PasswordHash;
            }

            _repository.Update(user);
            _repository.Save();
        }

        public void DeleteUser(int id)
        {
            _repository.Delete(id);
            _repository.Save();
        }
    }
}