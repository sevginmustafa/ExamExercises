using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Heroes
{
    public class HeroRepository
    {
        List<Hero> heroes;

        public int Count { get { return heroes.Count; } }

        public HeroRepository()
        {
            heroes = new List<Hero>();
        }

        public void Add(Hero hero)
        {
            heroes.Add(hero);
        }

        public void Remove(string name)
        {
            Hero toRemove = heroes.FirstOrDefault(x => x.Name == name);

            heroes.Remove(toRemove);
        }

        public Hero GetHeroWithHighestStrength()
       => heroes.OrderByDescending(x => x.Item.Strength).ToList()[0];

        public Hero GetHeroWithHighestAbility()
       => heroes.OrderByDescending(x => x.Item.Ability).ToList()[0];

        public Hero GetHeroWithHighestIntelligence()
        => heroes.OrderByDescending(x => x.Item.Intelligence).ToList()[0];

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var hero in heroes)
            {
                sb.AppendLine(hero.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
