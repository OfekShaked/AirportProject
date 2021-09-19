using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.Common.Interfaces
{
    public interface ISimulator
    {
        TimeSpan ArrivalsInterval
        {
            get;
            set;
        }
        TimeSpan DeparturesInterval
        {
            get;
            set;
        }
        /// <summary>
        /// Start the simulator based on the configured time and generate landings and departures
        /// </summary>
        void StartSimulator();
        /// <summary>
        /// Pauses/Stops the simulator from running
        /// </summary>
        void StopSimulator();
        /// <summary>
        /// Created a random arrival or departure
        /// </summary>
        void CreateRandomFlight();
        /// <summary>
        /// Sends a new flight to the airport
        /// </summary>
        void SendNewFlight(IPlane plane);
    }
}
