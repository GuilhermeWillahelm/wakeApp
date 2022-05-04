using wakeApp.Models;

namespace wakeApp.Repositories
{
    public interface IUsersRepository
    {
        User GetUserById(int? id);
        User CreateUser(User user);
        bool LoginUser(UserLogin userLogin);
        void Logoff();
        User EditUser(int id, User user);
        bool DeleteUser(int id);
        int GetUserId();
        string GetUserName();
    }
}
