namespace CarShop.Services
{
    using CarShop.ViewModels.Cars;
    using System.Collections.Generic;

    public interface ICarsService
    {
        void AddCar(AddCarInputModel input,string userId);

        IEnumerable<GetAllCarsOutputModel> GetAll(string userId);

        IEnumerable<GetAllCarsOutputModel> GetAllForMechanics();
    }
}
