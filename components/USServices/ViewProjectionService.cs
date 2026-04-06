using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USRepositories;
using USModel;

namespace USServices
{
    /// <summary>
    /// Projects repository data for presentation in the view layer.
    ///
    /// This service provides a read-only facade over <see cref="IUserDbRepo"/>,
    /// exposing user data in a form suitable for UI consumption.
    /// </summary>
    public sealed class ViewProjectionService 
    {
        private IUserDbRepo Repository;
        public ViewProjectionService(IUserDbRepo repo) 
        {
            Repository = repo;
        }

        public List<User> GetAllUsers()
        {
            return Repository.GetAll();
        }
        public User? GetUser(string Username)
        {
            return Repository.Search(Username) ?? null;
        }
        public User? GetUser(int ID)
        {
            return Repository.Search(ID) ?? null;
        }
    }
}
