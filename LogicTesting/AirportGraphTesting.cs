using AirportProject.BL.DataStructures;
using AirportProject.BL.Interfaces;
using AirportProject.BL.Models;
using System;
using System.Collections.Generic;
using Xunit;
using Xunit.Abstractions;

namespace LogicTesting
{
    public class AirportGraphTesting
    {
        [Fact]
        public void TestDefaultDeparturePathFromStation5()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            IEnumerable<Path> departuresPathsFrom6 = airport.Departures.FindAllPaths(5, 3);
            int countOfPaths = 0;
            foreach (var item in departuresPathsFrom6)
            {
                countOfPaths++;
                Assert.True(item.VerticesIndexes.First.Value == 5);
                Assert.True(item.VerticesIndexes.First.Next.Value == 7);
                Assert.True(item.VerticesIndexes.Last.Value == 3);
            }
            Assert.True(countOfPaths == 1);
        }
        [Fact]
        public void TestDefaultDeparturePathFromStation6()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            IEnumerable<Path> departuresPathsFrom6 = airport.Departures.FindAllPaths(6, 3);
            int countOfPaths = 0;
            foreach (var item in departuresPathsFrom6)
            {
                countOfPaths++;
                Assert.True(item.VerticesIndexes.First.Value == 6);
                Assert.True(item.VerticesIndexes.First.Next.Value == 7);
                Assert.True(item.VerticesIndexes.Last.Value == 3);
            }
            Assert.True(countOfPaths == 1);
        }
        [Fact]
        public void TestDefaultArrivalsPathFromStation0To5()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            IEnumerable<Path> departuresPathsFrom6 = airport.Arrivals.FindAllPaths(0, 5);
            int countOfPaths = 0;
            foreach (var item in departuresPathsFrom6)
            {
                countOfPaths++;
                var currentStation = item.VerticesIndexes.First;
                Assert.True(currentStation.Value == 0);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 1);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 2);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 3);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 4);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 5);
            }
            Assert.True(countOfPaths == 1);
        }

        [Fact]
        public void TestDefaultArrivalsPathFromStation0To6()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            IEnumerable<Path> departuresPathsFrom6 = airport.Arrivals.FindAllPaths(0, 6);
            int countOfPaths = 0;
            foreach (var item in departuresPathsFrom6)
            {
                countOfPaths++;
                var currentStation = item.VerticesIndexes.First;
                Assert.True(currentStation.Value == 0);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 1);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 2);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 3);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 4);
                currentStation = currentStation.Next;
                Assert.True(currentStation.Value == 6);
            }
            Assert.True(countOfPaths == 1);
        }
        [Fact]
        public void TestFindMinPathFromStation0To6()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            var arrivalPathFrom0 = airport.Arrivals.FindMinPath(0, 6);
            Assert.True(arrivalPathFrom0.Path.PathIndexes.Count==6);
        }
        [Fact]
        public void TestPathNotFound()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            var arrivalPathFrom0 = airport.Arrivals.FindMinPath(0, 7);
            Assert.True(arrivalPathFrom0.Path.PathIndexes.Count == 0);
        }
        [Fact]
        public void FindMinPathFrom0To6()
        {
            IAirport airport = new Airport();
            airport.CreateNewAirport();
            var departuresPathsFrom6 = airport.Arrivals.FindMinPath(0, 6);
            var path = departuresPathsFrom6.Path.PathIndexes;
                Assert.True(path[0] == 0);
                Assert.True(path[1] == 1);
                Assert.True(path[2] == 2);
                Assert.True(path[3] == 3);
                Assert.True(path[4] == 4);
                Assert.True(path[5] == 6);
            
        }

    }
}
