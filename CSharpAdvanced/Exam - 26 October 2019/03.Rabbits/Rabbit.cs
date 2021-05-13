using System;
using System.Collections.Generic;
using System.Text;

namespace Rabbits
{
    public class Rabbit
    {
        public string Name { get; set; }
        public string Species { get; set; }
        public bool Available { get; set; }

        public Rabbit(string name,string species)
        {
            Available = true;
            Name = name;
            Species = species;
        }

        public override string ToString()
        {
            return $"Rabbit ({Species}): {Name}";
        }
    }
}
