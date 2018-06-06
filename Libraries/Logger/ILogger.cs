using System;
using System.Collections.Generic;

namespace Libraries.Log
{
    public interface ILogger
    {
        void Log(string message);
        void Log(string message, Exception exception);
        List<string> Release();
    }
}
