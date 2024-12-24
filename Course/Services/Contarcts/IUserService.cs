using Course.Models;

namespace Course.Services.Contarcts
{
    public interface IUserService
    {
        IEnumerable<User> GetAllUsers();
        User GetUserById(string id);
        void AddUser(User user);
        void UpdateUser(string id, User updatedUser);
        void DeleteUser(string id);
    }
}
