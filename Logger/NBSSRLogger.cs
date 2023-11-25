using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Logger
{
    public class NBSSRLogger
    {
        public string LoggerKey;

        public NBSSRLogger(string loggerKey) 
        {
            LoggerKey = loggerKey;
        }

        public void LogInfo(object obj)
        {
            NBSSRLogWriter.Log("I", BuildMessage(obj));
        }

        public void LogWarning(object obj)
        {
            NBSSRLogWriter.Log("W", BuildMessage(obj));
        }

        public void LogError(object obj)
        {
            NBSSRLogWriter.Log("E", BuildMessage(obj));
        }

        private string BuildMessage(object obj)
        {
            return $"[{LoggerKey}] {obj}";
        }
    }
}
