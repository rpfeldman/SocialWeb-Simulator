using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Metadata;

namespace Repositories
{
    public class EntityFramework_UserRepository : IUserDbRepo
    {
        private UserDbContext _context;

        public EntityFramework_UserRepository(UserDbContext context)
        {
            _context = context;
        }

        private int IdSetter()
        {
            int IdCount;

            IdCount = _context.Usuarios.Count();
           
            while (Search(IdCount) != null)
            {
                IdCount++;
            }

            return IdCount;
        }
        private Predicate<string> CharController(int Limit, bool Greather)
        {
            return t => Greather ? t.Length >= Limit : t.Length <= Limit;
        }


        public bool AddToDB(string Username, string Password)
        {
            Username = CharController(_context.UsernameCharLimits[0], false)(Username) || CharController(_context.UsernameCharLimits[1], true)(Username) ? throw new Exception($"Invalid {nameof(Username)}, the lenght must be between {_context.UsernameCharLimits[0]} and {_context.UsernameCharLimits[1]} characters") : Username;
            Password = CharController(_context.PasswordCharLimits[0], false)(Password) || CharController(_context.PasswordCharLimits[1], true)(Password) ? throw new Exception($"Invalid {nameof(Password)}, the lenght must be between {_context.PasswordCharLimits[0]} and {_context.PasswordCharLimits[1]} characters") : Password;

            _context.Usuarios.Add(new User { ID = IdSetter(), Username = Username, Password = Password, Admin = false });
            return _context.SaveChanges() > 0;
        }

        public bool ClearDB()
        {
            _context.Usuarios.ToList().Clear();
            return _context.SaveChanges() > 0;
        }

        public bool Find(User User)
        {
            return _context.Usuarios.Find(User.ID) != null;
        }

        public List<User> GetAll()
        {
            return _context.Usuarios.ToList();
        }

        public bool RemoveFromDB(User User)
        {
            _context.Remove(User);
            return _context.SaveChanges() > 0;
        }

        public User? Search(string Username)
        {
            return _context.Usuarios.Where(i => i.Username == Username).FirstOrDefault();
        }

        public User? Search(int ID)
        {
            return _context.Usuarios.Find(ID);
        }

        public bool UpdateFromDB(User User, string Username, string Password)
        {
            var user = _context.Usuarios.Find(User.ID) ?? throw new Exception("non-existent user");

            Username = CharController(_context.UsernameCharLimits[0], false)(Username) || CharController(_context.UsernameCharLimits[1], true)(Username) ? throw new Exception($"Invalid {nameof(Username)}, the lenght must be between {_context.UsernameCharLimits[0]} and {_context.UsernameCharLimits[1]} characters") : Username;
            Password = CharController(_context.PasswordCharLimits[0], false)(Password) || CharController(_context.PasswordCharLimits[1], true)(Password) ? throw new Exception($"Invalid {nameof(Password)}, the lenght must be between {_context.PasswordCharLimits[0]} and {_context.PasswordCharLimits[1]} characters") : Password;

            user.Username = Username; 
            user.Password = Password;

            _context.Usuarios.Update(user);
            return _context.SaveChanges() > 0;
        }
    }

    public class UserDbContext : DbContext
    {
        public DbSet<User> Usuarios { get; set; }
        public int[] UsernameCharLimits, PasswordCharLimits;
        public UserDbContext(DbContextOptions options, int[] UserCharLimits, int[] PassCharLimits) : base(options)
        {
            UsernameCharLimits = UserCharLimits;
            PasswordCharLimits = PassCharLimits;
        }
    }
}
