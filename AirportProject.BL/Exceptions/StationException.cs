using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Exceptions
{
    public class StationException : Exception
    {
        public StationException()
        {

        }
        public StationException(string message)
        : base(message)
        {
        }

        public StationException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
