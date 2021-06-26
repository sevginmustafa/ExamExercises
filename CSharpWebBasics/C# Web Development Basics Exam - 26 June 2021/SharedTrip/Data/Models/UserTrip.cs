﻿namespace SharedTrip.Data.Models
{
    public class UserTrip
    {
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public string TripId { get; set; }

        public virtual Trip Trio { get; set; }
    }
}
