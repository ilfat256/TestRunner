using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries.Log
{
    public sealed class ConcurrentLogAccumulator : ILogger
    {
        private static ConcurrentLogAccumulator instance = null;
        private static readonly object _syncObjectStatic = new object();


        private List<string> _logs = new List<string>();
        private object _syncObject = new object();

        private ConcurrentLogAccumulator()
        {
        }

        public static ConcurrentLogAccumulator GetLogger
        {
            get
            {
                lock (_syncObjectStatic)
                {
                    if (instance == null)
                    {
                        instance = new ConcurrentLogAccumulator();
                    }
                    return instance;
                }
            }
        }

        public void Log(string message)
        {
            lock (_syncObject)
            {
                _logs.Add($"{DateTime.Now} message : {message}");
            }
        }

        public List<String> Release()
        {
            lock (_syncObject)
            {
                var logs = _logs;
                _logs = new List<string>();
                return logs;
            }
        }

        public void Log(string message, Exception exception)
        {
            lock (_syncObject)
            {
                _logs.Add($"{DateTime.Now} message : {message} exception : {exception}");
            }
        }
    }
}
