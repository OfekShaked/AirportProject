using AirportProject.BL.Interfaces.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface IStationDTO
    {
        int Id
        {
            get;
            set;
        }
        string Name
        {
            get;
            set;
        }
        IPlane CurrentPlaneInside
        {
            get;
            set;
        }
        TimeSpan HandlingTime
        {
            get;
            set;
        }
        List<int> ConnectedDepartureStations
        {
            get;
            set;
        }
        List<int> ConnectedArrivalStations
        {
            get;
            set;
        }

    }
}
