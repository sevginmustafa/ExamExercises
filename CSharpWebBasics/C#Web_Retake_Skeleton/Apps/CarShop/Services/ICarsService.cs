using CarShop.ViewModels.Cars;
using System.Collections.Generic;

namespace CarShop.Services
{
    public interface ICarsService
    {
        void AddCar(AddCarInputModel model);

        IEnumerable<GetAllCarsOutputModel> GetAll(string userId);
    }
}
