using GuildCars.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Data
{
    internal interface ISaleRepository
    {
        Task Add(SaleRecord sale);
        Task<IEnumerable<SaleRecord>> GetAll();
        Task<SaleRecord> GetById(int id);
        Task<IEnumerable<SaleRecord>> GetByEmployee(string id);
        Task<IEnumerable<SaleRecord>> GetByCustomer(string id);
        Task<IEnumerable<SaleRecord>> GetByDateRange(DateTime start, DateTime end);
        Task Edit(SaleRecord sale);
        Task Delete(int id);
        Task<PurchaseType> GetPurchaseType(int id);
        Task<IEnumerable<SaleRecord>> GetSalesForMake(int id);
        Task<IEnumerable<SaleRecord>> GetSalesForModel(int id);
    }
}
