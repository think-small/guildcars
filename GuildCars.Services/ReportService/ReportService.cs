using GuildCars.Data;
using GuildCars.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Services.ReportService
{
    internal class ReportService : IReportService
    {
        private readonly ISaleRepository _saleRepo;
        public ReportService(ISaleRepository saleRepo)
        {
            _saleRepo = saleRepo;
        }

        public async Task<Dictionary<DateTime, IList<SaleRecord>>> ReportSalesBetween(DateTime start, DateTime end)
        {
            var accumulator = new Dictionary<DateTime, IList<SaleRecord>>();

            var sales = await GetSalesBetween(start, end);

            sales.Aggregate(accumulator, (Dictionary<DateTime, IList<SaleRecord>> acc, SaleRecord current) =>
            {
                if (acc.ContainsKey(current.Date))
                    acc[current.Date].Add(current);
                else
                    acc.Add(current.Date, new List<SaleRecord> { current });

                return acc;
            });

            return accumulator;
        }
        private async Task<IEnumerable<SaleRecord>> GetSalesBetween(DateTime start, DateTime end)
        {
            return await _saleRepo.GetByDateRange(start, end);
        }

        public async Task<Dictionary<DateTime, IList<SaleRecord>>> ReportSalesFor(string employeeId)
        {
            var accumulator = new Dictionary<DateTime, IList<SaleRecord>>();

            var sales = await GetSalesFor(employeeId);

            sales.Aggregate(accumulator, (Dictionary<DateTime, IList<SaleRecord>> acc, SaleRecord current) =>
            {
                if (acc.ContainsKey(current.Date))
                    acc[current.Date].Add(current);
                else
                    acc.Add(current.Date, new List<SaleRecord> { current });
                return acc;
            });

            return accumulator;
        }

        private async Task<IEnumerable<SaleRecord>> GetSalesFor(string employeeId)
        {
            return await _saleRepo.GetByEmployee(employeeId);
        }

        public async Task<Dictionary<string, IList<SaleRecord>>> ReportSalesFor(Make make)
        {
            var accumulator = new Dictionary<string, IList<SaleRecord>>();

            var sales = await GetSalesForMake(make.Id);

            sales.Aggregate(accumulator, (Dictionary<string, IList<SaleRecord>> acc, SaleRecord current) =>
            {
                if ((acc.ContainsKey(current.Vehicle.Model.Make.Name)))
                    acc[current.Vehicle.Model.Make.Name].Add(current);
                else
                    acc.Add(current.Vehicle.Model.Make.Name, new List<SaleRecord> { current });

                return acc;
            });

            return accumulator;
        }
        private async Task<IEnumerable<SaleRecord>> GetSalesForMake(int id)
        {
            return await _saleRepo.GetSalesForMake(id);
        }


        public async Task<Dictionary<string, IList<SaleRecord>>> ReportSalesFor(Model model)
        {
            var accumulator = new Dictionary<string, IList<SaleRecord>>();

            var sales = await GetSalesForModel(model.Id);

            sales.Aggregate(accumulator, (Dictionary<string, IList<SaleRecord>> acc, SaleRecord current) =>
            {
                if (acc.ContainsKey(current.Vehicle.Model.Name))
                    acc[current.Vehicle.Model.Name].Add(current);
                else
                    acc.Add(current.Vehicle.Model.Name, new List<SaleRecord> { current });

                return acc;
            });

            return accumulator;
        }
        private async Task<IEnumerable<SaleRecord>> GetSalesForModel(int id)
        {
            return await _saleRepo.GetSalesForModel(id);
        }
    }
}
