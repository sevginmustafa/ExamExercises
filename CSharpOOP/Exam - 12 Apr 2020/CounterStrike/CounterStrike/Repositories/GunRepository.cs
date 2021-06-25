using CounterStrike.Models.Guns.Contracts;
using CounterStrike.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CounterStrike.Repositories
{
    public class GunRepository : IRepository<IGun>
    {
        public IReadOnlyCollection<IGun> Models { get; }

        public GunRepository()
        {
            Models = new List<IGun>();
        }

        public void Add(IGun model)
        {
            if (model == null)
            {
                throw new ArgumentException("Cannot add null in Gun Repository");
            }

            ((List<IGun>)Models).Add(model);
        }

        public IGun FindByName(string name)
        {
            return Models.FirstOrDefault(x => x.Name == name);
        }

        public bool Remove(IGun model)
        {
            return ((List<IGun>)Models).Remove(model);
        }
    }
}
