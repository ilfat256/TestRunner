using Libraries.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libraries
{
    public class MultiThreadRunner<T> where T : class
    {
        private readonly int _parallelTaskCount = 0;
        private readonly Task<string>[] _tasks;
        private Queue<T> _commands;
        private readonly ILogger _log = ConcurrentLogAccumulator.GetLogger;
        private int _finishedTasks = 0;
        private IJobManager<T> _jobManager;

        public MultiThreadRunner(int parallelTaskCount, IJobManager<T> jobManager)
        {
            _parallelTaskCount = parallelTaskCount;
            _tasks = new Task<string>[parallelTaskCount];
            _jobManager = jobManager;
        }

        public void SetCommands(Queue<T> commands)
        {
            _finishedTasks = 0;
            _commands = commands;
        }

        public List<string> Start()
        {
            try
            {
                var commandsCount = _commands.Count();
                do
                {
                    ProcessFinishedTasks();
                    StartingNewTasks();
                    WaitToCompleteAnyTask();
                }
                while (_finishedTasks < commandsCount);
                return _log.Release();
            }
            catch (Exception ex)
            {                
                throw new MultiThreadRunnerException("MultiThreadRunner throws exception", ex);
            }
        }

        private void ProcessFinishedTasks()
        {
            for (int i = 0; i < _parallelTaskCount; i++)
            {
                var task = _tasks[i];

                if (task != null && (task.IsCompleted || task.IsFaulted || task.IsCanceled))
                {
                    _finishedTasks++;
                    _tasks[i] = null;
                }
            };
        }

        private void StartingNewTasks()
        {
            for (int i = 0; i < _parallelTaskCount; i++)
            {
                var task = _tasks[i];
                if (task == null)
                {
                    T command = GetNextCommand();

                    if (command == null)
                    {
                        break;
                    }
                    _tasks[i] = Task.Run(() => { return ExecuteCommand(command); });
                }
            }
        }

        private T GetNextCommand()
        {
            if (_commands.Count() > 0)
            {
                return _commands.Dequeue();
            }
            return default(T);
        }

        private void WaitToCompleteAnyTask()
        {
            var tasks = _tasks.Where(t => t != null).ToArray();
            Task.WaitAny(tasks);
        }

        private string ExecuteCommand(T command)
        {
            return _jobManager.Run(command).ToString();
        }
    }
}
