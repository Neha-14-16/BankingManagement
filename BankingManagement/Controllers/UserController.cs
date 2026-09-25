using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class UserController : Controller
    {
        private readonly UserBusiness _userBusiness;

        public UserController()
        {
            BankingDbContext context = new BankingDbContext();

            IGenericRepository<User> repository =
                new GenericRepository<User>(context);

            _userBusiness = new UserBusiness(repository);
        }

        public ActionResult Index()
        {
            var users = _userBusiness.GetAllUsers();

            return View(users);
        }

        public ActionResult Details(int id)
        {
            var user = _userBusiness.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UserDTO userDTO)
        {
            if (ModelState.IsValid)
            {
                _userBusiness.AddUser(userDTO);

                return RedirectToAction("Index");
            }

            return View(userDTO);
        }

        public ActionResult Edit(int id)
        {
            var user = _userBusiness.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserDTO userDTO)
        {
            if (string.IsNullOrWhiteSpace(userDTO.PasswordHash))
            {
                ModelState.Remove("PasswordHash");
            }

            if (ModelState.IsValid)
            {
                _userBusiness.UpdateUser(userDTO);

                return RedirectToAction("Index");
            }

            return View(userDTO);
        }

        // GET: User/DeleteUser/2
        public ActionResult DeleteUser(int id)
        {
            var user = _userBusiness.GetUserById(id);

            if (user == null)
            {
                return HttpNotFound();
            }

            return View("Delete", user);
        }

        // POST: User/DeleteConfirmed
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _userBusiness.DeleteUser(id);

            return RedirectToAction("Index");
        }
    }
}