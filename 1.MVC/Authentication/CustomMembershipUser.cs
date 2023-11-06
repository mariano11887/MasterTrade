using _4.DTO;
using System;
using System.Web.Security;

namespace MasterTrade.Authentication
{
    public class CustomMembershipUser : MembershipUser
    {
        #region User Properties

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RoleName { get; set; }
        public string[] Permissions { get; set; }

        #endregion

        public CustomMembershipUser(DTOUser user) : base("CustomMembership", user.Email, user.Id, user.Email, string.Empty, string.Empty, true, false, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now, DateTime.Now)
        {
            UserId = user.Id;
            FirstName = user.FirstName;
            LastName = user.LastName;
            RoleName = user.Role.Description;
            Permissions = user.Role.Permissions;
        }
    }
}