using AirportProject.BL.Interfaces;
using AirportProject.BL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Departure : IDeparture
    {
        public Queue<IPlaneDTO> DepartureQueue { get; set; }

        public void AddNewPlaneToQueue(IPlane plane)
        {
            throw new NotImplementedException();
        }

        public void RemovePlaneFromQueue(IPlane plane)
        {
            throw new NotImplementedException();
        }
    }
}
