using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.Data.SqlClient;
using Dapper;
using System.Dynamic;

namespace Repositories
{
    public class Dapper_UserRepository : IUserDbRepo
    {
        private string _conecctionString;
        private string _tablename;
        private SqlConnection? _conecction;
        public Dapper_UserRepository(string ConecctionString, string TableName)
        {
            _tablename = TableName;
            _conecctionString = ConecctionString;
            _conecction = new SqlConnection(ConecctionString);
        }

        private int IdSetter()
        {
            string query = $@"select count(*) from {_tablename}";
            int IdCount = _conecction!.ExecuteScalar<int>(query);

            while(Search(IdCount) != null)
            {
                IdCount++;
            };

            return IdCount;
        }

        public bool AddToDB(string Username, string Password)
        {
            int ID = IdSetter();

            string query = $@"insert into {_tablename}(Username, Password, Id, Admin) values(@username, @password, @id, @admin)";

            return _conecction!.Execute(query, new { username = Username, password = Password, id = ID, admin = 0 }) > 0; 
        }

        public bool ClearDB()
        {
            string query = $@"delete from {_tablename}";
            return _conecction!.Execute(query) > 0;
        }

        public bool Find(User User)
        {
            string query = $@"select * from {_tablename} where Id = {User.ID}";
            var list = _conecction!.Query<User>(query);
            return list.Any();
        }

        public List<User> GetAll()
        {
            string query = $@"select * from {_tablename}";
            var list = _conecction!.Query<User>(query);
            return list.ToList();
        }

        public bool RemoveFromDB(User User)
        {
            string query = $@"delete from {_tablename} where Id = {User.ID}";
            return _conecction!.Execute(query) > 0;
        }

        public User? Search(string Username)
        {
            string query = $@"select * from {_tablename}";
            return _conecction!.Query<User>(query).Where(i => i.Username == Username).FirstOrDefault();
        }

        public User? Search(int ID)
        {
            string query = $@"select * from {_tablename}";
            return _conecction!.Query<User>(query).Where(i => i.ID == ID).FirstOrDefault();
        }

        public bool UpdateFromDB(User User, string Username, string Password)
        {
            string query = $@"update {_tablename} set Username = '{Username}', Password = '{Password}' where Id = {User.ID}";
            return _conecction!.Execute(query) > 0;
        }
    }
}
