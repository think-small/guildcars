using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models
{
    public class AmortizedLoanSchedule
    {
        public int Id { get; set; }
        public List<LoanPaymentDetail> Schedule { get; set;  }
        public DateTime OriginationDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MonthlyPayment { get; set; }
    }
}
