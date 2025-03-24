using SpendingManagement.Models;
using SpendingManagement.Repositories;

namespace SpendingManagement.Services
{
    public class UserService
    {
        private UserRepository userRepository;
        public UserService(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public User ValidateUser(string email, string password)
        {
            return userRepository.ValidateUser(email, password);
        }
        public User GetUserById(int userId)
        {
            return userRepository.GetUserById(userId);
        }
        public void UpdateUsername(int userId, string newUsername)
        {
            userRepository.UpdateUsername(userId, newUsername);
        }
        public bool ChangePassword(int userId, string newPassword)
        {
            return userRepository.ChangePassword(userId, newPassword);
        }
        public bool RegisterUser(User user)
        {
            return userRepository.RegisterUser(user);
        }
    }
}
