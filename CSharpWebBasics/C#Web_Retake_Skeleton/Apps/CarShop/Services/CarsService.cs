namespace CarShop.Services
{
    using CarShop.Data;
    using CarShop.Data.Models;
    using CarShop.ViewModels.Cars;
    using System.Collections.Generic;
    using System.Linq;

    public class CarsService : ICarsService
    {
        private readonly ApplicationDbContext db;

        public CarsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddCar(AddCarInputModel input, string userId)
        {
            var car = new Car
            {
                Model = input.Model,
                Year = input.Year,
                PictureUrl = input.Image,
                PlateNumber = input.PlateNumber,
                OwnerId = userId,
            };

            this.db.Cars.Add(car);

            this.db.SaveChanges();
        }

        public IEnumerable<GetAllCarsOutputModel> GetAll(string userId)
        {
            return this.db.Cars.Where(x => x.OwnerId == userId)
                .Select(x => new GetAllCarsOutputModel
                {
                    Id = x.Id,
                    Model = x.Model,
                    Image = x.PictureUrl,
                    Year = x.Year,
                    PlateNumber = x.PlateNumber,
                    RemainingIssues = x.Issues.Count(x => x.IsFixed == false),
                    FixedIssues = x.Issues.Count(x => x.IsFixed),
                })
                .ToList();
        }

        public IEnumerable<GetAllCarsOutputModel> GetAllForMechanics()
        {
            return this.db.Cars.Where(x => x.Issues.Any(x => x.IsFixed == false))
               .Select(x => new GetAllCarsOutputModel
               {
                   Id = x.Id,
                   Model = x.Model,
                   Image = x.PictureUrl,
                   Year = x.Year,
                   PlateNumber = x.PlateNumber,
                   RemainingIssues = x.Issues.Count(x => x.IsFixed == false),
                   FixedIssues = x.Issues.Count(x => x.IsFixed),
               })
                .ToList();
        }
    }
}
