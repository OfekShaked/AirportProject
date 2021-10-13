using AirportProject.BL.Interfaces;
using AirportProject.Common.Enums;
using AirportProject.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    public class Simulator : ISimulator
    {
        public TimeSpan ArrivalsInterval { get; set; }
        public TimeSpan DeparturesInterval { get; set; }
        public TimeSpan ArrivalInterval { get; set; }
        private object _lockArrivalTimer = new object();
        private object _lockDepartureTimer = new object();
        private IAirport _airport;
        private INotifySimulatorUpdates _notifySimulatorUpdates;
        private bool _isTimerRandom = false;
        private static readonly TimeSpan defaultTime = new TimeSpan(0, 0, 2);
        private Timer arrivalGenerator;
        private Timer departureGenerator;
        private TimerCallback arrivalCallback;
        private TimerCallback departureCallback;
        private static Random rnd = new Random();
        private bool isEnabled = false;

        public Simulator(bool isTimerRandom, IAirport airport, INotifySimulatorUpdates notifySimulatorUpdates, TimeSpan? arrivalInterval = null, TimeSpan? departureInterval = null)
        {
            _airport = airport;
            _notifySimulatorUpdates = notifySimulatorUpdates;
            _isTimerRandom = isTimerRandom;
            arrivalCallback = new TimerCallback(CreateNewArrival_Elapsed);
            departureCallback = new TimerCallback(CreateNewDeparture_Elapsed);
        }


        public void CreateRandomFlight()
        {
            int randomStatus = rnd.Next(1, 3);
            if (randomStatus == 1)
            {
                CreateNewArrival_Elapsed(null);
            }
            else
            {
                CreateNewDeparture_Elapsed(null);
            }
        }

        public void SendNewFlight(IPlane plane)
        {
            _airport.GetPlaneFromSimulator(plane);
        }

        public void StartSimulator()
        {
            try
            {
                isEnabled = true;
                arrivalGenerator = new Timer(CreateNewArrival_Elapsed, null, 1000, rnd.Next(1000, 10000));
                departureGenerator = new Timer(CreateNewDeparture_Elapsed, null, 1000, rnd.Next(1000, 10000));
                if (_notifySimulatorUpdates != null) _notifySimulatorUpdates.NotifySimulatorToggled.Invoke(true);

            }
            catch (Exception e)
            {
                e = e;
            }
        }
        public void StopSimulator()
        {
            try
            {
                isEnabled = false;
                arrivalGenerator.Dispose();
                departureGenerator.Dispose();
                if (_notifySimulatorUpdates != null) _notifySimulatorUpdates.NotifySimulatorToggled.Invoke(false);
            }
            catch (Exception e)
            {
                e = e;
            }
        }

        private void CreateNewDeparture_Elapsed(object stateInfo)
        {
            try
            {
                if (isEnabled == false) return;
                IPlane departingPlane = new Plane("qw", RandomPlaneNameGenerator(),  PlaneStatus.Departure );
                SendNewFlight(departingPlane);
            }
            catch (Exception ex)
            {

            }
        }


        private void CreateNewArrival_Elapsed(Object stateInfo)
        {

            try
            {
                if (isEnabled == false) return;
                IPlane arrivingPlane = new Plane { Id = Guid.NewGuid().ToString(), Name = RandomPlaneNameGenerator(), Status = PlaneStatus.Arrival };
                SendNewFlight(arrivingPlane);
            }
            catch (Exception ex)
            {

            }

        }
        private string RandomPlaneNameGenerator()
        {
            string[] consonants = { "b", "c", "d", "f", "g", "h", "j", "k", "l", "m", "l", "n", "p", "q", "r", "s", "sh", "zh", "t", "v", "w", "x" };
            string[] vowels = { "a", "e", "i", "o", "u", "ae", "y" };
            string Name = "";
            Name += consonants[rnd.Next(consonants.Length)].ToUpper();
            Name += vowels[rnd.Next(vowels.Length)];
            int b = 2;
            while (b < 5)
            {
                Name += consonants[rnd.Next(consonants.Length)];
                b++;
                Name += vowels[rnd.Next(vowels.Length)];
                b++;
            }

            return Name;
        }
        public bool IsSimulatorRunning()
        {
            return isEnabled;
        }
    }
}
