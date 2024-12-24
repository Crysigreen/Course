using Course.Data;
using Course.Models;
using Course.Services.Contarcts;

namespace Course.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context) => _context = context;

        public IEnumerable<User> GetAllUsers() => _context.Users.ToList();
        public User GetUserById(string id) => _context.Users.Find(id);
        public void AddUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }
        public void UpdateUser(string id, User updatedUser)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                user.FirstName = updatedUser.FirstName;
                user.LastName = updatedUser.LastName;
                user.Email = updatedUser.Email;
                user.DateOfBirth = updatedUser.DateOfBirth;
                _context.SaveChanges();
            }
        }
        public void DeleteUser(string id)
        {
            var user = GetUserById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }
    }
}
