using System;
using EasterRaces.Models.Cars.Contracts;

namespace EasterRaces.Models.Cars.Entities
{
    public abstract class Car : ICar
    {
        private string model;
        private int horsePower;

        protected Car(string model, int horsePower)
        {
            Model = model;
            HorsePower = horsePower;
        }

        public string Model
        {
            get
            {
                return model;
            }
            private set
            {
                if (string.IsNullOrWhiteSpace(value) || value.Length < 4)
                {
                    throw new ArgumentException($"Model {value} cannot be less than 4 symbols.");
                }

                model = value;
            }
        }

        public virtual int HorsePower
        {
            get
            {
                return horsePower;
            }
            private set
            {
                if (value < MinHorsePower || value > MaxHorsePower)
                {
                    throw new ArgumentException($"Invalid horse power: {value}.");
                }

                horsePower = value;
            }
        }

        public abstract double CubicCentimeters { get; }

        protected abstract int MinHorsePower { get; }
        protected abstract int MaxHorsePower { get; }

        public double CalculateRacePoints(int laps)
        {
            return CubicCentimeters / HorsePower * laps;
        }
    }
}
