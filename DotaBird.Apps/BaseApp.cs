using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NLog;
using NLog.Targets;
using NLog.Config;

namespace DotaBird.Apps
{
    /// <summary>
    /// Abstract base class for all applications.  Sets up logging, etc...
    /// </summary>
    public abstract class BaseApp
    {
        private const String LogLayout = @"${longdate} [${level:uppercase=true}] ${message}";

        protected BaseApp()
        {
            SetupLogging();
        }

        protected abstract String AppName { get; }

        protected void SetupLogging()
        {
            LoggingConfiguration config = new LoggingConfiguration();

            Target target = SetupConsoleTarget();
            config.AddTarget("target", target);

            // Send messages from all loggers, to the target log, with Log level debug and higher.
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));

            LogManager.Configuration = config;
        }

        private Target SetupConsoleTarget()
        {
            return new ColoredConsoleTarget
            {
                Layout = LogLayout
            };
        }

        private Target SetupEventLogTarget()
        {
            return new EventLogTarget
            {
                Source = AppName,
                Log = AppName,
                Layout = LogLayout
            };
        }

        private Target SetupFileTarget()
        {
            return new FileTarget {
                FileName = "${basedir}/file.txt",
                Layout = LogLayout
            };
        }
    }
}
