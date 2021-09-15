using AirportProject.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AirportProject.BL.Models
{
    public class Simulator : ISimulator
    {
        public TimeSpan ArrivalsInterval { get; set; }
        public TimeSpan DeparturesInterval { get; set; }
        public TimeSpan ArrivalInterval { get; set; }
        private IAirport _airport;
        private bool _isTimerRandom = false;
        private static readonly TimeSpan defaultTime = new TimeSpan(0, 0, 2);
        private static Timer arrivalGenerator;
        private static Timer departureGenerator;
        private static Random rnd = new Random();

        public Simulator(bool isTimerRandom, IAirport airport, TimeSpan? arrivalInterval = null, TimeSpan? departureInterval = null)
        {
            _airport = airport;
            if (isTimerRandom == false) SetSimulatorIntervals(arrivalInterval, departureInterval);
            else SetRandomIntervals();
        }


        public void CreateRandomFlight()
        {
            int randomStatus = rnd.Next(1, 3);
            if(randomStatus == 1)
            {
                CreateNewArrival_Elapsed(null, null);
            }
            else
            {
                CreateNewDeparture_Elapsed(null, null);
            }
        }

        public void SendNewFlight(IPlane plane)
        {
            _airport.GetPlaneFromSimulator(plane);
        }

        public void StartSimulator()
        {
            arrivalGenerator.Start();
            departureGenerator.Start();
        }
        public void StopSimulator()
        {
            arrivalGenerator.Stop();
            departureGenerator.Stop();
        }

        private void ConfigureTimerSettings()
        {
            arrivalGenerator.Elapsed += CreateNewArrival_Elapsed;
            departureGenerator.Elapsed += CreateNewDeparture_Elapsed;
            arrivalGenerator.AutoReset = true;
            departureGenerator.AutoReset = true;
        }
        private void SetRandomIntervals()
        {
            _isTimerRandom = true;
            arrivalGenerator.Interval = rnd.Next(1000, 20000);
            departureGenerator.Interval = rnd.Next(1000, 20000);
            ConfigureTimerSettings();
        }

        private void CreateNewDeparture_Elapsed(object sender, ElapsedEventArgs e)
        {
            IPlane departingPlane = new Plane { Id = Guid.NewGuid().ToString(), Name = RandomPlaneNameGenerator(), Status = Enums.PlaneStatus.Departure };
            SendNewFlight(departingPlane);
        }

        private void CreateNewArrival_Elapsed(object sender, ElapsedEventArgs e)
        {
            IPlane arrivingPlane = new Plane { Id = Guid.NewGuid().ToString(), Name = RandomPlaneNameGenerator(), Status = Enums.PlaneStatus.Arrival };
            SendNewFlight(arrivingPlane);
        }

        private void SetSimulatorIntervals(TimeSpan? arrivalInterval, TimeSpan? departureInterval)
        {
            if (arrivalInterval == null) ArrivalInterval = defaultTime;
            else ArrivalInterval = (TimeSpan)arrivalInterval;
            if (departureInterval == null) DeparturesInterval = defaultTime;
            else DeparturesInterval = (TimeSpan)departureInterval;
            arrivalGenerator.Interval = ArrivalInterval.TotalMilliseconds;
            departureGenerator.Interval = DeparturesInterval.TotalMilliseconds;
            ConfigureTimerSettings();
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
    }
}
