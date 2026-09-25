using System.Web.Mvc;
using BankingManagement.Business;
using BankingManagement.Data;
using BankingManagement.DTOs;
using BankingManagement.Interfaces;
using BankingManagement.Models;
using BankingManagement.Repository;

namespace BankingManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly LoginBusiness _loginBusiness;

        public LoginController()
        {
            BankingDbContext context =
                new BankingDbContext();

            IGenericRepository<User> repository =
                new GenericRepository<User>(context);

            _loginBusiness =
                new LoginBusiness(repository);
        }


        // GET: Login
        public ActionResult Index()
        {
            return View();
        }


        // POST: Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginDTO loginDTO)
        {
            if (ModelState.IsValid)
            {
                var user =
                    _loginBusiness.Login(loginDTO);

                if (user != null)
                {
                    Session["UserId"] =
                        user.UserId;

                    Session["Username"] =
                        user.Username;

                    Session["Role"] =
                        user.Role;


                    // Admin Login
                    if (user.Role.ToLower() == "admin")
                    {
                        return RedirectToAction(
                            "Index",
                            "AdminDashboard"
                        );
                    }


                    // Customer Login
                    if (user.Role.ToLower() == "customer")
                    {
                        return RedirectToAction(
                            "Index",
                            "CustomerDashboard"
                        );
                    }
                }

                ModelState.AddModelError(
                    "",
                    "Invalid username, password, or inactive user."
                );
            }

            return View(loginDTO);
        }


        // GET: Logout
        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction(
                "Index",
                "Login"
            );
        }
    }
}