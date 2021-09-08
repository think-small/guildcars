using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GuildCars.Models.Exceptions
{
    public class CascadeDeleteException : Exception
    {
        public CascadeDeleteException() : base("Cascade delete prevented. Delete related items first, then try again.") { }
        public CascadeDeleteException(string message) : base(message) { }
        public CascadeDeleteException(string message, Exception inner) : base(message, inner) { }
    }
}
