using GuildCars.Models;
using GuildCars.Services.ReceiptGeneratorService;
using System.IO;
using System.Threading.Tasks;

namespace GuildCars.Services.SaleProcessorService
{
    internal class DealerFinanceSale : IVehicleSale
    {
        public Vehicle Vehicle { get; set; }
        public decimal PurchasePrice { get; set; }
        public Vehicle TradeIn { get; set; }
        public decimal DownPayment { get; set; }
        public int LoanLength { get; set; }
        public decimal InterestRate { get; set; }
        public string CustomerId { get; set; }
        public ILoanCalculatorService LoanCalculator { get; set; }
        private AmortizedLoanSchedule _amortizedLoanSchedule;
        public IReceiptGeneratorService ReceiptService { get; set; }

        public Stream GetPurchaseAgreement()
        {
            ReceiptService.Configure(this);
            ReceiptService.CreateHeader();
            ReceiptService.AddPurchaseInformation();
            ReceiptService.AddAmortizedSchedule(_amortizedLoanSchedule);
            ReceiptService.CreateFooter();

            return ReceiptService.GetPdf();
        }

        public void ProcessTransaction()
        {
            _amortizedLoanSchedule = LoanCalculator.GetRepaymentScheduleFor(this);
        }

        public override string ToString()
        {
            return "Dealer Finance";
        }
    }
}
