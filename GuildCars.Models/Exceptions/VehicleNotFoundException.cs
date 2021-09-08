using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Exceptions
{
    public class VehicleNotFoundException : Exception
    {
        public VehicleNotFoundException() : base("No such vehicle found.") { }
        public VehicleNotFoundException(string message) : base(message) { }
        public VehicleNotFoundException(string message, Exception inner) : base(message, inner) { }
    }
}
