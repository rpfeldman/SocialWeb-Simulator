using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using USModel;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Metadata;

namespace USRepositories
{
    public sealed class EntityFramework_UserRepository : IUserDbRepo
    {
        private UserDbContext _context;

        public EntityFramework_UserRepository(UserDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Computes an integer ID to assign to a new <see cref="User"/>.
        /// </summary>
        private int IdSetter()
        {
            int IdCount;

            IdCount = _context.Users.Count();
           
            while (Search(IdCount) != null)
            {
                IdCount++;
            }

            return IdCount;
        }

        public bool AddToDB(string Username, string Password)
        {
            Username = RepoInnerServices.CharController(_context.UsernameCharLimits[0], false)(Username) || RepoInnerServices.CharController(_context.UsernameCharLimits[1], true)(Username) ? throw new Exception($"Invalid {nameof(Username)}, the lenght must be between {_context.UsernameCharLimits[0]} and {_context.UsernameCharLimits[1]} characters") : Username;
            Password = RepoInnerServices.CharController(_context.PasswordCharLimits[0], false)(Password) || RepoInnerServices.CharController(_context.PasswordCharLimits[1], true)(Password) ? throw new Exception($"Invalid {nameof(Password)}, the lenght must be between {_context.PasswordCharLimits[0]} and {_context.PasswordCharLimits[1]} characters") : Password;

            _context.Users.Add(new User { ID = IdSetter(), Username = Username, Password = Password, Admin = false });
            return _context.SaveChanges() > 0;
        }

        public bool ClearDB()
        {
            _context.Users.ToList().Clear();
            return _context.SaveChanges() > 0;
        }

        public bool Find(User User)
        {
            return _context.Users.Find(User.ID) != null;
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public bool RemoveFromDB(User User)
        {
            _context.Remove(User);
            return _context.SaveChanges() > 0;
        }

        public User? Search(string Username)
        {
            return _context.Users.Where(i => i.Username == Username).FirstOrDefault();
        }

        public User? Search(int ID)
        {
            return _context.Users.Find(ID);
        }

        public bool UpdateFromDB(User User, string Username, string Password)
        {
            var user = _context.Users.Find(User.ID) ?? throw new Exception("non-existent user");

            Username = RepoInnerServices.CharController(_context.UsernameCharLimits[0], false)(Username) || RepoInnerServices.CharController(_context.UsernameCharLimits[1], true)(Username) ? throw new Exception($"Invalid {nameof(Username)}, the lenght must be between {_context.UsernameCharLimits[0]} and {_context.UsernameCharLimits[1]} characters") : Username;
            Password = RepoInnerServices.CharController(_context.PasswordCharLimits[0], false)(Password) || RepoInnerServices.CharController(_context.PasswordCharLimits[1], true)(Password) ? throw new Exception($"Invalid {nameof(Password)}, the lenght must be between {_context.PasswordCharLimits[0]} and {_context.PasswordCharLimits[1]} characters") : Password;

            user.Username = Username; 
            user.Password = Password;

            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }
    }

    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public int[] UsernameCharLimits, PasswordCharLimits;
        public UserDbContext(DbContextOptions options, int[] UserCharLimits, int[] PassCharLimits) : base(options)
        {
            UsernameCharLimits = UserCharLimits;
            PasswordCharLimits = PassCharLimits;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().Property(u => u.ID).ValueGeneratedNever();
            modelBuilder.Entity<User>().HasKey(u => u.ID);
        }
    }
}
