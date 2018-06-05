using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Libraries
{
    class ProcessJobManagerException : Exception
    {
        public ProcessJobManagerException(string message) : base(message)
        {
        }

        public ProcessJobManagerException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
