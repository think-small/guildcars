using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Models.QueryParams;
using GuildCars.Models.SaleProcessServiceModels;
using GuildCars.Services.InventoryService;
using GuildCars.Services.SaleProcessorService;
using GuildCars.UI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace GuildCars.UI.Controllers
{
    [Authorize(Roles = "Admin,Sales")]
    public class SaleController : Controller
    {
        private readonly IInventoryService _inventory;
        private readonly ISaleProcessorService _saleService;
        public string SavePath
        {
            get { return Server.MapPath("~/ApprovalLetters"); }
        }

        public SaleController(IInventoryService inventory, ISaleProcessorService saleService)
        {
            _inventory = inventory;
            _saleService = saleService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public async Task<ActionResult> GetVehiclesOwnedBy(string ownerId)
        {
            var ownedVehicles = await _inventory.GetVehiclesOwnedBy(ownerId);
            return PartialView("_VehiclesOwnedBy", ownedVehicles);
        }

        [HttpPost]
        public async Task<ActionResult> GetCustomerInfo(string firstName, string lastName, string email)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = await userManager.FindByEmailAsync(email);
            var customerInfo = new CustomerInfoViewModel();

            if (user is null || user.FirstName.ToLower() != firstName.ToLower().Trim() || user.LastName.ToLower() != lastName.ToLower().Trim())
                customerInfo = null;
            else
            {
                customerInfo.Id = user.Id;
                customerInfo.FirstName = user.FirstName;
                customerInfo.LastName = user.LastName;
                customerInfo.Email = user.Email;
                customerInfo.Address1 = user.Address1;
                customerInfo.Address2 = user.Address2;
                customerInfo.City = user.City;
                customerInfo.State = user.State;
                customerInfo.ZipCode = user.ZipCode;
            }
            return PartialView("_CustomerInfo", customerInfo);
        }

        public async Task<ActionResult> GetAvailableVehicles()
        {
            var vehicleFilter = new VehicleFilter
            {
                Makes = new int[0],
                Models = new int[0],
                BodyStyles = new int[0],
                Transmissions = new int[0],
                MinPrice = 0,
                MaxPrice = 100000,
                IsSearchingForAvailableCars = true 
            };
            var availableVehicles = await _inventory.GetVehiclesFilteredBy(vehicleFilter);

            return PartialView("_AvailableVehicles", availableVehicles);
        }

        public async Task<ActionResult> GetVehicleDetailsFor(int id)
        {
            var vehicle = await _inventory.GetDetailsFor(id);
            return PartialView("_Details", vehicle);
        }

        [HttpPost]
        public async Task<ActionResult> ProcessSale(SaleViewModel viewModel)
        {
            try
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));

                var userPromise = userManager.FindByNameAsync(User.Identity.Name);
                var vehiclePromise = _inventory.GetDetailsFor(viewModel.VehicleId);
                var tradeInPromise = _inventory.GetDetailsFor(viewModel.TradeInId);
                await Task.WhenAll(vehiclePromise, tradeInPromise, userPromise);

                var sale = new SaleProcessServiceArgs
                {
                    VehicleId = viewModel.VehicleId,
                    Vehicle = vehiclePromise.Result,
                    PurchasePrice = viewModel.PurchasePrice,
                    TradeInId = viewModel.TradeInId,
                    TradeIn = tradeInPromise.Result,
                    PurchaseTypeId = viewModel.PurchaseTypeId,
                    CustomerId = viewModel.CustomerId,
                    EmployeeId = userPromise.Result.Id,
                    LoanLength = viewModel?.LoanLength,
                    InterestRate = viewModel?.InterestRate,
                    DownPayment = viewModel?.DownPayment,
                    ApprovalAmount = viewModel?.ApprovalAmount,
                    ApprovalLetter = viewModel.PurchaseTypeId != 2 ? null : new FileUploadArgs
                    {
                        FileName = Path.GetFileNameWithoutExtension(viewModel.ApprovalLetter.FileName),
                        Extension = Path.GetExtension(viewModel.ApprovalLetter.FileName),
                        DirectoryPath = SavePath,
                        Data = viewModel.ApprovalLetter.InputStream
                    }
                };

                await _saleService.Process(sale);
                var purchaseAgreement = _saleService.GetPurchaseAgreement();

                return new FileStreamResult(purchaseAgreement, "application/pdf")
                {
                    FileDownloadName = "PurchaseAgreement"
                };
            }
            catch (Exception e)
            {
                return RedirectToAction("Failure");
            }
        }

        [ActionName("Success")]
        public ActionResult SuccessfulSale()
        {
            return View();
        }

        [ActionName("Failure")]
        public ActionResult FailedSale()
        {
            return View();
        }
    }
}