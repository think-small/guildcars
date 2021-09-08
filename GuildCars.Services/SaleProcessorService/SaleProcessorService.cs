using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Services.ReceiptGeneratorService;
using GuildCars.Services.SaleProcessorService.SaleTypes;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.SaleProcessorService
{
    internal class SaleProcessorService : ISaleProcessorService
    {
        private readonly ISaleFactory _saleFactory;
        private readonly IVehicleRepository _vehicleRepo;
        private readonly ISaleRepository _saleRepo;
        private SaleProcessServiceArgs _purchaseInfo;
        private Stream _purchaseAgreement;

        public SaleProcessorService(ISaleFactory saleFactory, IVehicleRepository vehicleRepo, ISaleRepository saleRepo)
        {
            _saleFactory = saleFactory;
            _vehicleRepo = vehicleRepo;
            _saleRepo = saleRepo;
        }

        public async Task Process(SaleProcessServiceArgs purchaseInfo)
        {
            _purchaseInfo = purchaseInfo;

            await ThrowIfVehiclesDoNotExistAsync();

            var sale = _saleFactory.GetVehicleSaleFor(purchaseInfo);
            sale.ProcessTransaction();
            _purchaseAgreement = sale.GetPurchaseAgreement();
            await Task.WhenAll(UpdateSoldVehicleAsync(), UpdateTradeInAsync(), SaveRecordOfSaleAsync());
        }

        private async Task ThrowIfVehiclesDoNotExistAsync()
        {
            try
            {
                var purchasedVehicle = _vehicleRepo.GetById(_purchaseInfo.Vehicle.Id);
                var tradeInVehicle = _purchaseInfo.TradeIn != null ? _vehicleRepo.GetById(_purchaseInfo.TradeIn.Id) : Task.CompletedTask;
                await Task.WhenAll(purchasedVehicle, tradeInVehicle);

                if (purchasedVehicle == null)
                    throw new SaleInformationMissingException($"Unalbe to process sale - no vehicle with id: {_purchaseInfo.Vehicle.Id}");
                if (_purchaseInfo.TradeIn != null && tradeInVehicle == null)
                    throw new SaleInformationMissingException($"Unable to process sale - no trade-in vehicle with id: {_purchaseInfo.TradeIn.Id}");
            }
            catch (NullReferenceException)
            {
                throw new SaleInformationMissingException($"Unable to process sale - no vehicle provided.");
            }
            
        }

        private async Task SaveRecordOfSaleAsync()
        {
            var saleRecord = new SaleRecord
            {
                CustomerId = _purchaseInfo.CustomerId,
                EmployeeId = _purchaseInfo.EmployeeId,
                VehicleId = _purchaseInfo.Vehicle.Id,
                TradeInId = _purchaseInfo.TradeIn?.Id,
                PurchasePrice = _purchaseInfo.PurchasePrice,
                ExpectedSalePrice = _purchaseInfo.Vehicle.SalePrice,
                PurchaseTypeId = _purchaseInfo.PurchaseTypeId,
                Date = DateTime.Now.Date
            };
            await _saleRepo.Add(saleRecord);
        }

        private async Task UpdateTradeInAsync()
        {
            if (_purchaseInfo.TradeIn != null)
            {
                _purchaseInfo.TradeIn.OwnerId = null;
                _purchaseInfo.TradeIn.IsNew = false;

                if (_purchaseInfo.TradeIn.Id == 0)
                {
                    await _vehicleRepo.Add(_purchaseInfo.TradeIn);
                }
                else
                {
                    await _vehicleRepo.Edit(_purchaseInfo.TradeIn);
                }
            }
        }

        private async Task UpdateSoldVehicleAsync()
        {
            _purchaseInfo.Vehicle.OwnerId = _purchaseInfo.CustomerId;
            _purchaseInfo.Vehicle.IsNew = false;
            await _vehicleRepo.Edit(_purchaseInfo.Vehicle);
        }

        public Stream GetPurchaseAgreement()
        {
            return _purchaseAgreement;
        }
    }
}
