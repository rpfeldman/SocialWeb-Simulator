using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USModel;

namespace USRepositories
{
    public class Test_UserRepository : IUserDbRepo
    {
        private List<User> _UserList;
        private int IdCount = 0;

        public Test_UserRepository()
        {
            _UserList = new();
        }

        public bool AddToDB(string Username, string Password)
        {
            try
            {
                IdCount++;
                
                _UserList.Add(new User {ID = IdCount, Username = Username, Password = Password });

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool ClearDB()
        {
            try
            {
                _UserList.Clear();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public bool Find(User User)
        {
            Predicate<User> find = user => _UserList.Contains(user);
            return find(User);
        }

        public List<User> GetAll()
        {
            return _UserList;
        }

        public bool RemoveFromDB(User User)
        {
            try
            {
                _UserList.Remove(User);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        public User? Search(string Username)
        {
            IEnumerable<User> user = from i in _UserList where i.Username == Username select i;
            return user.FirstOrDefault();
        }
        public User? Search(int ID)
        {
            IEnumerable<User> user = from i in _UserList where i.ID == ID select i;
            return user.FirstOrDefault();
        }

        public bool UpdateFromDB(User User, string Username, string Password)
        {
            try
            {
                string username = string.IsNullOrEmpty(Username) ? throw new ArgumentNullException(nameof(Username)) : Username;
                string password = string.IsNullOrEmpty(Password) ? throw new ArgumentNullException(nameof(Password)) : Password;

                _UserList.Remove(User);
                _UserList.Add(new User { ID = User.ID, Username = username, Password = password });

                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
    }
}
