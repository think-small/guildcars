using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models
{
    public class LoanPaymentDetail
    {
        public int Id { get; set; }
        public DateTime DueDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public decimal Principal { get; set; }
        public decimal Interest { get; set; }
        public decimal CumulativeInterest { get; set; }
        public decimal Balance { get; set; }
        public int AmortizedLoanScheduleId { get; set; }
        public AmortizedLoanSchedule AmortizedLoanSchedule { get; set; }
    }
}
