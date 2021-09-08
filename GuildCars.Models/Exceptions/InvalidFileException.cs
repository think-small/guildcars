using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Exceptions
{
    public class InvalidFileException : Exception
    {
        public InvalidFileException() : base("Invalid file") { }
        public InvalidFileException(string message) : base(message) { }
        public InvalidFileException(string message, Exception inner) : base(message, inner) { }
    }
}
