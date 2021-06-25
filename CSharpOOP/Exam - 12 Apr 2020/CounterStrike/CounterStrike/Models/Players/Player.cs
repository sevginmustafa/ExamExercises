using CounterStrike.Models.Guns.Contracts;
using CounterStrike.Models.Players.Contracts;
using System;
using System.Text;

namespace CounterStrike.Models.Players
{
    public abstract class Player : IPlayer
    {
        private string username;
        private int health;
        private int armor;
        private IGun gun;

        protected Player(string username, int health, int armor, IGun gun)
        {
            Username = username;
            Health = health;
            Armor = armor;
            Gun = gun;
            IsAlive = true;
        }

        public string Username
        {
            get
            {
                return username;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Username cannot be null or empty.");
                }

                username = value;
            }
        }

        public int Health
        {
            get
            {
                return health;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Player health cannot be below or equal to 0.");
                }

                health = value;
            }
        }

        public int Armor
        {
            get
            {
                return armor;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Player armor cannot be below 0.");
                }

                armor = value;
            }
        }

        public IGun Gun
        {
            get
            {
                return gun;
            }
            private set
            {
                if (value == null)
                {
                    throw new ArgumentException("Gun cannot be null or empty.");
                }

                gun = value;
            }
        }

        public bool IsAlive { get; set; }

        public void TakeDamage(int points)
        {
            int result = Armor - points;

            if (result < 0)
            {
                Armor = 0;

                if (Health + result < 0)
                {
                    Health = 0;
                }
                else
                {
                    Health += result;
                }
            }
            else
            {
                Armor -= points;
            }

            if (Health == 0)
            {
                IsAlive = false;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine($"{GetType().Name}: {Username}");
            sb.AppendLine($"--Health: {Health}");
            sb.AppendLine($"--Armor: {Armor}");
            sb.AppendLine($"--Gun: {Gun.Name}");

            return sb.ToString().TrimEnd();
        }
    }
}
