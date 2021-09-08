using GuildCars.Models;

namespace GuildCars.Services.SaleProcessorService.SaleTypes
{
    public interface ISaleFactory
    {
        IVehicleSale GetVehicleSaleFor(SaleProcessServiceArgs args);
    }
}
