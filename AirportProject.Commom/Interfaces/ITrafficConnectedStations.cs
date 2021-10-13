using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Commom.Interfaces
{
    public interface ITrafficConnectedStations
    {
        LinkedList<int> StationsInPath { get; set; }
        string Name { get; set; }
        bool IsBlocked { get; set; }
        int FirstArrivalStation { get; set; }
        int LastArrivalStation { get; set; }
        int FirstDepartureStation { get; set; }
        int LastDepartureStation { get; set; }
        Queue<ISpecialTrafficQueueModel> PlanesWaiting { get; set; }
        IPlane CurrentPlaneInside { get; set; }
        void EnterStation(IStation stationToEnter, IPlane plane);
        void ExitStation(IStation stationToExit, IPlane plane);
        bool CompareNameToList(List<int> stations);
        void SetName(List<int> stations);
        void AddToQueue(IPlane plane);
        void RemoveFromQueue(IPlane plane);
    }
}
