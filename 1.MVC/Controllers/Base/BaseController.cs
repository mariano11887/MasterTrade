using _4.DTO.Enums;
using MasterTrade.Authentication;
using System.Linq;
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

                    if (user.Permissions.Any(p => p == Permission.ManageStrategies))
                    {
                        ViewData.Add("MyStrategies", true);
                    }

                    if (user.Permissions.Any(p => p == Permission.Backtesting))
                    {
                        ViewData.Add("Backtesting", true);
                    }

                    if (user.Permissions.Any(p => p == Permission.BacktestingWithRanges))
                    {
                        ViewData.Add("BacktestingWithRanges", true);
                    }

                    if (user.Permissions.Any(p => p == Permission.ManageCryptoPairs))
                    {
                        ViewData.Add("CryptoPairs", true);
                    }

                    if (user.Permissions.Any(p => p == Permission.ManageUsers))
                    {
                        ViewData.Add("Users", true);
                    }
                }
            }

            base.OnActionExecuted(filterContext);
        }

        protected int GetUserId()
        {
            CustomMembershipUser user = (CustomMembershipUser)Membership.GetUser(User.Identity.Name);
            return user != null ? user.UserId : 0;
        }
    }
}