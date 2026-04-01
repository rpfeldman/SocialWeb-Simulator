using USModel;
using USRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USServices
{
    public class UserProfileService : IDisposable
    {
        private int _UserId;
        private IUserDbRepo Repository;

        public UserProfileService(int UserID, IUserDbRepo repo)
        {
            Repository = repo;

            if (Repository.Search(UserID) != null) { _UserId = UserID; return; } // Comprueba que la id corresponda a un usuario real en la db
            throw new Exception("UserId must be connected to a real existent user");
        }
        public bool Delete()
        {
            var User = Repository.Search(_UserId);
            return Repository.RemoveFromDB(User!);
        }

        public void Dispose()
        {
           
        }

        public bool Update(string Username, string Password)
        {
            var User = Repository.Search(_UserId);

            if (Repository.Search(Username) == null) 
            {
                string _username = string.IsNullOrWhiteSpace(Username) ? User!.Username : Username;
                string _password = string.IsNullOrWhiteSpace(Password) ? User!.Password : Password;
                _password = _password.Replace(' ', '_');

                return Repository.UpdateFromDB(User!, _username, _password);
            } // Se comprueba que el nuevo nombre de usuario no este en uso

            throw new Exception("username must be one that is not already taken.");
        }
    }
}
