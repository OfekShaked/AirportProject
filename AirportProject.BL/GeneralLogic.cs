using AirportProject.BL.Interfaces;
using AirportProject.BL.Models;
using AirportProject.Common.Interfaces;
using AirportProject.DAL.Interfaces;
using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL
{
    public class GeneralLogic
    {
        private IDataAccess _dataAccess;
        private DTOMapper _mapper;
        private IUnitOfWork _uow;
        private INotifyUpdates _notifyUpdates;
        private INotifySimulatorUpdates _notifySimulatorUpdates;
        public IAirport Airport;
        private ISimulator _simulator;


        public GeneralLogic(IDataAccess dataAccess, IUnitOfWork uow)
        {
            _dataAccess = dataAccess;
            _notifyUpdates = null;
            _notifySimulatorUpdates = null;
            _mapper = new DTOMapper();
            _uow = uow;
        }
        public void CreateBasicAirport()
        {
            IAirport airport;
            Task.Run(async () => {
                var airportDTO = await _dataAccess.AirportRepository.GetAirport();
                if (airportDTO == null)
                {
                    airport = new Airport(_dataAccess, _uow, null, _notifyUpdates);
                }
                else
                {
                    airport = new Airport(_dataAccess, _uow, _mapper.AirportDTOToAirport(airportDTO), _notifyUpdates);
                    var plannedDepartures = await _dataAccess.DepartureRepository.GetAllWaitingDepartures();
                    var plannedArrivals = await _dataAccess.ArrivalRepository.GetAllWaitingArrivals();
                    await _dataAccess.DepartureRepository.RemoveAllWaitingDepartures();
                    await _dataAccess.ArrivalRepository.RemoveAllWaitingArrivals();
                    foreach (var item in plannedDepartures)
                    {
                        airport.GetPlaneFromSimulator(_mapper.PlaneDTOToPlane(await _dataAccess.PlaneRepository.GetById(item.PlaneId)));
                        await Task.Delay(100);
                    }
                    foreach (var item in plannedArrivals)
                    {
                        airport.GetPlaneFromSimulator(_mapper.PlaneDTOToPlane(await _dataAccess.PlaneRepository.GetById(item.PlaneId)));
                        await Task.Delay(100);
                    }
                }
                Airport = airport;
                _simulator = new Simulator(false, Airport, _notifySimulatorUpdates);
            });
        }
        public AirportDTO GetAirport()
        {
            while (Airport == null)
            {

            }
            return _mapper.AirportToAirportDTO((Airport)Airport);
        }
        public async Task<List<ArrivalDTO>> GetFutureArrivals()
        {
            return await _dataAccess.ArrivalRepository.GetAllFutureArrivals();
        }
        public async Task<List<DepartureDTO>> GetFutureDepartures()
        {
            return await _dataAccess.DepartureRepository.GetAllFutureDepartures();
        }
        public void StartSimulator()
        {
            if(_simulator!=null)
            _simulator.StartSimulator();
        }
        public void StopSimulator()
        {
            if (_simulator != null)
                _simulator.StopSimulator();
        }
        public bool IsSimulatorRunning()
        {
            if (_simulator == null) return false;
            return _simulator.IsSimulatorRunning();
        }
        public void SetNotifyUpdated(INotifyUpdates notifyUpdates)
        {
            _notifyUpdates = notifyUpdates;
        }
        public void SetNotifySimulatorUpdates(INotifySimulatorUpdates notifySimulatorUpdates)
        {
            _notifySimulatorUpdates = notifySimulatorUpdates;
        }
    }
}
