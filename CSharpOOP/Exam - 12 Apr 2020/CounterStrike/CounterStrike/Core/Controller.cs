using CounterStrike.Core.Contracts;
using CounterStrike.Models.Guns;
using CounterStrike.Models.Guns.Contracts;
using CounterStrike.Models.Maps;
using CounterStrike.Models.Maps.Contracts;
using CounterStrike.Models.Players;
using CounterStrike.Models.Players.Contracts;
using CounterStrike.Repositories;
using CounterStrike.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CounterStrike.Core
{
    public class Controller : IController
    {
        private IRepository<IGun> guns = new GunRepository();
        private IRepository<IPlayer> players = new PlayerRepository();
        private IMap map = new Map();

        public string AddGun(string type, string name, int bulletsCount)
        {
            if (type == "Pistol")
            {
                guns.Add(new Pistol(name, bulletsCount));
            }
            else if (type == "Rifle")
            {
                guns.Add(new Rifle(name, bulletsCount));
            }
            else
            {
                throw new ArgumentException("Invalid gun type!");
            }

            return $"Successfully added gun {name}.";
        }

        public string AddPlayer(string type, string username, int health, int armor, string gunName)
        {
            IGun gun = guns.FindByName(gunName);

            if (gun == null)
            {
                throw new ArgumentException("Gun cannot be found!");
            }

            if (type == "Terrorist")
            {
                players.Add(new Terrorist(username, health, armor, gun));
            }
            else if (type == "CounterTerrorist")
            {
                players.Add(new CounterTerrorist(username, health, armor, gun));
            }
            else
            {
                throw new ArgumentException("Invalid player type!");
            }

            return $"Successfully added player {username}.";
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var player in players.Models.OrderBy(x => x.GetType().Name).ThenByDescending(x => x.Health).ThenBy(x => x.Username))
            {
                sb.AppendLine(player.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string StartGame()
        {
            return map.Start((List<IPlayer>)players.Models);
        }
    }
}
