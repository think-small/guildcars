using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Services.ReceiptGeneratorService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.SaleProcessorService
{
    internal class CashSale : IVehicleSale
    {
        public Vehicle Vehicle { get; set; }
        public decimal PurchasePrice { get; set; }
        public Vehicle TradeIn { get; set; }
        public decimal DownPayment { get; set; }
        public string CustomerId { get; set; }
        public IReceiptGeneratorService ReceiptService { get; set; }
        public void ProcessTransaction()
        {
            if (InsufficientFundsForPurchase())
                throw new InsufficientFundsException();
        }

        //  TODO: Incorporate TradeIn value when service is available to estimate car values
        private bool InsufficientFundsForPurchase()
        {
            return PurchasePrice > DownPayment;
        }

        public Stream GetPurchaseAgreement()
        {
            ReceiptService.Configure(this);
            ReceiptService.CreateHeader();
            ReceiptService.AddPurchaseInformation();
            ReceiptService.CreateFooter();

            return ReceiptService.GetPdf();
        }

        public override string ToString()
        {
            return "Cash";
        }
    }
}