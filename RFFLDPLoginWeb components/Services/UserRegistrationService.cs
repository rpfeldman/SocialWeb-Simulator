using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using USModel;
using USRepositories;

namespace USServices
{
    public class UserRegistrationService
    {
        private IUserDbRepo Repository;
        public UserRegistrationService(IUserDbRepo repo)
        {
            Repository = repo;
        }

        /// <summary>
        /// Se encarga de agregar un nuevo usuario a la base de datos, realizando filtros y comprobaciones de datos antes de ejecutar la adicion
        /// </summary>
        /// <param name="Username"></param>
        /// <param name="Password"></param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        public bool RegisterUser(string Username, string Password)
        {
            Username = string.IsNullOrWhiteSpace(Username) ? throw new Exception($"{nameof(Username)} must be non-empty") : Username;
            Password = string.IsNullOrWhiteSpace(Password) ? throw new Exception($"{nameof(Password)} must be non-empty") : Password;

            if (Repository.Search(Username) == null) 
            {
                Password = Password.Replace(' ', '_');

                return Repository.AddToDB(Username, Password);
            } // Se comprueba que el nombre de usuario no exista aun

            throw new Exception("username must be one that is not already taken.");
        }
    }
}
