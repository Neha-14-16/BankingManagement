using System.Web.Mvc;
using BankingManagement.Exceptions;

namespace BankingManagement.Controllers
{
    [AdminAuthorize]
    public class AdminDashboardController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}