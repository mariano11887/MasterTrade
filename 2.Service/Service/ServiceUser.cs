using _3.Repository;
using Enums = _4.DTO.Enums;
using _3.Repository.Repository;
using _4.DTO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace _2.Service.Service
{
    public class ServiceUser
    {
        private readonly RepositoryUser repositoryUser;

        public ServiceUser() { 
            repositoryUser = new RepositoryUser();
        }

        public bool Authenticate(string email, string password)
        {
            User user = repositoryUser.GetQuery().FirstOrDefault(u => u.Email == email);
            if (user == null)
            {
                return false;
            }

            return user.Password == EncryptPassword(password);
        }

        public DTOUser GetUser(string email)
        {
            DTOUser user = repositoryUser.GetQuery().Where(u => u.Email == email).Select(u => new DTOUser
            {
                Id = u.Id,
                Email = email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = new DTORole
                {
                    Id = u.RoleId,
                    Description = u.Role.Description
                }
            }).FirstOrDefault();

            return user;
        }

        public void SaveUser(DTOUser userDTO)
        {
            User user = new User
            {
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Password = EncryptPassword(userDTO.Password),
                RoleId = (int)Enums.Role.RegularUser
            };

            repositoryUser.Insert(user);
            repositoryUser.SaveChanges();
        }

        private string EncryptPassword(string password)
        {
            byte[] data = Encoding.ASCII.GetBytes(password);
            data = new SHA256Managed().ComputeHash(data);

            return Encoding.ASCII.GetString(data);
        }
    }
}
