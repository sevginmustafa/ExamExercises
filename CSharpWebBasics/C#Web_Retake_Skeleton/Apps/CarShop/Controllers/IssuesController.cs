namespace CarShop.Controllers
{
    using CarShop.Services;
    using CarShop.ViewModels.Issues;
    using SUS.HTTP;
    using SUS.MvcFramework;

    public class IssuesController : Controller
    {
        private readonly IIssuesService issuesService;
        private readonly IUsersService usersService;

        public IssuesController(IIssuesService issuesService, IUsersService usersService)
        {
            this.issuesService = issuesService;
            this.usersService = usersService;
        }

        public HttpResponse CarIssues(string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var model = this.issuesService.GetAllCarIssues(carId);

            return this.View(model);
        }

        public HttpResponse Add(string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddIssueInpuModel input)
        {
            input.CarId = this.Request.QueryString.Split('=')[1];

            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(input.Description) || input.Description.Length < 5)
            {
                return this.Error("Description is required and cannot be less than 5 characters!");
            }

            this.issuesService.AddIssue(input);

            return this.Redirect($"/Issues/CarIssues?carId={input.CarId}");
        }

        public HttpResponse Delete(string issueId, string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            this.issuesService.DeleteIssue(issueId, carId);

            return this.Redirect($"/Issues/CarIssues?carId={carId}");
        }

        public HttpResponse Fix(string issueId, string carId)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (this.usersService.IsUserMechanic(this.GetUserId()))
            {
                this.issuesService.FixIssue(issueId, carId);
            }

            return this.Redirect($"/Issues/CarIssues?carId={carId}");
        }
    }
}
