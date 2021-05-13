using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guild
{
    public class Guild
    {
        List<Player> players;
        public string Name { get; set; }
        public int Capacity { get; set; }

        public int Count { get { return players.Count; } }

        public Guild(string name, int capacity)
        {
            players = new List<Player>();
            Name = name;
            Capacity = capacity;
        }

        public void AddPlayer(Player player)
        {
            if (Capacity > players.Count)
            {
                players.Add(player);
            }
        }

        public bool RemovePlayer(string name)
        {
            Player toRemove = players.FirstOrDefault(x => x.Name == name);

            if (toRemove == null)
            {
                return false;
            }

            players.Remove(toRemove);

            return true;
        }

        public void PromotePlayer(string name)
        {
            Player player = players.FirstOrDefault(x => x.Name == name);

            if (player.Rank != "Member")
            {
                player.Rank = "Member";
            }
        }

        public void DemotePlayer(string name)
        {
            Player player = players.FirstOrDefault(x => x.Name == name);

            if (player.Rank != "Trial")
            {
                player.Rank = "Trial";
            }
        }

        public Player[] KickPlayersByClass(string classs)
        {
            Player[] playersToRemove = players.Where(x => x.Class == classs).ToArray();
            players.RemoveAll(x => x.Class == classs);

            return playersToRemove;
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Players in the guild: {Name}");

            foreach (var player in players)
            {
                sb.AppendLine(player.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
