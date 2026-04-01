using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace Repositories
{
    public interface IUserDbRepo
    {
        public bool AddToDB(string Username, string Password);
        public List<User> GetAll();
        public bool UpdateFromDB(User User, string Username, string Password);
        public bool RemoveFromDB(User User);
        public bool ClearDB();
        public bool Find(User User);
        public User? Search(string Username);
        public User? Search(int ID);
    }
}
