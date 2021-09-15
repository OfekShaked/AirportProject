using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces.DTO
{
    public interface IArrivalDTO
    {
        Queue<IPlaneDTO> LandingQueue
        {
            get;
            set;
        }
    
    }
}
