using System;

namespace GuildCars.Models.Exceptions
{
    public class InvalidInventoryFilterException : Exception
    {
        public InvalidInventoryFilterException() : base("Missing inventory filter information. Make sure collections are initialized.") { }
        public InvalidInventoryFilterException(string message) : base(message) { }
        public InvalidInventoryFilterException(string message, Exception inner) : base(message, inner) { }
    }
}
