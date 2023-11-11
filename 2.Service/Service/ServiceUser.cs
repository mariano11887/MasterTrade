using _3.Repository;
using Enums = _4.DTO.Enums;
using _3.Repository.Repository;
using _4.DTO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

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
            User user = repositoryUser.GetQuery().FirstOrDefault(u => u.Email == email && u.Role.Permissions.Any(p => p.Name == Enums.Permission.Login));
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
                    Description = u.Role.Description,
                    PermissionsSet = u.Role.Permissions.Select(p => p.Name)
                }
            }).FirstOrDefault();

            return user;
        }

        public DTOUser GetUser(int id)
        {
            DTOUser user = repositoryUser.GetQuery().Where(u => u.Id == id).Select(u => new DTOUser
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = new DTORole
                {
                    Id = u.RoleId,
                    Description = u.Role.Description,
                    PermissionsSet = u.Role.Permissions.Select(p => p.Name)
                }
            }).FirstOrDefault();

            return user;
        }

        public List<DTOUser> GetAllUsers()
        {
            List<DTOUser> users = repositoryUser.GetQuery().Select(u => new DTOUser
            {
                Id = u.Id,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Role = new DTORole
                {
                    Id = u.RoleId,
                    Description = u.Role.Description,
                    PermissionsSet = u.Role.Permissions.Select(p => p.Name)
                }
            }).ToList();

            return users;
        }

        public void SaveUser(DTOUser userDTO)
        {
            User user = new User
            {
                Email = userDTO.Email,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Password = EncryptPassword(userDTO.Password),
                RoleId = (int)Enums.Role.FreeUser
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
