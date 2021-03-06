using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Races.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasterRaces.Models.Races.Entities
{
    public class Race : IRace
    {
        private string name;
        private int laps;

        public Race(string name, int laps)
        {
            Name = name;
            Laps = laps;
            Drivers = new List<IDriver>();
        }

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 5)
                {
                    throw new ArgumentException($"Name {value} cannot be less than 5 symbols.");
                }

                name = value;
            }
        }

        public int Laps
        {
            get
            {
                return laps;
            }
            private set
            {
                if (value < 1)
                {
                    throw new ArgumentException("Laps cannot be less than 1.");
                }

                laps = value;
            }
        }

        public IReadOnlyCollection<IDriver> Drivers { get; private set; }

        public void AddDriver(IDriver driver)
        {
            if (driver == null)
            {
                throw new ArgumentNullException("Driver cannot be null.");
            }
            else if (driver.Car == null)
            {
                throw new ArgumentException($"Driver {driver.Name} could not participate in race.");
            }
            else if (Drivers.Any(x => x.Name == driver.Name))
            {
                throw new ArgumentNullException($"Driver {driver.Name} is already added in {Name} race.");
            }

            ((ICollection<IDriver>)Drivers).Add(driver);
        }
    }
}
