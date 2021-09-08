using FluentAssertions;
using GuildCars.Models;
using GuildCars.Services;
using GuildCars.Services.SaleProcessorService;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Tests.Unit
{
    [TestFixture]
    public class LoanCalculatorServiceTests
    {
        [Test]
        [Category("GetRepaymentScheduleFor")]
        public void CanGetCorrectAmortizedLoanSchedule_WhenGivenValidDealerFinanceSaleWithNoTradeInAndNoDownPayment()
        {
            var loanService = new LoanCalculatorService();
            var sale = new DealerFinanceSale
            {
                PurchasePrice = 200000,
                DownPayment = 0,
                LoanLength = 30,
                InterestRate = 0.05M
            };
            AmortizedLoanSchedule schedule = loanService.GetRepaymentScheduleFor(sale);
            var actualFirstPayment = schedule.Schedule[0];
            var actualLastPayment = schedule.Schedule[359];

            var expectedFirstPayment = new LoanPaymentDetail
            {
                PaymentAmount = 1073.64M,
                Principal = 240.31M,
                Interest = 833.33M,
                CumulativeInterest = 833.33M,
                Balance = 199759.69M
            };
            var expectedLastPayment = new LoanPaymentDetail
            {
                PaymentAmount = 1073.64M,
                Principal = 1069.19M,
                Interest = 4.45M,
                CumulativeInterest = 186511.57M,
                Balance = 0
            };

            schedule.Should().BeOfType(typeof(AmortizedLoanSchedule));
            schedule.MonthlyPayment.Should().Be(1073.64M);
            schedule.InterestRate.Should().Be(sale.InterestRate);
            schedule.Schedule.Should().NotBeNullOrEmpty().And.HaveCount(360);

            actualFirstPayment.PaymentAmount.Should().Be(expectedFirstPayment.PaymentAmount);
            actualFirstPayment.Principal.Should().Be(expectedFirstPayment.Principal);
            actualFirstPayment.Interest.Should().Be(expectedFirstPayment.Interest);
            actualFirstPayment.CumulativeInterest.Should().Be(expectedFirstPayment.CumulativeInterest);
            actualFirstPayment.Balance.Should().Be(expectedFirstPayment.Balance);

            actualLastPayment.PaymentAmount.Should().Be(expectedLastPayment.PaymentAmount);
            actualLastPayment.Principal.Should().Be(expectedLastPayment.Principal);
            actualLastPayment.Interest.Should().Be(expectedLastPayment.Interest);
            actualLastPayment.CumulativeInterest.Should().Be(actualLastPayment.CumulativeInterest);
            actualLastPayment.Balance.Should().Be(expectedLastPayment.Balance);
        }
    }
}
