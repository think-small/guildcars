using GuildCars.Models;
using GuildCars.Services.SaleProcessorService;

namespace GuildCars.Services
{
    internal interface ILoanCalculatorService
    {
        AmortizedLoanSchedule GetRepaymentScheduleFor(DealerFinanceSale sale);
    }
}
