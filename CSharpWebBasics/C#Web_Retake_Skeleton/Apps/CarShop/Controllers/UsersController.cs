namespace CarShop.Controllers
{
    using CarShop.Services;
    using CarShop.ViewModels.Users;
    using SUS.HTTP;
    using SUS.MvcFramework;
    using System.ComponentModel.DataAnnotations;

    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public HttpResponse Login()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Login(string username, string password)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            var userId = this.usersService.GetUserId(username, password);

            if (userId == null)
            {
                return this.Error("Invalid password or username!");
            }

            this.SignIn(userId);

            return this.Redirect("/Cars/All");
        }

        public HttpResponse Register()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Register(UserRegisterInputModel input)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (input.Password != input.ConfirmPassword)
            {
                return this.Error("Passwords do not match!");
            }

            if (string.IsNullOrWhiteSpace(input.Username) || input.Username.Length < 4 || input.Username.Length > 20)
            {
                return this.Error("Username is required and should be between 4 and 20 characters!");
            }

            if (string.IsNullOrWhiteSpace(input.Email))
            {
                return this.Error("Email address is required!");
            }

            if (!new EmailAddressAttribute().IsValid(input.Email))
            {
                return this.Error("Email address is not valid");
            }

            if (string.IsNullOrWhiteSpace(input.Password) || input.Password.Length < 4 || input.Password.Length > 20)
            {
                return this.Error("Password is required and should be between 5 and 20 characters!");
            }

            if (!this.usersService.IsUsernameAvailable(input.Username))
            {
                return this.Error("Entered Username already exists! Please try again with different username!");
            }

            if (!this.usersService.IsEmailAvailable(input.Email))
            {
                return this.Error("Entered email already exists! Please try again with different email!");
            }

            this.usersService.Create(input);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            this.SignOut();

            return this.Redirect("/");
        }
    }
}
