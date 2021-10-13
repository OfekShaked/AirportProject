using AirportProject.Commom.Interfaces;
using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class SpecialTrafficQueueModel : ISpecialTrafficQueueModel
    {
        public IPlane Plane { get; private set; }
        public DateTime TimeAddedToQueue { get; private set; }
        public SpecialTrafficQueueModel(IPlane plane)
        {
            TimeAddedToQueue = DateTime.Now;
            Plane = plane;
        }
    }
}
