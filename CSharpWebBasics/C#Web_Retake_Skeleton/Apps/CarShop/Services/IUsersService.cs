using CarShop.ViewModels.Users;

namespace CarShop.Services
{
    public interface IUsersService
    {
        string GetUserId(string username, string password);

        void Create(UserRegisterInputModel model);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);

        public bool IsUserMechanic(string Userid);
    }
}
