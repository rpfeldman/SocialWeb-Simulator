using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace USModel
{
    /// <summary>
    /// <summary>
    /// Represents the persisted user entity in the database.
    /// This type maps to a database table where the integer <see cref="ID"/> property is the primary key.
    /// </summary>
    public sealed class User
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
