using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Departure : IDeparture
    {
        public List<IPlane> Departures { get; set; }
        public Departure()
        {
            Departures = new List<IPlane>();
        }

        public void AddNewPlaneToQueue(IPlane plane)
        {
            Departures.Add(plane);
        }

        public void RemovePlaneFromQueue(IPlane plane)
        {
            Departures.Remove(plane);
        }
    }
}
