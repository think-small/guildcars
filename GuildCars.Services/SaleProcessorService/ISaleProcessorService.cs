using GuildCars.Models;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.SaleProcessorService
{
    public interface ISaleProcessorService
    {
        Task Process(SaleProcessServiceArgs args);
        Stream GetPurchaseAgreement();
    }
}
