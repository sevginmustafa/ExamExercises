namespace CarShop.ViewModels.Cars
{
    public class GetAllCarsOutputModel
    {
        public string Image { get; set; }

        public string Model { get; set; }

        public short Year { get; set; }

        public string PlateNumber { get; set; }

        public string FixedIssues { get; set; }

        public string RemainingIssues { get; set; }
    }
}
