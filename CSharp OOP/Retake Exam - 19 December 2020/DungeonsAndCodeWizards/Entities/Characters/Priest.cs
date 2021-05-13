using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Inventory;

namespace WarCroft.Entities.Characters
{
    public class Priest : Character, IHealer
    {
        private const double baseHealth = 50;
        private const double baseArmor = 25;
        private const double abilityPoints = 40;

        public Priest(string name)
            : base(name, baseHealth, baseArmor, abilityPoints, new Backpack())
        {
            Health = baseHealth;
            Armor = baseArmor;
        }

        public void Heal(Character character)
        {
            EnsureAlive();
            character.EnsureAlive();

            double healValue = character.Health + AbilityPoints;

            if (healValue >= character.BaseHealth)
            {
                character.Health = character.BaseHealth;
            }
            else
            {
                character.Health += AbilityPoints;
            }
        }
    }
}
