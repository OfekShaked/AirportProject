using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    class Arrival : IArrival
    {
        public List<IPlane> Landings { get; set; }
        public Arrival()
        {
            Landings = new List<IPlane>();
        }
        public void AddNewPlaneToQueue(IPlane plane)
        {
            Landings.Add(plane);
        }

        public void RemovePlaneFromQueue(IPlane plane)
        {
            Landings.Remove(plane);
        }
    }
}
