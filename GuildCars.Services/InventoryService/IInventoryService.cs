using GuildCars.Models;
using GuildCars.Models.QueryParams;
using GuildCars.Models.QueryResults;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Services.InventoryService
{
    public interface IInventoryService
    {
        Task<Vehicle> GetDetailsFor(int id);
        Task<ICollection<Detail>> GetDetailsRange(IEnumerable<int> ids);
        Task<IEnumerable<Vehicle>> GetVehiclesFilteredBy(VehicleFilter args);
        Task<VehicleOptions> GetAllVehicleOptions();
        Task<Model> GetMakeAndModel(int id);
        Task<IEnumerable<Vehicle>> GetVehiclesOwnedBy(string ownerId);
        Task<int> Save(Vehicle vehicle);
        Task<Vehicle> GetVehicleBy(int id);
        Task<int> Edit(Vehicle vehicle, int[] selectedDetailIds);
        Task Delete(int id, string pathToImagesDirectory);
    }
}
