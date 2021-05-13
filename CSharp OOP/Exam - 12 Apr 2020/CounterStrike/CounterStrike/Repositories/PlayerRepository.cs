using CounterStrike.Models.Players.Contracts;
using CounterStrike.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CounterStrike.Repositories
{
    public class PlayerRepository : IRepository<IPlayer>
    {
        public IReadOnlyCollection<IPlayer> Models { get; }

        public PlayerRepository()
        {
            Models = new List<IPlayer>();
        }

        public void Add(IPlayer model)
        {
            if (model == null)
            {
                throw new ArgumentException("Cannot add null in Player Repository");
            }

             ((List<IPlayer>)Models).Add(model);
        }

        public IPlayer FindByName(string name)
        {
            return Models.FirstOrDefault(x => x.Username == name);
        }

        public bool Remove(IPlayer model)
        {
            return ((List<IPlayer>)Models).Remove(model);
        }
    }
}
