namespace EasterRaces.Models.Cars.Entities
{
    public class SportsCar : Car
    {
        public SportsCar(string model, int horsePower)
            : base(model, horsePower)
        {
        }

        public override double CubicCentimeters => 3000;

        protected override int MinHorsePower => 250;

        protected override int MaxHorsePower => 450;
    }
}
