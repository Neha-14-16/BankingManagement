using System.Web;
using System.Web.Mvc;

namespace BankingManagement.Exceptions
{
    public class AdminAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(
            HttpContextBase httpContext)
        {
            if (httpContext.Session["UserId"] == null)
            {
                return false;
            }

            string role =
                httpContext.Session["Role"] as string;

            if (string.IsNullOrEmpty(role))
            {
                return false;
            }

            return role.ToLower() == "admin";
        }

        protected override void HandleUnauthorizedRequest(
            AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Session["UserId"] == null)
            {
                filterContext.Result =
                    new RedirectToRouteResult(
                        new System.Web.Routing.RouteValueDictionary
                        {
                            { "controller", "Login" },
                            { "action", "Index" }
                        });
            }
            else
            {
                filterContext.Result =
                    new HttpStatusCodeResult(403);
            }
        }
    }
}