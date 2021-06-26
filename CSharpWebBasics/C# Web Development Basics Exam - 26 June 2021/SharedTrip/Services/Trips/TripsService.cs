namespace SharedTrip.Services.Trips
{
    using SharedTrip.Data.Models;
    using SharedTrip.ViewModels.Trips;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    public class TripsService : ITripsService
    {
        private readonly ApplicationDbContext db;

        public TripsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddTrip(TripAddModel model)
        {
            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                ImagePath = model.ImagePath,
                Seats = model.Seats,
                Description = model.Description
            };

            this.db.Trips.Add(trip);

            this.db.SaveChanges();
        }

        public bool AddUserToTrip(string tripId, string userId)
        {
            var userTrip = this.db.UsersTrips.FirstOrDefault(x => x.UserId == userId && x.TripId == tripId);

            if (userTrip == null)
            {
                this.db.UsersTrips.Add(new UserTrip
                {
                    UserId = userId,
                    TripId = tripId
                });

                this.db.SaveChanges();

                return true;
            }

            return false;
        }

        public IEnumerable<TripViewModel> GetAll()
        {
            return this.db.Trips
                .Select(x => new TripViewModel
                {
                    Id = x.Id,
                    StartPoint = x.StartPoint,
                    EndPoint = x.EndPoint,
                    DepartureTime = x.DepartureTime.ToString("G"),
                    Seats = x.Seats - x.UserTrips.Count
                })
                .ToList();
        }

        public TripDetailsViewModel GetDetails(string tripId)
        {
            return this.db.Trips.Where(x => x.Id == tripId)
             .Select(x => new TripDetailsViewModel
             {
                 Id = x.Id,
                 StartPoint = x.StartPoint,
                 EndPoint = x.EndPoint,
                 DepartureTime = x.DepartureTime.ToString("s"),
                 Seats = x.Seats - x.UserTrips.Count,
                 ImagePath = x.ImagePath,
                 Description = x.Description
             })
             .FirstOrDefault();
        }

        public bool HasAvailableSeats(string tripId)
        {
            return this.db.Trips.Any(x => x.Id == tripId && x.Seats - x.UserTrips.Count > 0);
        }
    }
}
