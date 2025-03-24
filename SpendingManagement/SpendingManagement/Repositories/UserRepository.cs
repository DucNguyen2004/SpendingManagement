using SpendingManagement.DAOs;
using SpendingManagement.Models;

namespace SpendingManagement.Repositories
{
    public class UserRepository
    {
        private UserDAO userDAO;

        public UserRepository(UserDAO userDAO)
        {
            this.userDAO = userDAO;
        }

        public User ValidateUser(string email, string password)
        {
            return userDAO.ValidateUser(email, password);
        }
        public User GetUserById(int userId)
        {
            return userDAO.GetUserById(userId);
        }
        public void UpdateUsername(int userId, string newUsername)
        {
            userDAO.UpdateUsername(userId, newUsername);
        }
        public bool ChangePassword(int userId, string newPassword)
        {
            return userDAO.ChangePassword(userId, newPassword);
        }
    }
}
