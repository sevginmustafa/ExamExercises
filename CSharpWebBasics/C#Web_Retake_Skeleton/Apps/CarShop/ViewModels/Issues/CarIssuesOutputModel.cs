namespace CarShop.ViewModels.Issues
{
    using System.Collections.Generic;

    public class CarIssuesOutputModel
    {
        public string Id { get; set; }

        public string Model { get; set; }

        public short Year { get; set; }

        public IEnumerable<IssuesOutputModel> Issues { get; set; }
    }
}
