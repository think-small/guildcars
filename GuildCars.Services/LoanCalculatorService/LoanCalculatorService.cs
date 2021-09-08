using GuildCars.Models;
using GuildCars.Services.SaleProcessorService;
using System;
using System.Collections.Generic;

namespace GuildCars.Services
{
    internal class LoanCalculatorService : ILoanCalculatorService
    {
        int _paymentsPerYear = 12;
        int _numberOfPaymentsForLifeOfLoan;
        decimal _principal;
        decimal _roundedMonthlyPayment;
        decimal _monthlyPayment;
        decimal _monthlyInterestRate;

        public AmortizedLoanSchedule GetRepaymentScheduleFor(DealerFinanceSale loan)
        {
            _principal = loan.PurchasePrice - (loan.DownPayment);  //  TODO: Include a service for fetching loan.TradeIn's value and include in calculation
            _monthlyInterestRate = loan.InterestRate / _paymentsPerYear;
            _numberOfPaymentsForLifeOfLoan = loan.LoanLength * _paymentsPerYear;
            var accruedInterest = CalculateAccruedInterest();
            var now = DateTime.Now;

            _monthlyPayment = CalculateMonthlyPayment(accruedInterest);
            _roundedMonthlyPayment = Math.Round(_monthlyPayment, 2);

            return new AmortizedLoanSchedule
            {
                OriginationDate = now,
                EndDate = now.AddYears(loan.LoanLength),
                MonthlyPayment = _roundedMonthlyPayment,
                InterestRate = loan.InterestRate,
                Schedule = GenerateSchedule()
            };
        }

        private decimal CalculateAccruedInterest()
        {
            return (decimal)Math.Pow((double)(1 + _monthlyInterestRate), _numberOfPaymentsForLifeOfLoan);
        }

        private decimal CalculateMonthlyPayment(decimal accruedInterest)
        {
            return _principal * (_monthlyInterestRate * accruedInterest / (accruedInterest - 1));
        }

        private List<LoanPaymentDetail> GenerateSchedule()
        {
            var output = new List<LoanPaymentDetail>();
            DateTime dueDate = DateTime.Now;
            var currentBalance = _principal;
            var accruedInterest = 0M;

            for (int i = 0; i < _numberOfPaymentsForLifeOfLoan; i++)
            {
                dueDate = dueDate.AddMonths(1);
                accruedInterest += MonthlyInterestDueOn(currentBalance);

                var nextPaymentDetail = new LoanPaymentDetail
                {
                    DueDate = dueDate,
                    PaymentAmount = _roundedMonthlyPayment,
                    Interest = Math.Round(MonthlyInterestDueOn(currentBalance), 2),
                    Principal = Math.Round(PaymentTowards(currentBalance), 2),
                    Balance = Math.Round(CalculateNewBalance(currentBalance), 2),
                    CumulativeInterest = Math.Round(accruedInterest, 2)
                };

                output.Add(nextPaymentDetail);
                currentBalance = CalculateNewBalance(currentBalance);
            }

            return output;
        }

        private decimal CalculateNewBalance(decimal currentBalance)
        {
            return currentBalance - PaymentTowards(currentBalance);
        }

        private decimal PaymentTowards(decimal currentBalance)
        {
            return _monthlyPayment - (currentBalance * _monthlyInterestRate);            
        }

        private decimal MonthlyInterestDueOn(decimal currentBalance)
        {
            return _monthlyPayment - PaymentTowards(currentBalance);
        }
    }
}
