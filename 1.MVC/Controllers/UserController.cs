using _2.Service.Service;
using _4.DTO;
using _4.DTO.Enums;
using MasterTrade.Controllers.Base;
using MasterTrade.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace MasterTrade.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        // GET: User
        public ActionResult Index()
        {
            List<DTOUser> users = new ServiceUser().GetAllUsers();
            List<UserModel> usersModel = users.Select(u => new UserModel
            {
                Id = u.Id,
                Email = u.Email,
                RoleName = u.Role.Description,
                FirstName = u.FirstName,
                LastName = u.LastName
            }).ToList();

            return View(usersModel);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int id)
        {
            DTOUser user = new ServiceUser().GetUser(id);
            UserModel model = new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                RoleId = user.Role.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                AllRoles = new List<SelectListItem>
                {
                    new SelectListItem { Text = "Administrador", Value = ((int)Role.Administrator).ToString() },
                    new SelectListItem { Text = "Usuario Premium", Value = ((int)Role.PremiumUser).ToString() },
                    new SelectListItem { Text = "Usuario Gratuito", Value = ((int)Role.FreeUser).ToString() },
                    new SelectListItem { Text = "Deshabilitado", Value = ((int)Role.Disabled).ToString() }
                }
            };

            return View(model);
        }

        // POST: User/Edit/5
        [HttpPost]
        public ActionResult Edit(UserModel model)
        {
            try
            {
                DTOUser userDTO = new DTOUser
                {
                    Id = model.Id,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Role = new DTORole
                    {
                        Id = model.RoleId
                    }
                };
                
                new ServiceUser().SaveUser(userDTO);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(model);
            }
        }
    }
}
