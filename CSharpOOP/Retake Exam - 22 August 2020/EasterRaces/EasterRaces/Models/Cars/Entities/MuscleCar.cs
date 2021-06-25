namespace EasterRaces.Models.Cars.Entities
{
    public class MuscleCar : Car
    {
        public MuscleCar(string model, int horsePower)
            : base(model, horsePower)
        {
        }

        public override double CubicCentimeters => 5000;

        protected override int MinHorsePower => 400;

        protected override int MaxHorsePower => 600;
    }
}
