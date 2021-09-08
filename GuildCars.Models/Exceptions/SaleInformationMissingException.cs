using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Exceptions
{
    public class SaleInformationMissingException : Exception
    {
        public SaleInformationMissingException() : base("Sale information was missing. Unable to process transaction.") { }
        public SaleInformationMissingException(string message) : base(message) { }
        public SaleInformationMissingException(string message, Exception inner) : base(message, inner) { }
    }
}
