﻿using AirportProject.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IArrivalRepository : IRepository<ArrivalDTO>
    {
        Task AddArrival (string planeId);
        Task SetArrivalFinished(string planeId);
    }
}
