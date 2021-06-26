namespace SharedTrip.Services.Trips
{
    using SharedTrip.ViewModels.Trips;
    using System.Collections.Generic;

    public interface ITripsService
    {
        void AddTrip(TripAddModel model);

        IEnumerable<TripViewModel> GetAll();

        TripDetailsViewModel GetDetails(string tripId);

        bool AddUserToTrip(string tripId, string userId);

        bool HasAvailableSeats(string tripId);
    }
}
