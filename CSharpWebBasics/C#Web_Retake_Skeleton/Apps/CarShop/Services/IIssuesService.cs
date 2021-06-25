namespace CarShop.Services
{
    using CarShop.ViewModels.Issues;

    public interface IIssuesService
    {
        void AddIssue(AddIssueInpuModel input);

        CarIssuesOutputModel GetAllCarIssues(string carId);

        void DeleteIssue(string issueId, string carId);

        void FixIssue(string issueId, string carId);
    }
}
