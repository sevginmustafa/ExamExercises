namespace CarShop.Services
{
    using CarShop.Data;
    using CarShop.Data.Models;
    using CarShop.ViewModels.Users;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;

    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext db;

        public UsersService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void Create(UserRegisterInputModel input)
        {
            var user = new User
            {
                Username = input.Username,
                Email = input.Email,
                Password = ComputeHash(input.Password),
                IsMechanic = input.UserType == "Mechanic" ? true : false,
            };

            this.db.Users.Add(user);

            this.db.SaveChanges();
        }

        public string GetUserId(string username, string password)
        {
            return this.db.Users
                .FirstOrDefault(x => x.Username == username && x.Password == ComputeHash(password))?.Id;
        }

        public bool IsUserMechanic(string Userid)
        {
            return this.db.Users.Any(x => x.Id == Userid && x.IsMechanic);
        }

        public bool IsUsernameAvailable(string username)
        {
            return !this.db.Users.Any(x => x.Username == username);
        }

        public bool IsEmailAvailable(string email)
        {
            return !this.db.Users.Any(x => x.Email == email);
        }

        private static string ComputeHash(string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using var hash = SHA512.Create();
            var hashedInputBytes = hash.ComputeHash(bytes);
            var hashedInputStringBuilder = new StringBuilder(128);
            foreach (var b in hashedInputBytes)
                hashedInputStringBuilder.Append(b.ToString("X2"));
            return hashedInputStringBuilder.ToString();
        }
    }
}
