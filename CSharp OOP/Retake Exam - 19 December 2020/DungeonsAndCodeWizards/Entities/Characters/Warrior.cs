using System;
using WarCroft.Constants;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Inventory;

namespace WarCroft.Entities.Characters
{
    public class Warrior : Character, IAttacker
    {
        private const double baseHealth = 100;
        private const double baseArmor = 50;
        private const double abilityPoints = 40;

        public Warrior(string name)
            : base(name, baseHealth, baseArmor, abilityPoints, new Satchel())
        {
            Health = baseHealth;
            Armor = baseArmor;
        }

        public void Attack(Character character)
        {
            EnsureAlive();
            character.EnsureAlive();

            if (Name == character.Name)
            {
                throw new InvalidOperationException(ExceptionMessages.CharacterAttacksSelf);
            }

            character.TakeDamage(AbilityPoints);
        }
    }
}
