namespace SharedTrip.Controllers
{
    using SharedTrip.Services.Users;
    using SharedTrip.ViewModels.Users;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using System.ComponentModel.DataAnnotations;

    public class UsersController:Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            var user = this.usersService.GetUserId(username, password);

            if (user == null)
            {
                return this.Error("Invalid username or password!");
            }

            this.SignIn(user);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(UserRegisterModel model)
        {
            if (this.IsUserLoggedIn())
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 5 || model.Username.Length > 20)
            {
                return this.Error("Username is required and shoud be between 5 and 20 characters!");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return this.Error("Email is required!");
            }

            if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Email is not valid!");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 6 || model.Password.Length > 20)
            {
                return this.Error("Password is required and shoud be between 6 and 20 characters!");
            }

            if (!this.usersService.IsUsernameAvailable(model.Username))
            {
                return this.Error("Entered username already exists! Try again with different!");
            }

            if (!this.usersService.IsEmailAvailable(model.Email))
            {
                return this.Error("Entered email adress already exists! Try again with different!");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Passwords do not match!");
            }

            this.usersService.RegisterUser(model);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
