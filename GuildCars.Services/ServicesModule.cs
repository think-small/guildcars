using Autofac;
using GuildCars.Services.FileUploadService;
using GuildCars.Services.InventoryService;
using GuildCars.Services.ReceiptGeneratorService;
using GuildCars.Services.ReportService;
using GuildCars.Services.SaleProcessorService;
using GuildCars.Services.SaleProcessorService.SaleTypes;

namespace GuildCars.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<LoanCalculatorService>().As<ILoanCalculatorService>();
            builder.RegisterType<SaleProcessorService.SaleProcessorService>().As<ISaleProcessorService>();
            builder.RegisterType<IronPdfReceiptService>().As<IReceiptGeneratorService>();
            builder.RegisterType<InventoryService.InventoryService>().As<IInventoryService>();
            builder.RegisterType<ReportService.ReportService>().As<IReportService>();
            builder.RegisterType<UploadToDiskService>().As<IFileUploadService>();
            builder.RegisterType<SaleFactory>().As<ISaleFactory>();
        }
    }
}
