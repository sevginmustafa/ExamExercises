using EasterRaces.Models.Cars.Contracts;
using EasterRaces.Repositories.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace EasterRaces.Repositories.Entities
{
    public class CarRepository : IRepository<ICar>
    {
        private readonly List<ICar> cars;

        public CarRepository()
        {
            cars = new List<ICar>();
        }

        public void Add(ICar model)
        {
            cars.Add(model);
        }

        public IReadOnlyCollection<ICar> GetAll()
        {
            return cars;
        }

        public ICar GetByName(string name)
        {
            return cars.FirstOrDefault(x => x.Model == name);
        }

        public bool Remove(ICar model)
        {
            return cars.Remove(model);
        }
    }
}
