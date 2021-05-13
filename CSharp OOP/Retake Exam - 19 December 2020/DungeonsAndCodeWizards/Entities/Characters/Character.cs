using System;
using WarCroft.Constants;
using WarCroft.Entities.Inventory;
using WarCroft.Entities.Items;

namespace WarCroft.Entities.Characters.Contracts
{
    public abstract class Character
    {
        private string name;

        protected Character(string name, double baseHealth, double baseArmor, double abilityPoints, Bag bag)
        {
            Name = name;
            BaseHealth = baseHealth;
            BaseArmor = baseArmor;
            AbilityPoints = abilityPoints;
            Bag = bag;
        }

        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException(ExceptionMessages.CharacterNameInvalid);
                }

                name = value;
            }
        }

        public double BaseHealth { get; set; }

        public double Health { get; set; }

        public double BaseArmor { get; set; }

        public bool IsAlive { get; set; } = true;

        public double Armor { get; set; }

        public double AbilityPoints { get; set; }

        public Bag Bag { get; set; }

        public void EnsureAlive()
        {
            if (!this.IsAlive)
            {
                throw new InvalidOperationException(ExceptionMessages.AffectedCharacterDead);
            }
        }

        public void TakeDamage(double hitPoints)
        {
            EnsureAlive();

            double damage = Armor - hitPoints;

            if (damage < 0)
            {
                Armor = 0;

                if (Health + damage <= 0)
                {
                    Health = 0;
                    IsAlive = false;
                }
                else
                {
                    Health += damage;
                }
            }
            else
            {
                Armor -= hitPoints;
            }
        }

        public void UseItem(Item item)
        {
            item.AffectCharacter(this);
        }

        public override string ToString()
        {
            string status = "Alive";

            if (!IsAlive)
            {
                status = "Dead";
            }

            return string.Format(SuccessMessages.CharacterStats, Name, Health, BaseHealth, Armor, BaseArmor, status);
        }
    }
}