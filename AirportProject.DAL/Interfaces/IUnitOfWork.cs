using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
    }
}
