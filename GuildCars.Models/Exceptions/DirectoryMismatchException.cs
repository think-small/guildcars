using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Exceptions
{
    public class DirectoryMismatchException : Exception
    {
        public DirectoryMismatchException() : base("Paths point to different directories") { }
        public DirectoryMismatchException(string message) : base(message) { }
        public DirectoryMismatchException(string message, Exception inner) : base(message, inner) { }
    }
}
