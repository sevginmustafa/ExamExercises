using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Parking
{
    public class Parking
    {
        private List<Car> cars;
        public string Type { get; set; }
        public int Capacity { get; set; }
        public int Count => cars.Count;

        public Parking(string type, int capacity)
        {
            cars = new List<Car>();
            Type = type;
            Capacity = capacity;
        }

        public void Add(Car car)
        {
            if (cars.Count < Capacity)
            {
                cars.Add(car);
            }
        }

        public bool Remove(string manufacturer, string model)
        {
            Car toRemove = cars.FirstOrDefault(x => x.Manufacturer == manufacturer && x.Model == model);

            if (toRemove == null)
            {
                return false;
            }

            cars.Remove(toRemove);

            return true;
        }

        public Car GetLatestCar()
        {
            if (cars.Count == 0)
            {
                return null;
            }

            return cars.OrderByDescending(x => x.Year).ToList()[0];
        }

        public Car GetCar(string manufacturer, string model)
        {
            return cars.FirstOrDefault(x => x.Manufacturer == manufacturer && x.Model == model);
        }

        public string GetStatistics()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"The cars are parked in {Type}:");

            foreach (var car in cars)
            {
                sb.AppendLine(car.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
