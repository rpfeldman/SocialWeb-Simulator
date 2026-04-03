using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USModel
{
    /// <summary>
    /// Represents the entity user in the database
    /// </summary>
    public class User
    {
        private int _id;
        private string _username;
        private string _password;
        public User()
        {
            _username = string.Empty;
            _password = string.Empty;
        }
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value ?? throw new ArgumentNullException(nameof(value));
            }
        }
        public int ID { get { return _id; } set { _id = value; } }

        public bool Admin { get; set; }
    }
}
