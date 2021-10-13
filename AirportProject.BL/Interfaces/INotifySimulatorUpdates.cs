using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.BL.Interfaces
{
    public interface INotifySimulatorUpdates
    {
        Action<bool> NotifySimulatorToggled { get; set; }
    }
}
