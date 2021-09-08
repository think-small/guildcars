using GuildCars.Models.Exceptions;
using GuildCars.Models.QueryParams;
using GuildCars.Services.InventoryService;
using GuildCars.UI.Models;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GuildCars.UI.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        public async Task<ActionResult> New()
        {
            var vehicleOptions = await _inventoryService.GetAllVehicleOptions();
            var viewModel = new InventoryInfoViewModel
            {
                VehicleOptions = vehicleOptions,
                VehicleConditionsSelected = new string[] { "new" }
            };
            ViewBag.Title = "Guild Cars - New Vehicles";

            return View("Inventory", viewModel);
        }
        public async Task<ActionResult> Used()
        {
            var vehicleOptions = await _inventoryService.GetAllVehicleOptions();
            var viewModel = new InventoryInfoViewModel
            {
                VehicleOptions = vehicleOptions,
                VehicleConditionsSelected = new string[] { "used" }
            };
            ViewBag.Title = "Guild Cars - Used Vehicles";

            return View("Inventory", viewModel);
        }

        [HttpPost]

        public async Task<ActionResult> Filter(InventoryInfoViewModel searchParams)
        {
            var searchFilter = new VehicleFilter
            {
                MinPrice = searchParams.MinPrice,
                MaxPrice = searchParams.MaxPrice,
                BodyStyles = searchParams.BodyStylesSelected ?? new int[0],
                Makes = searchParams.MakesSelected ?? new int[0],
                Models = searchParams.ModelsSelected ?? new int[0],
                Transmissions = searchParams.TransmissionTypesSelected ?? new int[0],
                VehicleConditions = searchParams.VehicleConditionsSelected ?? new string[0],
                IsSearchingForAvailableCars = true              
            };

            try
            {
                var vehicles = await _inventoryService.GetVehiclesFilteredBy(searchFilter);
                return PartialView(vehicles);
            }
            catch (InvalidInventoryFilterException ex)
            {
                return Json(new { Message = ex.Message });
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            var vehicle = await _inventoryService.GetDetailsFor(id);
            return View(vehicle);
        }
    }
}