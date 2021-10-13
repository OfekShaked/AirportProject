using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Plane : IPlane
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public PlaneStatus Status { get; set; }
        private IStation _currentStation;
        public IStation CurrentStation {
            get
            {
                return _currentStation;
            }
            set
            {
                _currentStation = value;
                SetCurrentStation(value);
            }
        }
        public int CurrentStationId { get; set; }
        public Plane()
        {
            SetCurrentStation(null);
        }
        public Plane(string id, string name,PlaneStatus status)
        {
            Id = id;
            Name = name;
            Status = status;
            CurrentStation = null;        
        }
        public void SetCurrentStation(IStation station)
        {
            if (station==null)
            {
                switch (Status)
                {
                    case PlaneStatus.Arrival: CurrentStationId = -1; break;
                    case PlaneStatus.Departure: CurrentStationId = -2; break;
                    default: CurrentStationId = -3; break;
                }
            }
            else
            {
                CurrentStationId = station.Id;
            }
        }
    }
}
