using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USModel;

namespace USRepositories
{
    public class Dapper_UserRepository : IUserDbRepo
    {
        private string _conecctionString;
        private string _tablename;
        private SqlConnection? _conecction;
        int[] UsernameCharLimits;
        int[] PasswordCharLimits;
        public Dapper_UserRepository(string ConecctionString, string TableName, int[] UserCharLimits, int[] PassCharLimits)
        {
            _tablename = TableName;
            _conecctionString = ConecctionString;
            _conecction = new SqlConnection(ConecctionString);
            UsernameCharLimits = UserCharLimits;
            PasswordCharLimits = PassCharLimits;
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
            Username = RepoInnerServices.CharController(UsernameCharLimits[0], false)(Username) || RepoInnerServices.CharController(UsernameCharLimits[1], true)(Username) ? throw new Exception($"Invalid {nameof(Username)}, the lenght must be between {UsernameCharLimits[0]} and {UsernameCharLimits[1]} characters") : Username;
            Password = RepoInnerServices.CharController(PasswordCharLimits[0], false)(Password) || RepoInnerServices.CharController(PasswordCharLimits[1], true)(Password) ? throw new Exception($"Invalid {nameof(Password)}, the lenght must be between {PasswordCharLimits[0]} and {PasswordCharLimits[1]} characters") : Password;

            string query = $@"insert into {_tablename}(Username, Password, Id, Admin) values(@username, @password, @id, @admin)";

            return _conecction!.Execute(query, new { username = Username, password = Password, id = IdSetter(), admin = 0 }) > 0; 
        }

        public bool ClearDB()
        {
            string query = $@"delete from {_tablename}";
            return _conecction!.Execute(query) > 0;
        }

        public bool Find(User User)
        {
            return _conecction!.ExecuteScalar<int>($"SELECT COUNT(*) FROM {_tablename} where Id = {User.ID}") > 0;
        }

        public List<User> GetAll()
        {
            return _conecction!.Query<User>($@"select * from {_tablename}").ToList();
        }

        public bool RemoveFromDB(User User)
        {
            string query = $@"delete from {_tablename} where Id = {User.ID}";
            return _conecction!.Execute(query) > 0;
        }

        public User? Search(string Username)
        {
            return _conecction!.Query<User>($@"select * from {_tablename} where Username = '{Username}'").FirstOrDefault(); 
        }

        public User? Search(int ID)
        {
            return _conecction!.Query<User>($@"select * from {_tablename} where Id = {ID}").FirstOrDefault();
        }

        public bool UpdateFromDB(User User, string Username, string Password)
        {
            Username = RepoInnerServices.CharController(UsernameCharLimits[0], false)(Username) || RepoInnerServices.CharController(UsernameCharLimits[1], true)(Username) ? throw new Exception($"Invalid {nameof(Username)}, the lenght must be between {UsernameCharLimits[0]} and {UsernameCharLimits[1]} characters") : Username;
            Password = RepoInnerServices.CharController(PasswordCharLimits[0], false)(Password) || RepoInnerServices.CharController(PasswordCharLimits[1], true)(Password) ? throw new Exception($"Invalid {nameof(Password)}, the lenght must be between {PasswordCharLimits[0]} and {PasswordCharLimits[1]} characters") : Password;

            string query = $@"update {_tablename} set Username = '{Username}', Password = '{Password}' where Id = {User.ID}";
            return _conecction!.Execute(query) > 0;
        }
    }
}
