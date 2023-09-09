using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using MasterTrade.Models;
using System.Web.Security;
using MasterTrade.Authentication;
using Newtonsoft.Json;
using _2.Service.Service;
using _4.DTO;
using MasterTrade.Controllers.Base;

namespace MasterTrade.Controllers
{
    [Authorize]
    public class AccountController : BaseController
    {
        public AccountController()
        {
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return LogOff();
            }

            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (Membership.ValidateUser(model.Email, model.Password))
                {
                    CustomMembershipUser user = (CustomMembershipUser)Membership.GetUser(model.Email, false);
                    if (user != null)
                    {
                        CustomSerializeModel userModel = new CustomSerializeModel
                        {
                            UserId = user.UserId,
                            Email = user.Email,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            RoleName = user.RoleName
                        };

                        string userData = JsonConvert.SerializeObject(userModel);
                        FormsAuthenticationTicket authenticationTicket = new FormsAuthenticationTicket(1, model.Email, DateTime.Now, DateTime.Now.AddYears(1), false, userData);
                        string encTicket = FormsAuthentication.Encrypt(authenticationTicket);
                        HttpCookie authCookie = new HttpCookie("AuthCookie", encTicket);
                        Response.Cookies.Add(authCookie);
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

            ModelState.AddModelError("", "Intento de inicio de sesión no válido.");
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            bool statusRegistration = false;
            string messageRegistration;

            if (ModelState.IsValid)
            {
                string userName = Membership.GetUserNameByEmail(model.Email);
                if (!string.IsNullOrEmpty(userName))
                {
                    ModelState.AddModelError("Advertencia email", "El email ingresado ya está registrado");
                    return View(model);
                }

                ServiceUser serviceUser = new ServiceUser();
                serviceUser.SaveUser(new DTOUser
                {
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password
                });

                messageRegistration = "Tu cuenta fue creada satisfactoriamente";
                statusRegistration = true;
            }
            else
            {
                messageRegistration = "Hubo un error";
            }

            ViewBag.Message = messageRegistration;
            ViewBag.Status = statusRegistration;

            return RedirectToAction("Index", "Login");
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            HttpCookie cookie = new HttpCookie("AuthCookie", "")
            {
                Expires = DateTime.Now.AddYears(-1)
            };
            Response.Cookies.Add(cookie);

            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Account");
        }
    }
}