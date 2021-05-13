using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Christmas
{
    public class Bag
    {
        List<Present> presents;

        public string Color { get; set; }
        public int Capacity { get; set; }
        public int Count { get { return presents.Count; } }

        public Bag(string color, int capacity)
        {
            presents = new List<Present>();
            Color = color;
            Capacity = capacity;
        }

        public void Add(Present present)
        {
            if (Capacity > presents.Count)
            {
                presents.Add(present);
            }
        }

        public bool Remove(string name)
        {
            Present toRemove = presents.FirstOrDefault(x => x.Name == name);

            if (toRemove == null)
            {
                return false;
            }

            return true;
        }

        public Present GetHeaviestPresent()
        {
            return presents.OrderByDescending(x => x.Weight).ToList()[0];
        }

        public Present GetPresent(string name)
        {
            return presents.FirstOrDefault(x => x.Name == name);
        }

        public string Report()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{Color} bag contains:");

            foreach (var present in presents)
            {
                sb.AppendLine(present.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
