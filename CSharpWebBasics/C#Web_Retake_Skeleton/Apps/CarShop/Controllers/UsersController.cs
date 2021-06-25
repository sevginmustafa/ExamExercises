using CarShop.Services;
using CarShop.ViewModels.Users;
using SUS.HTTP;
using SUS.MvcFramework;
using System.ComponentModel.DataAnnotations;

namespace CarShop.Controllers
{
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

            return this.Redirect("/");
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
        public HttpResponse Register(UserRegisterInputModel model)
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return this.Error("Passwords do not match!");
            }

            if (string.IsNullOrWhiteSpace(model.Username) || model.Username.Length < 4 || model.Username.Length > 20)
            {
                return this.Error("Username is required and should be between 4 and 20 characters!");
            }

            if (string.IsNullOrWhiteSpace(model.Email))
            {
                return this.Error("Email address is required!");
            }

            if (!new EmailAddressAttribute().IsValid(model.Email))
            {
                return this.Error("Email address is not valid");
            }

            if (string.IsNullOrWhiteSpace(model.Password) || model.Password.Length < 4 || model.Password.Length > 20)
            {
                return this.Error("Password is required and should be between 5 and 20 characters!");
            }

            if (!this.usersService.IsUsernameAvailable(model.Username))
            {
                return this.Error("Entered Username already exists! Please try again with different username!");
            }

            if (!this.usersService.IsEmailAvailable(model.Email))
            {
                return this.Error("Entered email already exists! Please try again with different email!");
            }

            this.usersService.Create(model);

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Logout()
        {
            if (this.IsUserSignedIn())
            {
                return this.Redirect("/");
            }

            this.SignOut();

            return this.Redirect("/");
        }
    }
}
