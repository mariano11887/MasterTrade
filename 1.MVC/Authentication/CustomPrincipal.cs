using System.Linq;
using System.Security.Principal;

namespace MasterTrade.Authentication
{
    public class CustomPrincipal : IPrincipal
    {
        #region Identity Properties
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string[] Roles { get; set; }
        #endregion

        public IIdentity Identity
        {
            get; private set;
        }

        public CustomPrincipal(string username)
        {
            Identity = new GenericIdentity(username);
        }

        public bool IsInRole(string role)
        {
            return Roles.Any(r => role.Contains(r));
        }
    }
}