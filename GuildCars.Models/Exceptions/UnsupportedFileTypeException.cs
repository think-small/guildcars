using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Exceptions
{
    public class UnsupportedFileTypeException : Exception
    {
        public UnsupportedFileTypeException() : base("Unsupported file type.") { }
        public UnsupportedFileTypeException(string message) : base(message) { }
        public UnsupportedFileTypeException(string message, Exception inner) : base(message, inner) { }
    }
}
