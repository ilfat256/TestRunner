using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    class MultiThreadRunnerException : Exception
    {
        public MultiThreadRunnerException(string message) : base(message)
        {
        }

        public MultiThreadRunnerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
