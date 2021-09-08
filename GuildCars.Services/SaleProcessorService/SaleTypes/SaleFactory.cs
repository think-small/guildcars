using GuildCars.Models;
using GuildCars.Models.Exceptions;
using GuildCars.Services.FileUploadService;
using GuildCars.Services.ReceiptGeneratorService;
using System;

namespace GuildCars.Services.SaleProcessorService.SaleTypes
{
    internal class SaleFactory : ISaleFactory
    {
        private readonly ILoanCalculatorService _loanCalculator;
        private readonly IFileUploadService _fileUploader;
        private readonly IReceiptGeneratorService _receiptService;
        public SaleFactory(ILoanCalculatorService loanCalculator, IFileUploadService fileUploader, IReceiptGeneratorService receiptService)
        {
            _loanCalculator = loanCalculator;
            _fileUploader = fileUploader;
            _receiptService = receiptService;
        }

        public IVehicleSale GetVehicleSaleFor(SaleProcessServiceArgs purchaseInfo )
        {
            switch(purchaseInfo.PurchaseTypeId)
            {
                case 1:
                    return new CashSale
                    {
                        Vehicle = purchaseInfo.Vehicle,
                        PurchasePrice = purchaseInfo.PurchasePrice,
                        TradeIn = purchaseInfo.TradeIn,
                        DownPayment = purchaseInfo.DownPayment ?? 0M,
                        CustomerId = purchaseInfo.CustomerId ?? throw new SaleInformationMissingException(),
                        ReceiptService = _receiptService
                    };                    
                case 2:

                    return new BankFinanceSale
                    {
                        Vehicle = purchaseInfo.Vehicle,
                        PurchasePrice = purchaseInfo.PurchasePrice,
                        TradeIn = purchaseInfo.TradeIn,
                        CustomerId = purchaseInfo.CustomerId ?? throw new SaleInformationMissingException(),
                        DownPayment = purchaseInfo.DownPayment.Value,
                        LoanLength = purchaseInfo.LoanLength ?? throw new SaleInformationMissingException(),
                        InterestRate = purchaseInfo.InterestRate ?? throw new SaleInformationMissingException(),
                        ApprovalLetter = purchaseInfo.ApprovalLetter,
                        ApprovalAmount = purchaseInfo.ApprovalAmount ?? throw new SaleInformationMissingException(),
                        FileUploader = _fileUploader,
                        ReceiptService = _receiptService
                    };                   
                case 3:

                    return new DealerFinanceSale
                    {
                        Vehicle = purchaseInfo.Vehicle,
                        PurchasePrice = purchaseInfo.PurchasePrice,
                        TradeIn = purchaseInfo.TradeIn,
                        DownPayment = purchaseInfo.DownPayment ?? throw new SaleInformationMissingException(),
                        LoanLength = purchaseInfo.LoanLength ?? throw new SaleInformationMissingException(),
                        InterestRate = purchaseInfo.InterestRate ?? throw new SaleInformationMissingException(),
                        CustomerId = purchaseInfo.CustomerId ?? throw new SaleInformationMissingException(),
                        LoanCalculator = _loanCalculator,
                        ReceiptService = _receiptService
                    };
                default:
                    throw new SaleInformationMissingException("Invalid sale type");
            }
        }
    }
}
