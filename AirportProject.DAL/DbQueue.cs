using AirportProject.DAL.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportProject.DAL
{
    public class DbQueue : IDbQueue
    {
        ConcurrentQueue<Task> _tasksToPerform;
        bool _isRunning = false;
        public DbQueue()
        {
            _tasksToPerform = new ConcurrentQueue<Task>();
        }
        public void AddTask(Task task)
        {
            _tasksToPerform.Enqueue(task);
            if (!_isRunning && _tasksToPerform.Count != 0) RunTask();
        }
        private void RunTask()
        {
            _isRunning = true;
            do
            {
                _tasksToPerform.TryDequeue(out Task task);
                if (task != null)
                {
                    task.Wait();
                }
            }
            while (_tasksToPerform.Count != 0);
            _isRunning = false;
        }

    }
}
