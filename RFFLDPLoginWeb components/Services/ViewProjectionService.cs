using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USRepositories;
using USModel;

namespace USServices
{
    public class ViewProjectionService 
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
