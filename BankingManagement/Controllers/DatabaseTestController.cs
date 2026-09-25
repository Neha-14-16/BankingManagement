using System;
using System.Web.Mvc;
using BankingManagement.Data;

namespace BankingManagement.Controllers
{
    public class DatabaseTestController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                using (var db = new BankingDbContext())
                {
                    bool exists = db.Database.Exists();

                    if (exists)
                    {
                        return Content("Database connection successful.");
                    }

                    return Content("Database does not exist.");
                }
            }
            catch (Exception ex)
            {
                return Content("Database connection failed: " + ex.Message);
            }
        }
    }
}