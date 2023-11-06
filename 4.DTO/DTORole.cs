using System.Collections.Generic;
using System.Linq;

namespace _4.DTO
{
    public class DTORole
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> PermissionsSet { private get; set; }
        public string[] Permissions {
            get
            {
                return PermissionsSet.ToArray();
            }
        }
    }
}
