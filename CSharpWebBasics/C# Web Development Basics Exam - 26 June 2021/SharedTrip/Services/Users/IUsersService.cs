namespace SharedTrip.Services.Users
{
    using SharedTrip.ViewModels.Users;

    public interface IUsersService
    {
        void RegisterUser(UserRegisterModel model);

        string GetUserId(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
