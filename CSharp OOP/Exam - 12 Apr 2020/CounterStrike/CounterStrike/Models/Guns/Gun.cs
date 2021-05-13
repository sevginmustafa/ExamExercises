using CounterStrike.Models.Guns.Contracts;
using System;

namespace CounterStrike.Models.Guns
{
    public abstract class Gun : IGun
    {
        private string name;
        private int bulletsCount;

        protected Gun(string name, int bulletsCount)
        {
            Name = name;
            BulletsCount = bulletsCount;
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
                    throw new ArgumentException("Gun cannot be null or empty.");
                }

                name = value;
            }
        }

        public int BulletsCount
        {
            get
            {
                return bulletsCount;
            }
            protected set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Bullets cannot be below 0.");
                }

                bulletsCount = value;
            }
        }

        public abstract int Fire();
    }
}
