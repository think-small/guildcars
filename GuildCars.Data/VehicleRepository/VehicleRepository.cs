using GuildCars.Models;
using GuildCars.Models.Contexts;
using GuildCars.Models.Exceptions;
using GuildCars.Models.QueryParams;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Data
{
    internal class VehicleRepository : IVehicleRepository
    {
        public async Task<int> Add(Vehicle vehicle)
        {
            using (var context = new GCContext())
            {
                context.Vehicles.Add(vehicle);
                await context.SaveChangesAsync();

                return vehicle.Id;
            }
        }

        public async Task Delete(int id)
        {
            using (var context = new GCContext())
            {
                var storedProc = ConfigurationManager.AppSettings["Mode"] == "PROD" ? "EXEC DeleteVehicleById @Id" : "EXEC GCEFTestDeleteVehicleById @Id";

                try
                {
                    var idParam = new SqlParameter("Id", id);
                    await context.Database.ExecuteSqlCommandAsync(storedProc, idParam);
                }
                catch (SqlException)
                {
                    throw new CascadeDeleteException($"Unable to delete vehicle with id: {id}. Other data is currently referencing this vehicle.");
                }
                
            }
        }

        public async Task<int> Edit(Vehicle vehicle)
        {
            using (var context = new GCContext())
            {
                try
                {
                    var foundVehicle = await GetById(vehicle.Id);
                    context.Entry(foundVehicle).State = EntityState.Modified;

                    context.Entry(foundVehicle).CurrentValues.SetValues(vehicle);
                    SetDetailsFor(foundVehicle, vehicle.Details, context);
                    SetImagePathsFor(foundVehicle, vehicle.ImagePaths, context);
                    await context.SaveChangesAsync();
                }
                catch (Exception e) when (e is DbUpdateConcurrencyException || e is ArgumentNullException)
                {
                    throw new VehicleNotFoundException($"Invalid Id ({vehicle.Id}). Unable to update vehicle.", e);
                }
                
                return vehicle.Id;
            }
        }

        private void SetDetailsFor(Vehicle vehicle, ICollection<Detail> newDetails, GCContext context)
        {
            if (vehicle.Details is null)
                vehicle.Details = new HashSet<Detail>();
            if (newDetails is null)
                newDetails = new HashSet<Detail>();

            var previousDetails = new List<Detail>();
            previousDetails.AddRange(vehicle.Details);            

            vehicle.Details.Clear();

            foreach (var detail in newDetails)
            {
                var foundDetail = previousDetails.FirstOrDefault(d => d.Id == detail.Id);
                if (foundDetail != null)  //  Context is currently tracking entity, don't re-attach it.
                {
                    vehicle.Details.Add(foundDetail);
                    continue;
                }

                context.Entry(detail).State = EntityState.Modified;
                vehicle.Details.Add(detail);
            }
        }
        private void SetImagePathsFor(Vehicle vehicle, ICollection<ImagePath> imagePaths, GCContext context)
        {
            if (vehicle.ImagePaths is null)
                vehicle.ImagePaths = new HashSet<ImagePath>();
            if (imagePaths is null)
                imagePaths = new HashSet<ImagePath>();

            var imagePathsList = vehicle.ImagePaths.ToList();

            if (imagePaths.Count == 0)
            {
                imagePathsList.ForEach(i => context.Entry(i).State = EntityState.Deleted);
                return;
            }
            DeleteOldImages(imagePaths, context, imagePathsList);
            AddNewImages(vehicle, imagePaths, context, imagePathsList);

        }

        private void AddNewImages(Vehicle vehicle, ICollection<ImagePath> imagePaths, GCContext context, List<ImagePath> allImagesForVehicle)
        {
            foreach (var imagePath in imagePaths)
            {
                var foundImagePath = allImagesForVehicle.FirstOrDefault(i => i.Path == imagePath.Path);
                if (foundImagePath != null)  //  Context is currently tracking entity, don't re-attach it.
                {
                    context.Entry(foundImagePath).State = EntityState.Unchanged;
                    continue;
                }

                //  New image needs to be added to context,
                //  existing images just need their EntityState set to 'Modified'
                //  else duplicate entries in database are created
                if (allImagesForVehicle.Any(i => i.Path != imagePath.Path))
                {
                    context.ImagePaths.Add(imagePath);
                    continue;
                }

                vehicle.ImagePaths.Add(imagePath);
            }
        }
        private void DeleteOldImages(ICollection<ImagePath> newImagePaths, GCContext context, List<ImagePath> oldImagePaths)
        {
            oldImagePaths.Except(newImagePaths)
                               .ToList()
                               .ForEach(i => context.Entry(i).State = EntityState.Deleted);
        }

        public async Task<IEnumerable<Vehicle>> FilterBy(VehicleFilter filter)
        {
            using (var context = new GCContext())
            {
                var query = context.Vehicles
                                   .AsNoTracking()
                                   .Include(v => v.Model.Make)
                                   .Include(v => v.ImagePaths)
                                   .Where(c => c.SalePrice >= filter.MinPrice && c.SalePrice <= filter.MaxPrice);

                query = AddDealerOwnedVehiclesTo(filter, query);
                query = AddMakesTo(filter, query);
                query = AddModelsTo(filter, query);
                query = AddBodyStylesTo(filter, query);
                query = AddVehicleConditionTo(filter, query);
                query = AddTransmissionsTo(filter, query);

                return await query.ToListAsync();
            }
        }
        private IQueryable<Vehicle> AddDealerOwnedVehiclesTo(VehicleFilter filter, IQueryable<Vehicle> query)
        {
            if (filter.IsSearchingForAvailableCars == true)
                query = query.Where(c => c.OwnerId == null);
            else
                query = query.Where(c => c.OwnerId != null);

            return query;
        }
        private IQueryable<Vehicle> AddMakesTo(VehicleFilter filter, IQueryable<Vehicle> query)
        {
            if (filter.Makes.Length == 0)
                return query;

            query = query.Where(c => filter.Makes.Contains(c.Model.MakeId));
            return query;
        }
        private IQueryable<Vehicle> AddModelsTo(VehicleFilter filter, IQueryable<Vehicle> query)
        {
            if (filter.Models.Length == 0)
                return query;

            query = query.Where(c => filter.Models.Contains(c.ModelId));
            return query;
        }
        private IQueryable<Vehicle> AddBodyStylesTo(VehicleFilter filter, IQueryable<Vehicle> query)
        {
            if (filter.BodyStyles.Length == 0)
                return query;

            query = query.Where(c => filter.BodyStyles.Contains(c.BodyStyleId));
            return query;
        }
        private IQueryable<Vehicle> AddVehicleConditionTo(VehicleFilter filter, IQueryable<Vehicle> query)
        {
            if (filter.VehicleConditions?.Length == 1 && filter.VehicleConditions.Contains("new"))
            {
                query = query.Where(c => c.IsNew == true);
            }
            else if (filter.VehicleConditions?.Length == 1 && filter.VehicleConditions.Contains("used"))
            {
                query = query.Where(c => c.IsNew == false);
            }

            return query;            
        }
        private IQueryable<Vehicle> AddTransmissionsTo(VehicleFilter filter, IQueryable<Vehicle> query)
        {
            if (filter.Transmissions.Length == 0)
                return query;

            query = query.Where(c => filter.Transmissions.Contains(c.TransmissionTypeId));
            return query;
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            using (var context = new GCContext())
            {
                return await context.Vehicles.AsNoTracking().ToListAsync();
            }
        }

        public async Task<IEnumerable<BodyStyle>> GetAllBodyStyles()
        {
            using (var context = new GCContext())
            {
                return await context.BodyStyles
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Model>> GetAllMakesAndModels()
        {
            using (var context = new GCContext())
            {
                return await context.Models
                                    .AsNoTracking()
                                    .Include(m => m.Make)
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<TransmissionType>> GetAllTransmissionTypes()
        {
            using (var context = new GCContext())
            {
                return await context.TransmissionTypes
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Detail>> GetAllDetails()
        {
            using (var context = new GCContext())
            {
                return await context.Details
                                    .AsNoTracking()
                                    .ToListAsync();
            }
        }

        public async Task<IEnumerable<Detail>> GetDetails(IEnumerable<int> ids)
        {
            using (var context = new GCContext())
            {
                return await context.Details
                                    .AsNoTracking()
                                    .Where(d => ids.Contains(d.Id))
                                    .ToListAsync();
            }
        }

        public async Task<Vehicle> GetById(int id)
        {
            var storedProc = ConfigurationManager.AppSettings["Mode"] == "PROD" ? "EXEC GetDetailsForVehicle @Id" : "EXEC GCEFTestGetDetailsForVehicle @Id";

            using (var context = new GCContext())
            {

                var vehicle = await context.Vehicles
                                            .AsNoTracking()
                                            .Include(v => v.TransmissionType)
                                            .Include(v => v.BodyStyle)
                                            .Include(v => v.ImagePaths)
                                            .Include("Model.Make")
                                            .FirstOrDefaultAsync(v => v.Id == id);

                var idParam = new SqlParameter("Id", id);
                var detailsQuery = context.Database.SqlQuery<Detail>(storedProc, idParam);                
                var details = new HashSet<Detail>();
                await detailsQuery.ForEachAsync(d => details.Add(d));
                vehicle?.SetDetails(details);

                return vehicle;
            }
        }

        public async Task<Model> GetMakeAndModel(int id)
        {
            using (var context = new GCContext())
            {
                return await context.Models
                                    .AsNoTracking()
                                    .Include(m => m.Make)
                                    .FirstOrDefaultAsync(m => m.Id == id);
            }
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesOwnedBy(string userId)
        {
            using (var context = new GCContext())
            {
                return await context.Vehicles.AsNoTracking().Where(v => v.OwnerId.ToLower() == userId.ToLower())
                                                            .Include("Model.Make")
                                                            .Include(v => v.ImagePaths)                                                            
                                                            .Include(v => v.TransmissionType)
                                                            .Include(v => v.BodyStyle)
                                                            .ToListAsync();
            }                
        }
    }
}
