namespace SharedTrip.Controllers
{
    using SharedTrip.Services.Trips;
    using SharedTrip.ViewModels.Trips;
    using SIS.HTTP;
    using SIS.MvcFramework;
    using System;
    using System.Globalization;

    public class TripsController : Controller
    {
        private readonly ITripsService tripsService;

        public TripsController(ITripsService tripsService)
        {
            this.tripsService = tripsService;
        }

        public HttpResponse All()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var model = this.tripsService.GetAll();

            return this.View(model);
        }

        public HttpResponse Add()
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            return this.View();
        }

        [HttpPost]
        public HttpResponse Add(TripAddModel model)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (string.IsNullOrWhiteSpace(model.StartPoint))
            {
                return this.Error("Starting Point is required!");
            }

            if (string.IsNullOrWhiteSpace(model.EndPoint))
            {
                return this.Error("End Point is required!");
            }

            if (!DateTime.TryParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                return this.Error("Invalid DateTime format!");
            }

            if (!Uri.TryCreate(model.ImagePath, UriKind.Absolute, out _))
            {
                return this.Error("Image URL is invalid!");
            }

            if (model.Seats < 2 || model.Seats > 6)
            {
                return this.Error("Seats shoud be between 2 and 6!");
            }

            if (string.IsNullOrWhiteSpace(model.Description) || model.Description.Length > 80)
            {
                return this.Error("Description is required and shoud be less than 80 characters!");
            }

            this.tripsService.AddTrip(model);

            return this.Redirect("/Trips/All");
        }

        public HttpResponse Details(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            var model = this.tripsService.GetDetails(tripId);

            return this.View(model);
        }

        public HttpResponse AddUserToTrip(string tripId)
        {
            if (!this.IsUserLoggedIn())
            {
                return this.Redirect("/Users/Login");
            }

            if (!this.tripsService.HasAvailableSeats(tripId))
            {
                return this.Error("No available seats!");
            }

            if (!this.tripsService.AddUserToTrip(tripId, this.User))
            {
                return this.Redirect($"/Trips/Details?tripId={tripId}");
            }

            return this.Redirect("/Trips/All");
        }
    }
}
