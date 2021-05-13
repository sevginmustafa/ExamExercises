using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rabbits
{
    public class Cage
    {
        List<Rabbit> data;

        public string Name { get; set; }
        public int Capacity { get; set; }

        public int Count { get { return data.Count; } }

        public Cage(string name, int capacity)
        {
            data = new List<Rabbit>();
            Name = name;
            Capacity = capacity;
        }

        public void Add(Rabbit rabbit)
        {
            if (Capacity > data.Count)
            {
                data.Add(rabbit);
            }
        }

        public bool RemoveRabbit(string name)
        {
            Rabbit toRemove = data.FirstOrDefault(x => x.Name == name);

            if (toRemove == null)
            {
                return false;
            }

            return true;
        }

        public void RemoveSpecies(string species)
        {
            data.RemoveAll(x => x.Species == species);
        }

        public Rabbit SellRabbit(string name)
        {
            Rabbit toSell = data.FirstOrDefault(x => x.Name == name);

            int toSellIndex = data.IndexOf(toSell);

            data[toSellIndex].Available = false;

            return toSell;
        }

        public Rabbit[] SellRabbitsBySpecies(string species)
        {
            Rabbit[] rabbitsToSell = data.Where(x => x.Species == species).ToArray();

            for (int i = 0; i < rabbitsToSell.Length; i++)
            {
                for (int j = 0; j < data.Count; j++)
                {
                    if (rabbitsToSell[i].Name == data[j].Name)
                    {
                        data[j].Available = false;
                        break;
                    }
                }
            }

            return rabbitsToSell;
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"Rabbits available at {Name}:");

            foreach (var rabbit in data.Where(x=>x.Available==true))
            {
                sb.AppendLine(rabbit.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
