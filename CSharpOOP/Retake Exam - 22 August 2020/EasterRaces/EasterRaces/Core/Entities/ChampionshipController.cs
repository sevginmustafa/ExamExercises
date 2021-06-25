using EasterRaces.Core.Contracts;
using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Models.Cars.Entities;
using EasterRaces.Models.Drivers.Contracts;
using EasterRaces.Models.Drivers.Entities;
using EasterRaces.Models.Races.Contracts;
using EasterRaces.Models.Races.Entities;
using EasterRaces.Repositories.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EasterRaces.Core.Entities
{
    public class ChampionshipController : IChampionshipController
    {
        private readonly CarRepository carRepository;
        private readonly DriverRepository driverRepository;
        private readonly RaceRepository raceRepository;

        public ChampionshipController()
        {
            carRepository = new CarRepository();
            driverRepository = new DriverRepository();
            raceRepository = new RaceRepository();
        }

        public string AddCarToDriver(string driverName, string carModel)
        {
            IDriver driver = driverRepository.GetByName(driverName);
            ICar car = carRepository.GetByName(carModel);

            if (driver == null)
            {
                throw new InvalidOperationException($"Driver {driverName} could not be found.");
            }
            if (car == null)
            {
                throw new InvalidOperationException($"Car {carModel} could not be found.");
            }

            driver.AddCar(car);

            return $"Driver {driverName} received car {carModel}.";
        }

        public string AddDriverToRace(string raceName, string driverName)
        {
            IRace race = raceRepository.GetByName(raceName);
            IDriver driver = driverRepository.GetByName(driverName);

            if (race == null)
            {
                throw new InvalidOperationException($"Race {raceName} could not be found.");
            }
            if (driver == null)
            {
                throw new InvalidOperationException($"Driver {driverName} could not be found.");
            }

            race.AddDriver(driver);

            return $"Driver {driverName} added in {raceName} race.";
        }

        public string CreateCar(string type, string model, int horsePower)
        {
            ICar car = carRepository.GetByName(model);

            if (car != null)
            {
                throw new ArgumentException($"Car {model} is already created.");
            }

            ICar createdCar = null;

            if (type == "Muscle")
            {
                createdCar = new MuscleCar(model, horsePower);
            }
            else
            {
                createdCar = new SportsCar(model, horsePower);
            }

            carRepository.Add(createdCar);

            return $"{createdCar.GetType().Name} {model} is created.";
        }

        public string CreateDriver(string driverName)
        {
            IDriver driver = driverRepository.GetByName(driverName);

            if (driver != null)
            {
                throw new ArgumentException($"Driver {driverName} is already created.");
            }

            driverRepository.Add(new Driver(driverName));

            return $"Driver {driverName} is created.";
        }

        public string CreateRace(string name, int laps)
        {
            IRace race = raceRepository.GetByName(name);

            if (race != null)
            {
                throw new InvalidOperationException($"Race {name} is already create.");
            }

            raceRepository.Add(new Race(name, laps));

            return $"Race {name} is created.";
        }

        public string StartRace(string raceName)
        {
            IRace race = raceRepository.GetByName(raceName);

            if (race == null)
            {
                throw new InvalidOperationException($"Race {raceName} could not be found.");
            }
            if (race.Drivers.Count < 3)
            {
                throw new InvalidOperationException($"Race {raceName} cannot start with less than 3 participants.");
            }

            IReadOnlyCollection<IDriver> drivers = driverRepository.GetAll();

            drivers = drivers.OrderByDescending(x => x.Car.CalculateRacePoints(race.Laps)).ToList();

            Console.WriteLine($"Driver {drivers.First().Name} wins {raceName} race.");
            Console.WriteLine($"Driver {drivers.Skip(1).First().Name} is second in {raceName} race.");
            Console.WriteLine($"Driver {drivers.Skip(2).First().Name} is third in {raceName} race.");

            raceRepository.Remove(race);

            return $"Driver {drivers.First().Name} wins {raceName} race." + Environment.NewLine +
                $"Driver {drivers.Skip(1).First().Name} is second in {raceName} race." + Environment.NewLine +
                $"Driver {drivers.Skip(2).First().Name} is third in {raceName} race.";
        }
    }
}
