using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using USRepositories;
using USModel;

namespace USServices
{
    public sealed class UserAuthenticationService
    {
        private IUserDbRepo Repository;
        public int UserId { get; private set; }
        public UserAuthenticationService(IUserDbRepo repo)
        {
            Repository = repo;
        }

        public bool AuthenticateUser(string Username, string Password)
        {
            if (Repository.Search(Username) != null) 
            {
                UserId = Repository.Search(Username)!.ID; 
                return Repository.Search(Username)!.Password == Password;
            } // Checks user exists and password matches

            return false;
        }
    }
}
