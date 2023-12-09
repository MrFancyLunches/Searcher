using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileSearcher
{ /// <summary>
  ///     A utility logging class
  /// </summary>
    public static class Logger
    {
        /// <summary>
        ///     The logging delegate
        /// </summary>
        private static Action<string, LogSeverity> _logAction;

        static Logger()
        {
            SetDefaultLogBehavior();
        }

        public static void Log(LogSeverity severity, string msg)
        {
            _logAction(msg, severity);
        }
        public static void Log(LogSeverity severity, params string[] msgs)
        {
            string msg = string.Join(" ", msgs);
            _logAction(msg, severity);
        }

        public static void LogInfo(params string[] msgs)
        {
            Log(LogSeverity.Info, msgs);
        }
        public static void LogInfo(string msg)
        {
            Log(LogSeverity.Info, msg);
        }

        public static void LogDebug(params string[] msgs)
        {
            Log(LogSeverity.Debug, msgs);
        }
        public static void LogDebug(string msg)
        {
            Log(LogSeverity.Debug, msg);
        }

        public static void LogWarn(params string[] msgs)
        {
            Log(LogSeverity.Warn, msgs);
        }
        public static void LogWarn(string msg)
        {
            Log(LogSeverity.Warn, msg);
        }

        public static void LogError(params string[] msgs)
        {
            Log(LogSeverity.Error, msgs);
        }
        public static void LogError(string msg)
        {
            Log(LogSeverity.Error, msg);
        }

        /// <summary>
        ///     Reset the bound logging logic to use the console
        /// </summary>
        public static void SetDefaultLogBehavior()
        {
            _logAction = ((msg, sev) =>
            {
                Console.WriteLine(string.Format("Search: {0}: {1}", sev.ToString(), msg));
            });
        }

        /// <summary>
        ///     Override initial logging methodology
        /// </summary>
        /// <param name="logDelegate">The delegate to be used in logging</param>
        public static void BindLogDelegate(Action<string, LogSeverity> logDelegate)
        {
            _logAction = logDelegate;
        }

    }
}
