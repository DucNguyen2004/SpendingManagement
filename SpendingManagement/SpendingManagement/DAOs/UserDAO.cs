using SpendingManagement.Models;

namespace SpendingManagement.DAOs
{
    public class UserDAO
    {
        private SpendingManagementContext context;
        public UserDAO(SpendingManagementContext context)
        {
            this.context = context;
        }
        public User ValidateUser(string email, string password)
        {
            var user = context.Users.FirstOrDefault(u => u.Email.Equals(email) && u.Password.Equals(password));

            if (user != null)
            {
                return user;
            }

            return null;
        }

        public User GetUserById(int userId)
        {
            return context.Users.Find(userId);
        }
        public void UpdateUsername(int userId, string newUsername)
        {
            var user = context.Users.Find(userId);
            if (user != null)
            {
                user.Username = newUsername;
                context.SaveChanges();
            }
        }
        public bool ChangePassword(int userId, string newPassword)
        {
            var user = context.Users.Find(userId);
            if (user == null)
            {
                return false; // Incorrect password
            }

            user.Password = newPassword;
            context.SaveChanges();
            return true;
        }
        public bool RegisterUser(User user)
        {
            if (context.Users.Any(u => u.Email == user.Email))
            {
                return false; // Email already exists
            }

            context.Users.Add(user);
            context.SaveChanges();
            return true;
        }
    }
}
