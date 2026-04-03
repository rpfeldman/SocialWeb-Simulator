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

        public Test_UserRepository()
        {
            _UserList = new();
        }

        public bool AddToDB(string Username, string Password)
        {
            try
            {
                _UserList.Add(new User {ID = _UserList.Count+1, Username = Username, Password = Password });

                return true;
            }
            catch (Exception)
            {
                return false;
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
            }
        }

        public bool Find(User User)
        {
            return _UserList.Contains(User);
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
            }
        }

        public User? Search(string Username)
        {
            return _UserList.Where(i => i.Username == Username).FirstOrDefault();
        }
        public User? Search(int ID)
        {
            return _UserList.Where(i => i.ID == ID).FirstOrDefault();
        }

        public bool UpdateFromDB(User User, string Username, string Password)
        {
            try
            {
                _UserList.Remove(User);
                _UserList.Add(new User { ID = User.ID, Username = Username, Password = Password });

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
