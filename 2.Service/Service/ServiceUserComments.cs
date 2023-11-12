using _3.Repository;
using _3.Repository.Repository;
using System;

namespace _2.Service.Service
{
    public class ServiceUserComments
    {
        private readonly RepositoryUserComment repositoryUserComment;

        public ServiceUserComments()
        {
            repositoryUserComment = new RepositoryUserComment();
        }

        public void Save(int userId, string comment)
        {
            UserComment userComment = new UserComment()
            {
                Date = DateTime.Now,
                UserId = userId,
                Comments = comment
            };

            repositoryUserComment.Insert(userComment);
            repositoryUserComment.SaveChanges();
        }
    }
}
