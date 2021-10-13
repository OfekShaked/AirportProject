using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Commom.Interfaces
{
    public interface ISpecialTrafficQueueModel
    {
        IPlane Plane { get;}
        DateTime TimeAddedToQueue { get; }
    }
}
