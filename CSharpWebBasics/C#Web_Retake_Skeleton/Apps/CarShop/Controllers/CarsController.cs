namespace CarShop.Controllers
{
    using CarShop.Services;
    using CarShop.ViewModels.Cars;
    using SUS.HTTP;
    using SUS.MvcFramework;
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    public class CarsController : Controller
    {
        private readonly ICarsService carsService;
        private readonly IUsersService usersService;

        public CarsController(ICarsService carsService, IUsersService usersService)
        {
            this.carsService = carsService;
            this.usersService = usersService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (this.usersService.IsUserMechanic(this.GetUserId()))
            {
                return this.Redirect("/");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddCarInputModel input)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (this.usersService.IsUserMechanic(this.GetUserId()))
            {
                return this.Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(input.Model) || input.Model.Length < 4 || input.Model.Length > 20)
            {
                return this.Error("Model is required and should be between 5 and 20 characters!");
            }

            if (string.IsNullOrWhiteSpace(input.Image))
            {
                return this.Error("Image is required!");
            }

            if (!Uri.TryCreate(input.Image, UriKind.Absolute, out _))
            {
                return this.Error("Invalid image URL!");
            }

            if (string.IsNullOrWhiteSpace(input.PlateNumber))
            {
                return this.Error("Plate Number is required!");
            }

            if (!Regex.IsMatch(input.PlateNumber, @"^[A-Z]{2}[0-9]{4}[A-Z]{2}$"))
            {
                return this.Error("Invalid Plate Number!");
            }

            this.carsService.AddCar(input, this.GetUserId());

            return this.Redirect("/Cars/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            IEnumerable<GetAllCarsOutputModel> model = null;

            if (this.usersService.IsUserMechanic(this.GetUserId()))
            {
                model = this.carsService.GetAllForMechanics();
            }
            else
            {
                model = this.carsService.GetAll(this.GetUserId());
            }

            return this.View(model);
        }
    }
}
