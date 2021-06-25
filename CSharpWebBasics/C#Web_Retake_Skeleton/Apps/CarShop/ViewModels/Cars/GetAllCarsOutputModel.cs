namespace CarShop.ViewModels.Cars
{
    public class GetAllCarsOutputModel
    {
        public string Id { get; set; }

        public string Image { get; set; }

        public string Model { get; set; }

        public short Year { get; set; }

        public string PlateNumber { get; set; }

        public int FixedIssues { get; set; }

        public int RemainingIssues { get; set; }
    }
}
