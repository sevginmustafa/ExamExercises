using CarShop.Data;
using CarShop.Data.Models;
using CarShop.ViewModels.Cars;
using System;
using System.Collections.Generic;

namespace CarShop.Services
{
    public class CarsService : ICarsService
    {
        private readonly ApplicationDbContext db;

        public CarsService(ApplicationDbContext db)
        {
            this.db = db;
        }

        public void AddCar(AddCarInputModel model)
        {
            var car = new Car
            {
                Model = model.Model,
                Year = model.Year,
                PictureUrl = model.Image,
                PlateNumber = model.PlateNumber,
            };

            this.db.Cars.Add(car);

            this.db.SaveChanges();
        }

        public IEnumerable<GetAllCarsOutputModel> GetAll(string userId)
        {
            throw new NotImplementedException();
        }
    }
}
