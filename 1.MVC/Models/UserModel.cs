using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Mvc;

namespace MasterTrade.Models
{
    public class UserModel
    {
        public int Id { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Rol")]
        public int RoleId { get; set; }

        [DisplayName("Rol")]
        public string RoleName { get; set; }

        [DisplayName("Nombre")]
        public string FirstName { get; set; }

        [DisplayName("Apellido")]
        public string LastName { get; set; }

        public List<SelectListItem> AllRoles { get; set; }
    }
}