using CarShop.Services;
using CarShop.ViewModels.Cars;
using SUS.HTTP;
using SUS.MvcFramework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly ICarsService carsService;

        public CarsController(ICarsService carsService)
        {
            this.carsService = carsService;
        }

        public HttpResponse Add()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(AddCarInputModel model)
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.Model) || model.Model.Length < 4 || model.Model.Length > 20)
            {
                return this.Error("Model is required and should be between 5 and 20 characters!");
            }

            if (string.IsNullOrWhiteSpace(model.Image))
            {
                return this.Error("Image is required!");
            }

            if (Uri.TryCreate(model.Image, UriKind.Absolute, out _))
            {
                return this.Error("Invalid image URL!");
            }

            if (string.IsNullOrWhiteSpace(model.PlateNumber))
            {
                return this.Error("Plate Number is required!");
            }

            if (!Regex.IsMatch(model.PlateNumber, @"^[A-Z]{2}[0-9]{4}[A-Z]{2}$"))
            {
                return this.Error("Invalid Plate Number!");
            }

            this.carsService.AddCar(model);

            return this.Redirect("/Cards/All");
        }

        public HttpResponse All()
        {
            if (!this.IsUserSignedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }
    }
}
