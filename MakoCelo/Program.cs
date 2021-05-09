using System;
using System.Windows.Forms;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace MakoCelo
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            fileTarget.FileName = "DiagnosticLog.txt";
            fileTarget.Layout = "${time} ${logger}[${threadid}][${level}] ${event-properties:item=TypeInfo}.${event-properties:item=MethodInfo} - ${message}";
            fileTarget.MaxArchiveFiles = 2;
            fileTarget.ArchiveOldFileOnStartup = true;

            config.AddTarget("file", fileTarget);
            config.AddRuleForAllLevels(fileTarget);

            LogManager.Configuration = config;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new frmMain());
        }
    }
}
