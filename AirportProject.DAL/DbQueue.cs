using AirportProject.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL
{
    public class DbQueue : IDbQueue
    {
        Queue<Task> _tasksToPerform;
        bool _isRunning = false;
        public DbQueue()
        {
            _tasksToPerform = new Queue<Task>();
        }
        public void AddTask(Task task)
        {
            _tasksToPerform.Enqueue(task);
            if (!_isRunning&&_tasksToPerform.Count!=0) RunTask();
        }
        private void RunTask()
        {
            _isRunning = true;
            do
            {
                _tasksToPerform.Dequeue().Wait();
            }
            while (_tasksToPerform.Count != 0);
            _isRunning = false;
        }

    }
}
