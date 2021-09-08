using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Models.SaleProcessServiceModels;
using GuildCars.Services.FileUploadService;
using GuildCars.Services.ReceiptGeneratorService;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.SaleProcessorService
{
    internal class BankFinanceSale : IVehicleSale
    {
        public Vehicle Vehicle { get; set; }
        public decimal PurchasePrice { get; set; }
        public Vehicle TradeIn { get; set; }
        public string CustomerId { get; set; }
        public decimal DownPayment { get; set; }
        public int LoanLength { get; set; }
        public decimal InterestRate { get; set; }
        public FileUploadArgs ApprovalLetter { get; set; }
        public decimal ApprovalAmount { get; set; }
        public IFileUploadService FileUploader{ get; set; }
        public IReceiptGeneratorService ReceiptService { get; set; }
        public Stream GetPurchaseAgreement()
        {
            ReceiptService.Configure(this);
            ReceiptService.CreateHeader();
            ReceiptService.AddPurchaseInformation();
            ReceiptService.AddApprovalLetter();
            ReceiptService.CreateFooter();
            return ReceiptService.GetPdf();
        }

        public void ProcessTransaction()
        {
            if (InsufficientFundsForPurchase())
                throw new InsufficientFundsException();

            try
            {
                FileUploader.SaveAsync(ApprovalLetter);
            }
            catch (NullReferenceException ex)
            {
                throw new SaleInformationMissingException("Approval letter missing. Unable to process transaction", ex);
            }
        }

        //  TODO: Once estimate trade-in value service is completed, include it in this calculation
        private bool InsufficientFundsForPurchase()
        {
            return PurchasePrice > DownPayment + ApprovalAmount;
        }

        public override string ToString()
        {
            return "Bank Finance";
        }
    }
}
