namespace CarShop.Services
{
    using CarShop.ViewModels.Users;

    public interface IUsersService
    {
        string GetUserId(string username, string password);

        void Create(UserRegisterInputModel input);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);

        public bool IsUserMechanic(string Userid);
    }
}
