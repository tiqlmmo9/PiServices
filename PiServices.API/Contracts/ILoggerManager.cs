using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts
{
    public interface ILoggerManager
    {
        void LogInfo(string message);
        void LogWarm(string message);
        void LogDebug(string message);
        void LogError(string message);
    }
}
