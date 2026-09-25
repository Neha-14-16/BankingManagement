using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;

namespace BankingManagement.Business
{
    public class LoginBusiness
    {
        private readonly IGenericRepository<User> _repository;

        public LoginBusiness(IGenericRepository<User> repository)
        {
            _repository = repository;
        }

        public UserDTO Login(LoginDTO loginDTO)
        {
            var users = _repository.GetAll();

            foreach (var user in users)
            {
                if (user.Username == loginDTO.Username &&
                    user.PasswordHash == loginDTO.Password)
                {
                    if (!user.IsActive)
                    {
                        return null;
                    }

                    return new UserDTO
                    {
                        UserId = user.UserId,
                        Username = user.Username,
                        PasswordHash = user.PasswordHash,
                        Role = user.Role,
                        IsActive = user.IsActive,
                        CreatedDate = user.CreatedDate
                    };
                }
            }

            return null;
        }
    }
}