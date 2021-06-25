using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace CounterStrike.Models.Maps
{
    public class Map : IMap
    {
        public string Start(ICollection<IPlayer> players)
        {
            List<IPlayer> terrorists = new List<IPlayer>();
            List<IPlayer> counterTerrorists = new List<IPlayer>();

            foreach (var player in players)
            {
                if (player.GetType().Name == "Terrorist")
                {
                    terrorists.Add(player);
                }
                else
                {
                    counterTerrorists.Add(player);
                }
            }

            while (terrorists.Any(x => x.Health > 0) && counterTerrorists.Any(x => x.Health > 0))
            {
                foreach (var terrorist in terrorists)
                {
                    foreach (var counterTerrorist in counterTerrorists)
                    {
                        if (counterTerrorist.IsAlive)
                        {
                            counterTerrorist.TakeDamage(terrorist.Gun.Fire());
                        }
                    }
                }

                foreach (var counterTerrorist in counterTerrorists)
                {
                    foreach (var terrorist in terrorists)
                    {
                        if (terrorist.IsAlive)
                        {
                            terrorist.TakeDamage(counterTerrorist.Gun.Fire());
                        }
                    }
                }
            }

            if (!terrorists.Any(x => x.IsAlive))
            {
                return $"Counter Terrorist wins!";
            }

            return $"Terrorist wins!";
        }
    }
}
