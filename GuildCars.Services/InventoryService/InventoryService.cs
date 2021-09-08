using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Models.QueryParams;
using GuildCars.Models.QueryResults;
using GuildCars.Services.FileUploadService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GuildCars.Services.InventoryService
{
    internal class InventoryService : IInventoryService
    {
        private readonly IVehicleRepository _repo;
        private readonly IFileUploadService _fileService;

        public InventoryService(IVehicleRepository repo, IFileUploadService fileService)
        {
            _repo = repo;
            _fileService = fileService;
        }
        public async Task<IEnumerable<Vehicle>> GetVehiclesFilteredBy(VehicleFilter args)
        {
            if (AreInvalid(args))
                throw new InvalidInventoryFilterException();

            return await _repo.FilterBy(args);
        }

        private static bool AreInvalid(VehicleFilter args)
        {
            return args.Makes == null 
                || args.Models == null 
                || args.Transmissions == null 
                || args.BodyStyles == null 
                || args.MinPrice > args.MaxPrice
                || args.MinPrice < 0M
                || args.MaxPrice > 100000M;
        }

        public async Task<VehicleOptions> GetAllVehicleOptions()
        {
            var makesAndModels = await _repo.GetAllMakesAndModels();
            var bodyStyles = await _repo.GetAllBodyStyles();
            var transmissionTypes = await _repo.GetAllTransmissionTypes();
            var details = await _repo.GetAllDetails();

            var makeTuples = new List<Tuple<int, string>>();
            var modelTuples = new List<Tuple<int, string>>();
            var bodyStyleTuples = new List<Tuple<int, string>>();
            var transmissionTuples = new List<Tuple<int, string>>();
            var detailTuples = new List<Tuple<int, string>>();

            foreach (var model in makesAndModels)
            {
                modelTuples.Add(new Tuple<int, string>(model.Id, model.Name));

                if (makeTuples.Any(t => t.Item1 == model.MakeId))
                    continue;
                makeTuples.Add(new Tuple<int, string>(model.MakeId, model.Make.Name));
            }

            foreach (var bodyStyle in bodyStyles)
            {
                bodyStyleTuples.Add(new Tuple<int, string>(bodyStyle.Id, bodyStyle.Name));
            }

            foreach (var transmission in transmissionTypes)
            {
                transmissionTuples.Add(new Tuple<int, string>(transmission.Id, transmission.Name));
            }

            foreach (var detail in details)
            {
                detailTuples.Add(new Tuple<int, string>(detail.Id, detail.Description));
            }

            return new VehicleOptions
            {
                BodyStyles = bodyStyleTuples,
                Transmissions = transmissionTuples,
                Details = detailTuples,
                Makes = makeTuples,
                Models = modelTuples,
                MakesAndModels = CreateDictionaryFrom(makesAndModels)
            };
        }

        private Dictionary<Make, IList<Model>> CreateDictionaryFrom(IEnumerable<Model> makesAndModels)
        {
            var makeAndModelDict = new Dictionary<Make, IList<Model>>();
            foreach (var model in makesAndModels)
            {
                IList<Model> models = new List<Model>();
                var hasMakeBeenCached = makeAndModelDict.Keys.FirstOrDefault(k => k.Id == model.Make.Id && k.Name == model.Make.Name) != null;
                var hasModelBeenCached = models != null && models.Any(m => m.Id == model.Id);

                if (hasMakeBeenCached && hasModelBeenCached)
                    continue;
                else if (hasMakeBeenCached && !hasModelBeenCached)
                    makeAndModelDict[model.Make].Add(new Model { Id = model.Id, Name = model.Name });
                else
                    makeAndModelDict.Add(model.Make, new List<Model> { new Model { Id = model.Id, Name = model.Name} });
            }

            return makeAndModelDict;
        }

        public async Task Delete(int id, string pathToImages)
        {
            var vehicle = await _repo.GetById(id);
            if (vehicle.ImagePaths.Count == 0)
            {
                _fileService.DeleteDirectoryAndFiles(pathToImages);
            }
            else
            {
                var imagePaths = GenerateFilePathsFrom(vehicle.ImagePaths, pathToImages);
                _fileService.DeleteFilesFrom(imagePaths);
            }

            await _repo.Delete(id);
        }

        private IEnumerable<string> GenerateFilePathsFrom(ICollection<ImagePath> imagePaths, string parentDirectory)
        {
            var fullImagePaths = new List<string>();

            foreach (var imagePath in imagePaths)
            {
                fullImagePaths.Add(Path.Combine(parentDirectory, imagePath.Path));
            }

            return fullImagePaths;
        }

        public async Task<Vehicle> GetDetailsFor(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<ICollection<Detail>> GetDetailsRange(IEnumerable<int> ids)
        {
            return (ICollection<Detail>)await _repo.GetDetails(ids);
        }

        public async Task<IEnumerable<Vehicle>> GetVehiclesOwnedBy(string ownerId)
        {
            return await _repo.GetVehiclesOwnedBy(ownerId);
        }

        public async Task<int> Save(Vehicle vehicle)
        {
            return await _repo.Add(vehicle);
        }

        public async Task<Model> GetMakeAndModel(int id)
        {
            return await _repo.GetMakeAndModel(id);
        }

        public async Task<Vehicle> GetVehicleBy(int id)
        {
            return await _repo.GetById(id);
        }

        public async Task<int> Edit(Vehicle vehicle, int[] selectedDetailIds)
        {
            vehicle.Details = await GetDetailsRange(selectedDetailIds);
            return await _repo.Edit(vehicle);
        }
    }
}
