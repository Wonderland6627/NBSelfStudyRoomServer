using NBSSR.Network;
using NBSSRServer.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NBSSRServer.Logger
{
    public static class NBSSRLogWriter
    {
        private static StreamWriter LogWriter = null;
        private static DateTime Today = DateTime.Today;

        private static readonly string LoDateTimeFormat = "yyyy-MM-dd HH:mm:ss.fff";
        private static readonly string LogFileDateTimeFormat = "yyyy-MM-dd";
        private static readonly string LogFileDirPath = Path.Combine(PathUtils.GetApplicationDirectory(), "Logs");

        public static void StartRecord()
        {

        }

        public static void Log(string level, object obj)
        {
            string log = BuildMessage(level, obj);
            Console.WriteLine(log);
            Write(log);
        }

        private static string BuildMessage(string level, object obj)
        {
            return $"[{level}] [{DateTime.Now.ToString(LoDateTimeFormat)}] {obj}";
        }

        private static void Write(string line)
        {
            if (!DateTime.Today.Equals(Today)) //新的一天新建一个日志文件
            {
                LogWriter = null;
                Today = DateTime.Today;
            }

            if (LogWriter == null)
            {
                if (!Directory.Exists(LogFileDirPath))
                {
                    Directory.CreateDirectory(LogFileDirPath);
                }

                LogWriter = new StreamWriter($"{LogFileDirPath}/{Today.ToString(LogFileDateTimeFormat)}.log", true, Encoding.UTF8);
            }
            LogWriter.WriteLine(line);
            LogWriter.Flush();
        }

        public static void StopRecord()
        {
            if (LogWriter != null)
            {
                LogWriter.Close();
                LogWriter.Dispose();
            }
        }
    }
}
