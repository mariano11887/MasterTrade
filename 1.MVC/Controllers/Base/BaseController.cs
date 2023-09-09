using MasterTrade.Authentication;
using Microsoft.AspNet.Identity;
using System;
using System.Web.Mvc;
using System.Web.Security;

namespace MasterTrade.Controllers.Base
{
    public class BaseController : Controller
    {
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (User != null)
            {
                string username = User.Identity.Name;
                if (!string.IsNullOrEmpty(username) )
                {
                    CustomMembershipUser user = (CustomMembershipUser)Membership.GetUser(username);
                    string fullName = $"{user.FirstName} {user.LastName}";

                    ViewData.Add("FullName", fullName);
                }
            }

            base.OnActionExecuted(filterContext);
        }

        protected int GetUserId()
        {
            return Convert.ToInt32(User.Identity.GetUserId());
        }
    }
}