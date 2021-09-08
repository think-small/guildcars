using GuildCars.Models;
using GuildCars.Models.QueryParams;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Data
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetVehiclesOwnedBy(string userId);
        Task<IEnumerable<Vehicle>> GetAll();
        Task<Vehicle> GetById(int id);
        Task<int> Add(Vehicle vehicle);
        Task<int> Edit(Vehicle vehicle);
        Task Delete(int id);
        Task<Model> GetMakeAndModel(int id);
        Task<IEnumerable<Model>> GetAllMakesAndModels();
        Task<IEnumerable<BodyStyle>> GetAllBodyStyles();
        Task<IEnumerable<TransmissionType>> GetAllTransmissionTypes();
        Task<IEnumerable<Detail>> GetAllDetails();
        Task<IEnumerable<Detail>> GetDetails(IEnumerable<int> ids);
        Task<IEnumerable<Vehicle>> FilterBy(VehicleFilter filter);
    }
}
