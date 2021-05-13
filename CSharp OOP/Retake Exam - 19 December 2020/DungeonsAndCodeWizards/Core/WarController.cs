using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WarCroft.Constants;
using WarCroft.Entities.Characters;
using WarCroft.Entities.Characters.Contracts;
using WarCroft.Entities.Items;

namespace WarCroft.Core
{
    public class WarController
    {
        private readonly List<Character> party;
        private readonly List<Item> pool;

        public WarController()
        {
            party = new List<Character>();
            pool = new List<Item>();
        }

        public string JoinParty(string[] args)
        {
            string characterType = args[0];
            string name = args[1];

            Character character = null;

            if (characterType == "Warrior")
            {
                character = new Warrior(name);
            }
            else if (characterType == "Priest")
            {
                character = new Priest(name);
            }
            else
            {
                throw new ArgumentException(string.Format(ExceptionMessages.InvalidCharacterType, characterType));
            }

            party.Add(character);

            return string.Format(SuccessMessages.JoinParty, name);
        }

        public string AddItemToPool(string[] args)
        {
            string itemName = args[0];

            Item item = null;

            if (itemName == "FirePotion")
            {
                item = new FirePotion();
            }
            else if (itemName == "HealthPotion")
            {
                item = new HealthPotion();
            }
            else
            {
                throw new ArgumentException(string.Format(ExceptionMessages.InvalidItem, itemName));
            }

            pool.Add(item);

            return string.Format(SuccessMessages.AddItemToPool, itemName);
        }

        public string PickUpItem(string[] args)
        {
            string characterName = args[0];

            Character character = party.FirstOrDefault(x => x.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, characterName));
            }
            else if (pool.Count == 0)
            {
                throw new InvalidOperationException(ExceptionMessages.ItemPoolEmpty);
            }

            Item item = pool.Last();

            pool.Remove(item);

            character.Bag.AddItem(item);

            return string.Format(SuccessMessages.PickUpItem, characterName, item.GetType().Name);
        }

        public string UseItem(string[] args)
        {
            string characterName = args[0];
            string itemName = args[1];

            Character character = party.FirstOrDefault(x => x.Name == characterName);

            if (character == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, characterName));
            }

            Item toUse = character.Bag.GetItem(itemName);

            character.UseItem(toUse);

            return string.Format(SuccessMessages.UsedItem, characterName, itemName);
        }

        public string GetStats()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var character in party.OrderByDescending(x => x.IsAlive).ThenByDescending(x => x.Health))
            {
                sb.AppendLine(character.ToString());
            }

            return sb.ToString().TrimEnd();
        }

        public string Attack(string[] args)
        {
            string attackerName = args[0];
            string receiverName = args[1];

            Character attacker = party.FirstOrDefault(x => x.Name == attackerName);
            Character receiver = party.FirstOrDefault(x => x.Name == receiverName);

            if (attacker == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, attackerName));
            }

            if (receiver == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, receiverName));
            }

            if (attacker.GetType().Name != "Warrior")
            {
                throw new ArgumentException(string.Format(ExceptionMessages.AttackFail, attackerName));
            }

            ((Warrior)attacker).Attack(receiver);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(string.Format(SuccessMessages.AttackCharacter, attackerName, receiverName, attacker.AbilityPoints, receiverName,
                receiver.Health, receiver.BaseHealth, receiver.Armor, receiver.BaseArmor));

            if (receiver.IsAlive == false)
            {
                sb.AppendLine(string.Format(SuccessMessages.AttackKillsCharacter, receiverName));
            }

            return sb.ToString().TrimEnd();
        }

        public string Heal(string[] args)
        {
            string healerName = args[0];
            string healingReceiverName = args[1];

            Character healer = party.FirstOrDefault(x => x.Name == healerName);
            Character healingReceiver = party.FirstOrDefault(x => x.Name == healingReceiverName);

            if (healer == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, healerName));
            }

            if (healingReceiver == null)
            {
                throw new ArgumentException(string.Format(ExceptionMessages.CharacterNotInParty, healingReceiverName));
            }

            if (healer.GetType().Name != "Priest")
            {
                throw new ArgumentException(string.Format(ExceptionMessages.HealerCannotHeal, healerName));
            }

            ((Priest)healer).Heal(healingReceiver);

            return string.Format(SuccessMessages.HealCharacter, healerName, healingReceiverName,
                healer.AbilityPoints, healingReceiverName, healingReceiver.Health);
        }
    }
}
