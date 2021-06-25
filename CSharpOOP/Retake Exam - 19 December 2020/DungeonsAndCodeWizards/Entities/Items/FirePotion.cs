using WarCroft.Entities.Characters.Contracts;

namespace WarCroft.Entities.Items
{
    public class FirePotion : Item
    {
        private const int weight = 5;

        public FirePotion()
            : base(weight)
        {
        }

        public override void AffectCharacter(Character character)
        {
            base.AffectCharacter(character);

            double damage = character.Health - 20;

            if (damage <= 0)
            {
                character.Health = 0;
                character.IsAlive = false;
            }
            else
            {
                character.Health -= 20;
            }
        }
    }
}
