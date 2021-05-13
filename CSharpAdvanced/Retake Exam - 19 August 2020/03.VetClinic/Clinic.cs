using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VetClinic
{
    public class Clinic
    {
        private List<Pet> pets;
        public int Capacity { get; set; }

        public int Count => pets.Count;

        public Clinic(int capacity)
        {
            Capacity = capacity;
            pets = new List<Pet>();
        }

        public void Add(Pet pet)
        {
            if (Capacity > pets.Count)
            {
                pets.Add(pet);
            }
        }

        public bool Remove(string name)
        {
            Pet toRemove = pets.FirstOrDefault(x => x.Name == name);

            if (toRemove == null)
            {
                return false;
            }

            pets.Remove(toRemove);

            return true;
        }

        public Pet GetPet(string name, string owner)
        {
            return pets.FirstOrDefault(x => x.Name == name && x.Owner == owner);
        }

        public Pet GetOldestPet()
        {
            return pets.OrderByDescending(x => x.Age).ToList()[0];
        }

        public string GetStatistics()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("The clinic has the following patients:");

            foreach (var pet in pets)
            {
                sb.AppendLine($"Pet {pet.Name} with owner: {pet.Owner}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}
