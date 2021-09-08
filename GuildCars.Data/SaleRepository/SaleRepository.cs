using GuildCars.Models;
using GuildCars.Models.Contexts;
using GuildCars.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Data
{
    internal class SaleRepository : ISaleRepository
    {
        public async Task Add(SaleRecord sale)
        {
            using (var context = new GCContext())
            {
                context.SaleRecords.Add(sale);
                await context.SaveChangesAsync();
            }
        }

        public async Task Delete(int id)
        {
            using (var context = new GCContext())
            {
                try
                {
                    var idParam = new SqlParameter("Id", id);
                    await context.Database.ExecuteSqlCommandAsync("GCEFTestDeleteSaleById @Id", idParam);
                }
                catch (SqlException)
                {
                    throw new CascadeDeleteException($"Unable to delete sale with id: {id}.");
                }
            }
        }

        public async Task Edit(SaleRecord sale)
        {
            using (var context = new GCContext())
            {
                context.Entry(sale).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<SaleRecord>> GetAll()
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords.AsNoTracking().ToListAsync();
            }
        }

        public async Task<IEnumerable<SaleRecord>> GetByCustomer(string id)
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords
                                    .AsNoTracking()
                                    .Where(s => s.CustomerId == id)
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<SaleRecord>> GetByDateRange(DateTime start, DateTime end)
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords
                                    .AsNoTracking()
                                    .Where(s => start <= s.Date && end >= s.Date)
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<SaleRecord>> GetByEmployee(string id)
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords                    
                                    .AsNoTracking()
                                    .Where(s => s.EmployeeId == id)
                                    .ToListAsync();
            }
        }

        public async Task<SaleRecord> GetById(int id)
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords
                                    .AsNoTracking()
                                    .FirstOrDefaultAsync(s => s.Id == id);
            }
        }

        public async Task<PurchaseType> GetPurchaseType(int id)
        {
            using (var context = new GCContext())
            {
                return await context.PurchaseTypes.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
            }
        }

        public async Task<IEnumerable<SaleRecord>> GetSalesForMake(int id)
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords
                                    .AsNoTracking()
                                    .Where(s => s.Vehicle.Model.MakeId == id)
                                    .Include("Vehicle.Model.Make")
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<SaleRecord>> GetSalesForModel(int id)
        {
            using (var context = new GCContext())
            {
                return await context.SaleRecords
                                    .AsNoTracking()
                                    .Where(s => s.Vehicle.ModelId == id)
                                    .Include("Vehicle.Model")
                                    .ToListAsync();
            }
        }
    }
}
