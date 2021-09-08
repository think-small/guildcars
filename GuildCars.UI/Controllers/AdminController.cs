using GuildCars.Models;
using GuildCars.Models.SaleProcessServiceModels;
using GuildCars.Services.FileUploadService;
using GuildCars.Services.InventoryService;
using GuildCars.Services.ReportService;
using GuildCars.UI.Models;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.IO;
using System.Collections.Generic;
using System;
using System.Web;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using GuildCars.UI.Utils;

namespace GuildCars.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IReportService _reportService;
        private readonly IInventoryService _inventoryService;
        private readonly IFileUploadService _fileService;
        private Model _selectedModel;
        private string SavePath
        {
            get
            {
                return Server.MapPath("~/Images/Cars");
            }
        }
        public AdminController(IReportService reportService, IInventoryService inventoryService, IFileUploadService fileService)
        {
            _reportService = reportService;
            _inventoryService = inventoryService;
            _fileService = fileService;
        }
        
        public async Task<ActionResult> Vehicles()
        {
            var vehicleOptions = await _inventoryService.GetAllVehicleOptions();
            var viewModel = new InventoryInfoViewModel
            {
                VehicleOptions = vehicleOptions,
                VehicleConditionsSelected = new string[] { "new" }
            };
            ViewBag.Title = "Guild Cars Admin - Vehicles";
            return View("~/Views/Inventory/Inventory.cshtml", viewModel);
        }

        public async Task<ActionResult> AddVehicle()
        {
            var vehicleOptions = await _inventoryService.GetAllVehicleOptions();
            var viewModel = new AddVehicleViewModel
            {
                Vehicle = new Vehicle(),               
                TransmissionTypes = new SelectList(vehicleOptions.Transmissions, "Item1", "Item2"),
                BodyStyles = new SelectList(vehicleOptions.BodyStyles, "Item1", "Item2"),
                Details = new SelectList(vehicleOptions.Details, "Item1", "Item2"),
                MakesAndModels = vehicleOptions.MakesAndModels
            };

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("AddVehicle")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddVehiclePost(AddVehicleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var vehicleOptions = await _inventoryService.GetAllVehicleOptions();                
                viewModel.TransmissionTypes = new SelectList(vehicleOptions.Transmissions, "Item1", "Item2");
                viewModel.BodyStyles = new SelectList(vehicleOptions.BodyStyles, "Item1", "Item2");
                viewModel.Details = new SelectList(vehicleOptions.Details, "Item1", "Item2");
                viewModel.MakesAndModels = vehicleOptions.MakesAndModels;

                return View("AddVehicle", viewModel);
            }

            var detailsTask = _inventoryService.GetDetailsRange(viewModel.SelectedDetailIds);
            var modelTask = _inventoryService.GetMakeAndModel(viewModel.Vehicle.ModelId);
            await Task.WhenAll(detailsTask, modelTask);

            viewModel.Vehicle.OwnerId = null;
            viewModel.Vehicle.Details = detailsTask.Result;
            _selectedModel = modelTask.Result;

            var wereImagesUploaded = viewModel.ImageUploads.ToList().All(i => i != null);
            if (wereImagesUploaded)
                await SaveAllImagesAsync(viewModel);

            await _inventoryService.Save(viewModel.Vehicle);

            TempData["AddVehicleSuccessMessage"] = "You added a new vehicle to the database.";
            return RedirectToAction("AddVehicle");
        }

        private async Task SaveAllImagesAsync(AddVehicleViewModel viewModel)
        {
            if (viewModel.Vehicle.ImagePaths is null) viewModel.Vehicle.ImagePaths = new List<ImagePath>();

            var imageSaveTasks = new List<Task>();
            foreach (var img in viewModel.ImageUploads)
            {
                imageSaveTasks.Add(SaveImage(viewModel, img));
            }
            await Task.WhenAll(imageSaveTasks);
        }

        private async Task SaveImage(AddVehicleViewModel viewModel, HttpPostedFileBase img)
        {
            string fileName = GenerateFileName();
            var dir = GetAbsolutePathToDirectory();
            var extension = Path.GetExtension(img.FileName);

            var file = new FileUploadArgs
            {
                FileName = fileName,
                DirectoryPath = dir,
                Extension = extension,
                ByteCount = img.ContentLength,
                Data = img.InputStream
            };
            await _fileService.SaveAsync(file);

            viewModel.Vehicle.ImagePaths.Add(new ImagePath { VehicleId = viewModel.Vehicle.Id, Path = Path.Combine(GetRelativePathToDirectory(), fileName + extension) });
        }

        private string GenerateFileName()
        {
            return Guid.NewGuid().ToString();
        }

        private string GetAbsolutePathToDirectory()
        {
            return Path.Combine(SavePath, _selectedModel.Make.Name, _selectedModel.Name);
        }
        private string GetRelativePathToDirectory()
        {
            return Path.Combine(_selectedModel.Make.Name, _selectedModel.Name);
        }

        public async Task<ActionResult> EditVehicle(int id)
        {
            var vehicleOptions = await _inventoryService.GetAllVehicleOptions();
            var foundVehicle = await _inventoryService.GetVehicleBy(id);
            var preSelectedDetails = vehicleOptions.Details.Where(t => foundVehicle.Details.Any(d => d.Id == t.Item1))
                                                           .Select(d => d.Item1)
                                                           .ToList();
            var viewModel = new EditVehicleViewModel
            {
                Vehicle = foundVehicle,
                TransmissionTypes = new SelectList(vehicleOptions.Transmissions, "Item1", "Item2"),
                BodyStyles = new SelectList(vehicleOptions.BodyStyles, "Item1", "Item2"),
                Details = new MultiSelectList(vehicleOptions.Details, "Item1", "Item2", preSelectedDetails),
                MakesAndModels = vehicleOptions.MakesAndModels,
                ImagesToKeep = new List<string>()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ActionName("EditVehicle")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditVehiclePost(EditVehicleViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                var vehicleOptions = await _inventoryService.GetAllVehicleOptions();
                viewModel.TransmissionTypes = new SelectList(vehicleOptions.Transmissions, "Item1", "Item2");
                viewModel.BodyStyles = new SelectList(vehicleOptions.BodyStyles, "Item1", "Item2");
                viewModel.Details = new SelectList(vehicleOptions.Details, "Item1", "Item2");
                viewModel.MakesAndModels = vehicleOptions.MakesAndModels;

                return View("EditVehicle", viewModel);
            }

            _selectedModel = await _inventoryService.GetMakeAndModel(viewModel.Vehicle.ModelId);

            var files = new List<FileUploadArgs>();
            if (wereNewImagesUploadedFrom(viewModel))
            {
                var fileUploadArgs = CreateFileUploadArgsFrom(viewModel);
                files.AddRange(fileUploadArgs);
                viewModel.Vehicle.ImagePaths = CreateImagePathsFrom(files, viewModel.ImagesToKeep, viewModel.Vehicle.Id);
            }
            else if (viewModel.ImagesToKeep?.Count > 0)
            {
                viewModel.Vehicle.ImagePaths = CreateImagePathsFrom(viewModel.ImagesToKeep, viewModel.Vehicle.Id);
            }

            var editImagesTask = _fileService.EditAsync(files);
            var editVehicleTask = _inventoryService.Edit(viewModel.Vehicle, viewModel.SelectedDetailIds);
            try
            {
                await Task.WhenAll(editImagesTask, editVehicleTask);
                TempData["EditVehicleSuccessMessage"] = $"You edited the vehicle with VIN:  {viewModel.Vehicle.VIN}";
            }
            catch (Exception)
            {
                TempData["EditVehicleFailureMessage"] = "Unable to edit this vehicle. Try again later.";
            }
            
            return RedirectToAction("EditVehicle", editVehicleTask.Result);
        }

        private static bool wereNewImagesUploadedFrom(EditVehicleViewModel viewModel)
        {
            return viewModel.ImageUploads.Length > 0 && viewModel.ImageUploads[0] != null;
        }

        private List<FileUploadArgs> CreateFileUploadArgsFrom(EditVehicleViewModel viewModel)
        {
            var files = new List<FileUploadArgs>();
            foreach (var img in viewModel.ImageUploads)
            {
                var file = new FileUploadArgs
                {
                    FileName = GenerateFileName(),
                    DirectoryPath = GetAbsolutePathToDirectory(),
                    Extension = Path.GetExtension(img.FileName),
                    ByteCount = img.ContentLength,
                    Data = img.InputStream
                };

                files.Add(file);
            }
            return files;
        }

        private List<ImagePath> CreateImagePathsFrom(List<FileUploadArgs> args, List<string> imagePathsToKeep, int id)
        {
            var imagePaths = new List<ImagePath>();

            foreach (var file in args)
            {
                imagePaths.Add(new ImagePath
                {
                    Path = Path.Combine(_selectedModel.Make.Name, _selectedModel.Name, file.FileName + file.Extension),
                    VehicleId = id
                });
            }

            var oldImagePathsToKeep = CreateImagePathsFrom(imagePathsToKeep, id);
            imagePaths = imagePaths.Concat(oldImagePathsToKeep).ToList();

            return imagePaths;
        }
        private List<ImagePath> CreateImagePathsFrom(List<string> imagePathsToKeep, int id)
        {
            imagePathsToKeep = imagePathsToKeep ?? new List<string>();
            var imagePaths = new List<ImagePath>();

            foreach (var oldImage in imagePathsToKeep)
            {
                imagePaths.Add(new ImagePath
                {
                    Path = oldImage,
                    VehicleId = id
                });
            }

            return imagePaths;
        }

        [HttpPost]
        [ActionName("DeleteVehicle")]
        public async Task<ActionResult> DeleteVehiclePost(int id)
        {
            try
            {
                await _inventoryService.Delete(id, SavePath);
            }
            catch (Exception e) when (e is DirectoryNotFoundException || e is UnauthorizedAccessException)
            {
                return Json(new { DeleteVehicleFailureMessage = "Unable to delete vehicle.Please see the system administrator for assistance."});
            }
            catch (Exception e) when (e is IOException)
            {
                return Json(new { DeleteVehicleFailureMessage = "Unable to delete vehicle. Try again laer." });
            }

            return Json(new { DeleteVehicleSuccessMessage = "Vehicle was successfully deleted." });
        }

        public async Task<ActionResult> Users()
        {
            var context = new ApplicationDbContext();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var users = await userManager.Users.ToListAsync();
            var roles = await roleManager.Roles.ToListAsync();
            var viewModel = new UsersListViewModel();

            viewModel.Users = (from user in userManager.Users
                               select new BasicUserInfo
                               {
                                   Id = user.Id,
                                   FirstName = user.FirstName,
                                   LastName = user.LastName,
                                   Email = user.Email,
                                   Roles = (from userRole in user.Roles
                                                join role in roleManager.Roles on userRole.RoleId equals role.Id
                                                select role.Name).ToList()
                               }).ToList();

            return View(viewModel);
        }

        public async Task<ActionResult> EditUser(string id)
        {
            var rolesTask = Managers.GetAllRolesAsync();
            var ownedVehiclesTask = _inventoryService.GetVehiclesOwnedBy(id);
            var viewModelTask = Managers.CreateUserDetailsViewModelFrom(id);
            try
            {                
                await Task.WhenAll(rolesTask, ownedVehiclesTask, viewModelTask);

                viewModelTask.Result.Roles = rolesTask.Result;
                viewModelTask.Result.VehiclesOwned = ownedVehiclesTask.Result;

                return View(viewModelTask.Result);
            }
            catch (Exception)
            {
                return View("~/Views/FailedRequests/ServerError.cshtml");
            }                                  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("EditUser")]
        public async Task<ActionResult> EditUserPost(UserDetails viewModel)
        {
            if (!ModelState.IsValid)
            {
                var rolesTask = Managers.GetAllRolesAsync();
                var ownedVehiclesTask = _inventoryService.GetVehiclesOwnedBy(viewModel.Id);
                var viewModelTask = Managers.CreateUserDetailsViewModelFrom(viewModel.Id);

                try
                {
                    await Task.WhenAll(rolesTask, ownedVehiclesTask, viewModelTask);

                    viewModelTask.Result.Roles = rolesTask.Result;
                    viewModelTask.Result.VehiclesOwned = ownedVehiclesTask.Result;

                    return View("EditUser", viewModelTask.Result);
                }
                catch (Exception)
                {
                    return View("~/Views/FailedRequests/ServerError.cshtml");
                }
            }

            if (string.IsNullOrWhiteSpace(viewModel.NewPassword) == false)
            {

            }
            return RedirectToAction("Users");
        }

    }
}