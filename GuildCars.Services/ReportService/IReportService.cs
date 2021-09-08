using GuildCars.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Services.ReportService
{
    public interface IReportService
    {
        Task<Dictionary<DateTime, IList<SaleRecord>>> ReportSalesBetween(DateTime start, DateTime end);
        Task<Dictionary<DateTime, IList<SaleRecord>>> ReportSalesFor(string employeeId);
        Task<Dictionary<string, IList<SaleRecord>>> ReportSalesFor(Make make);
        Task<Dictionary<string, IList<SaleRecord>>> ReportSalesFor(Model model);
    }
}
