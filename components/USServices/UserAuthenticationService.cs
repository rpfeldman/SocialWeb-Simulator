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
    /// <summary>
    /// Service responsible for authenticating user logins.
    ///
    /// This service validates credentials against the supplied <see cref="IUserDbRepo"/>.
    /// Note: this project is a simulator and the authentication approach used here
    /// is not secure for production use; it exists only for simulation/testing purposes.
    /// </summary>
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
