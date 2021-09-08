using FluentAssertions;
using GuildCars.Data;
using GuildCars.Models;
using GuildCars.Services.ReceiptGeneratorService;
using GuildCars.Services.SaleProcessorService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GuildCars.Tests.Unit
{
    [TestFixture]
    public class IronPdfReceiptServiceTests
    {
        [Test]
        [Category("IronPdfReceiptService")]
        public async Task Pdf_Displays_Correct_Header()
        {
            var vehicleRepo = new VehicleRepository();
            var sut = new IronPdfReceiptService();
            sut.Configure(new CashSale
            {
                Vehicle = await vehicleRepo.GetById(5),
                TradeIn = null,
                CustomerId = "99999999-9999-9999-9999-99999999"
            });
            sut.CreateHeader();
            var actual = sut.GetPdf();

            actual.Should().NotBeNull();
        }

        [Test]
        [Category("IronPdfReceiptService")]
        public async Task Pdf_Displays_Purchase_Information()
        {
            var vehicleRepo = new VehicleRepository();
            var sut = new IronPdfReceiptService();
            sut.Configure(new CashSale
            {
                Vehicle = await vehicleRepo.GetById(5),
                TradeIn = null,
                CustomerId = "99999999-9999-9999-9999-99999999"
            });
            sut.CreateHeader();
            sut.AddPurchaseInformation();
            var actual = sut.GetPdf();

            actual.Should().NotBeNull();
        }

        [Test]
        [Category("IronPdfReceiptService")]
        public async Task Pdf_Displays_Amortized_Loan_Schedule()
        {
            var vehicleRepo = new VehicleRepository();
            var sut = new IronPdfReceiptService();

            var originationDate = new DateTime(2021, 7, 1).Date;
            var loanSchedule = new AmortizedLoanSchedule
            {
                OriginationDate = originationDate,
                EndDate = originationDate.AddYears(1),
                MonthlyPayment = 1712.15M,
                InterestRate = 0.05M,

                Schedule = new List<LoanPaymentDetail>
                {
                    new LoanPaymentDetail { DueDate = new DateTime(2021, 8, 1).Date, PaymentAmount = 1712.15M, Principal = 1628.82M, Interest = 83.33M, CumulativeInterest = 83.33M, Balance = 18371.18M },
                    new LoanPaymentDetail { DueDate = new DateTime(2021, 9, 1).Date, PaymentAmount = 1712.15M, Principal = 1635.60M, Interest = 76.55M, CumulativeInterest = 159.88M, Balance = 16735.58M },
                    new LoanPaymentDetail { DueDate = new DateTime(2021, 10, 1).Date, PaymentAmount = 1712.15M, Principal = 1642.42M, Interest = 69.73M, CumulativeInterest = 229.61M, Balance = 15093.16M },
                    new LoanPaymentDetail { DueDate = new DateTime(2021, 11, 1).Date, PaymentAmount = 1712.15M, Principal = 1649.26M, Interest = 62.89M, CumulativeInterest = 292.50M, Balance = 13443.90M },
                    new LoanPaymentDetail { DueDate = new DateTime(2021, 12, 1).Date, PaymentAmount = 1712.15M, Principal = 1656.13M, Interest = 56.02M, CumulativeInterest = 348.52M, Balance = 11787.77M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 1, 1).Date, PaymentAmount = 1712.15M, Principal = 1663.03M, Interest = 49.12M, CumulativeInterest = 397.63M, Balance = 10124.73M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 2, 1).Date, PaymentAmount = 1712.15M, Principal = 1669.96M, Interest = 42.19M, CumulativeInterest = 439.82M, Balance = 8454.77M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 3, 1).Date, PaymentAmount = 1712.15M, Principal = 1676.92M, Interest = 35.23M, CumulativeInterest = 475.05M, Balance = 6777.85M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 4, 1).Date, PaymentAmount = 1712.15M, Principal = 1683.91M, Interest = 28.24M, CumulativeInterest = 503.29M, Balance = 5093.94M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 5, 1).Date, PaymentAmount = 1712.15M, Principal = 1690.92M, Interest = 21.22M, CumulativeInterest = 524.51M, Balance = 3403.02M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 6, 1).Date, PaymentAmount = 1712.15M, Principal = 1697.97M, Interest = 14.18M, CumulativeInterest = 538.69M, Balance = 1705.05M },
                    new LoanPaymentDetail { DueDate = new DateTime(2022, 7, 1).Date, PaymentAmount = 1712.15M, Principal = 1705.05M, Interest = 7.10M, CumulativeInterest = 545.80M, Balance = 0.00M }
                }
            };
            sut.Configure(new DealerFinanceSale
            {
                Vehicle = await vehicleRepo.GetById(5),
                TradeIn = null,
                CustomerId = "99999999-9999-9999-9999-99999999"
            });
            sut.CreateHeader();
            sut.AddPurchaseInformation();
            sut.AddAmortizedSchedule(loanSchedule);
            var actual = sut.GetPdf();

            actual.Should().NotBeNull();
        }

        [Test]
        [Category("IronPdfReceiptService")]
        public async Task Pdf_Displays_Signature_Section_In_Footer()
        {
            var vehicleRepo = new VehicleRepository();
            var sut = new IronPdfReceiptService();

            sut.Configure(new CashSale
            {
                Vehicle = await vehicleRepo.GetById(5),
                TradeIn = null,
                CustomerId = "99999999-9999-9999-9999-99999999"
            });
            sut.CreateFooter();
            sut.GetPdf();
        }
    }
}
