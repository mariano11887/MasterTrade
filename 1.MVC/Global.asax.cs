using MasterTrade.Authentication;
using MasterTrade.Common;
using Newtonsoft.Json;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;

namespace MasterTrade
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            ModelBinders.Binders.Add(typeof(decimal), new DecimalModelBinder());
            ModelBinders.Binders.Add(typeof(decimal?), new DecimalModelBinder());
        }

        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies["AuthCookie"];
            if (authCookie != null)
            {
                FormsAuthenticationTicket authTicket = FormsAuthentication.Decrypt(authCookie.Value);

                CustomSerializeModel serializeModel = JsonConvert.DeserializeObject<CustomSerializeModel>(authTicket.UserData);
                CustomPrincipal principal = new CustomPrincipal(authTicket.Name)
                {
                    UserId = serializeModel.UserId,
                    FirstName = serializeModel.FirstName,
                    LastName = serializeModel.LastName,
                    Roles = new string[] { serializeModel.RoleName }
                };

                HttpContext.Current.User = principal;
            }
        }
    }
}
