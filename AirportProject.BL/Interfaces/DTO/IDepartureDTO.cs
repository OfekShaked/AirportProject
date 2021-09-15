using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces.DTO
{
    public interface IDepartureDTO
    {
        Queue<IPlaneDTO> DepartureQueue
        {
            get;
            set;
        }
    }
}
