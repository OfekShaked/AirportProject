using AirportProject.Common.DataStructures;
using AirportProject.Common.Interfaces;
using AirportProject.DAL;
using AirportProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Models
{
    /// <summary>
    /// Class for managing departing planes move from station to station as fast as possible
    /// Only accepting planes and stations that have finished being handled by the station
    /// </summary>
    public class DepartureMoveStation : IDepartureMoveStation
    {
        IAirport _airport;
        IDataAccess _dataAccess;
        IUnitOfWork _uow;
        public DepartureMoveStation(IAirport airport, IDataAccess dataAccess, IUnitOfWork uow)
        {
            _dataAccess = dataAccess;
            _uow = uow;
            _airport = airport;
        }
        public void MoveToNextStation(IPlane plane, IStation currentStation = null)
        {
            if (currentStation == null)
            {
                HandleDepartureStart(plane);
            }
            else
            {
                CheckForNextStationToMove(currentStation, plane);
            }
        }
        private void HandleDepartureStart(IPlane plane)
        {
            //check number of departure starting options
            if (_airport.DepartureStartingStations.Count == 1) //only one option move to it
            {
                HandleOneNextStation(null, plane, _airport.Stations[_airport.DepartureStartingStations[0]]);
            }
            else if (_airport.DepartureStartingStations.Count > 1)//multiple options
            {
                HandleMultipleNextStations(null, plane);
            }
        }
        private void CheckForNextStationToMove(IStation currentStation, IPlane plane)
        {
            if (currentStation.ConnectedDepartureStations.Count == 0)
            {
                currentStation.SetStationFree();
                _airport.SignalPlaneFinishedDeparture(plane);
            }
            else if (currentStation.ConnectedDepartureStations.Count == 1)
            {
                HandleOneNextStation(currentStation, plane, _airport.Stations[currentStation.ConnectedDepartureStations[0]]);
            }
            else
            {
                HandleMultipleNextStations(currentStation, plane);
            }
        }
        /// <summary>
        /// Handles a case where there is only one option to move to the next station
        /// </summary>
        /// <param name="currentStation"></param>
        /// <param name="plane"></param>
        private void HandleOneNextStation(IStation currentStation, IPlane plane, IStation nextStation)
        {
            if (nextStation.IsPlaneInside() == false)
            {
                if (MoveStation(currentStation, plane, nextStation)) return;
            }
            nextStation.AddPlaneToQueue(plane);
        }
        /// <summary>
        /// Sets the current station empty and moves to the next
        /// </summary>
        /// <param name="currentStation"></param>
        /// <param name="plane"></param>
        /// <param name="nextStation"></param>
        private bool MoveStation(IStation currentStation, IPlane plane, IStation nextStation)
        {
            if (nextStation.IsPlaneInside() == false)
            {
                if (nextStation.AddPlaneToStation(plane))
                {
                    if (currentStation != null)
                    {
                        currentStation.SetStationFree();
                    }
                    else
                    {
                        _airport.SignalPlaneFinishedWaitingToDepart(plane);
                        _dataAccess.TaskQueue.AddTask(Task.Run(async () =>
                        {
                            await _dataAccess.DepartureRepository.SetDepartureStarted(plane.Id);
                            await _uow.Commit();
                            _airport.SignalDepartureOrArrivalStarted(plane.Status);
                        }));
                    }
                    return true;
                }
            }
            return false;

        }

        private void HandleMultipleNextStations(IStation currentStation, IPlane plane)
        {
            List<int> emptyConnectedStations;
            if (currentStation != null)
            {
                emptyConnectedStations = GetEmptyNextStations(currentStation);
            }
            else
            {
                emptyConnectedStations = GetEmptyDepartureStations(plane);
            }

            if (emptyConnectedStations.Count == 0) //No empty connected stations found join the queue for all connected stations
            {
                //join all connected station queues
                JoinAllQueues(currentStation, plane);
            }
            else if (emptyConnectedStations.Count == 1) //Only one connected empty station was found 
            {
                //joins the only connected station
                HandleOneNextStation(currentStation, plane, _airport.Stations[emptyConnectedStations[0]]);
            }
            else
            {
                //searches for the shortest out of all the empty stations and joins it.
                var nextShortestStation = FindShortestStationOutOfMany(emptyConnectedStations);
                if (MoveStation(currentStation, plane, nextShortestStation)) return;
                else JoinAllQueues(currentStation, plane);
            }
        }

        private void JoinAllQueues(IStation currentStation, IPlane plane)
        {
            if (currentStation != null)
            {
                foreach (var stationId in currentStation.ConnectedDepartureStations)
                {
                    _airport.Stations[stationId].AddPlaneToQueue(plane);
                }
            }
            else
            {
                foreach (var stationId in _airport.DepartureStartingStations)
                {
                    _airport.Stations[stationId].AddPlaneToQueue(plane);
                }
            }
        }

        private IStation FindShortestStationOutOfMany(List<int> emptyConnectedStations)
        {
            int minPathTime = int.MaxValue;
            Path minPath = null;
            foreach (var nextStationId in emptyConnectedStations)
            {
                foreach (var endingStationId in _airport.DepartureEndingStations)
                {
                    var currentPath = _airport.Departures.FindMinPath(nextStationId, endingStationId);
                    if (currentPath.Path.PathIndexes.Count != 0)
                    {
                        if (minPathTime > currentPath.Path.TotalWeight)
                        {
                            minPathTime = currentPath.Path.TotalWeight;
                            minPath = currentPath.Path;
                        }
                    }
                }
            }
            if (minPath == null || minPath.PathIndexes.Count == 0) throw new Exception("No path found for the plane to continue");
            return _airport.Stations[minPath.PathIndexes[0]];

        }

        private List<int> GetEmptyNextStations(IStation currentStation)
        {
            List<int> emptyStations = new List<int>();
            foreach (var stationId in currentStation.ConnectedDepartureStations)
            {
                if (_airport.Stations[stationId].IsPlaneInside() == false) emptyStations.Add(stationId);
            }
            return emptyStations;
        }
        private List<int> GetEmptyDepartureStations(IPlane plane)
        {
            List<int> emptyStations = new List<int>();
            foreach (var stationId in _airport.DepartureStartingStations)
            {
                if (_airport.Stations[stationId].IsPlaneInside(plane) == false)
                {
                    //make sure that if arrival start and departure ends at the same station airport doesnt get stuck (check for one empty station after).
                    if (_airport.ArrivalEndingStations.Contains(stationId))
                    {
                        bool isEmptyPossible = false;
                        foreach (var station in _airport.Stations[stationId].ConnectedDepartureStations)
                        {
                            if (_airport.Stations[station].IsPlaneInside(plane) == false)
                            {
                                isEmptyPossible = true;
                                break;
                            }
                        }
                        if (!isEmptyPossible) continue;
                    }
                    emptyStations.Add(stationId);
                }

            }
            return emptyStations;
        }
    }
}
