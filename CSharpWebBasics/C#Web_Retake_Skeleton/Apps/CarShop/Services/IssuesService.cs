namespace CarShop.Services
{
    using CarShop.Data;
    using CarShop.Data.Models;
    using CarShop.ViewModels.Issues;
    using System.Linq;

    public class IssuesService : IIssuesService
    {
        private readonly ApplicationDbContext db;

        public IssuesService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddIssue(AddIssueInpuModel input)
        {
            var issue = new Issue
            {
                Description = input.Description,
                IsFixed = false,
                CarId = input.CarId,
            };

            this.db.Issues.Add(issue);

            this.db.SaveChanges();
        }

        public void DeleteIssue(string issueId, string carId)
        {
            var issue = this.db.Issues.FirstOrDefault(x => x.Id == issueId && x.CarId == carId);

            if (issue!=null)
            {
                this.db.Remove(issue);

                this.db.SaveChanges();
            }
        }

        public void FixIssue(string issueId, string carId)
        {
            var issue = this.db.Issues.FirstOrDefault(x => x.Id == issueId && x.CarId == carId);

            if (issue != null)
            {
                issue.IsFixed = true;

                this.db.SaveChanges();
            }
        }

        public CarIssuesOutputModel GetAllCarIssues(string carId)
        {
            return this.db.Cars.Where(x => x.Id == carId)
                .Select(x => new CarIssuesOutputModel
                {
                    Id=x.Id,
                    Model = x.Model,
                    Year = x.Year,
                    Issues = x.Issues.Select(x => new IssuesOutputModel
                    {
                        Id=x.Id,
                        Description = x.Description,
                        IsItFixed = x.IsFixed == true ? "Yes" : "Not yet"
                    })
                    .ToList()
                })
                .FirstOrDefault();
        }
    }
}
