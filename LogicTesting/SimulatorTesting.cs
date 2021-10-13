using AirportProject.BL.Models;
using AirportProject.Common.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;


namespace LogicTesting
{
    public class SimulatorTesting
    {
        ISimulator _simulator;
        public Mock<IAirport> _airport = new Mock<IAirport>();
        public SimulatorTesting()
        {
            _simulator = new Simulator(false,_airport.Object,null);
        }
        [Fact]
        public async void TestDefaultFlightsCreated()
        {
            _simulator.StartSimulator();
            Assert.True(_simulator.IsSimulatorRunning());
            await Task.Delay(4000);
            _airport.Verify(m => m.GetPlaneFromSimulator(It.Is<IPlane>(p=>p!=null)),Times.AtLeast(2));
            
        }
    }

}
