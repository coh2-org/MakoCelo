using System;
using System.Windows.Forms;
using MakoCelo.IoC;
using NLog;
using NLog.Config;
using NLog.Targets;
using SimpleInjector;
using SimpleInjector.Diagnostics;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            BuildLogConfiguration();
            var container = ContainerInitializer.BuildContainer();

            Application.Run(container.GetInstance<frmMain>());
        }

        private static void BuildLogConfiguration()
        {
            var config = new LoggingConfiguration();

            var fileTarget = new FileTarget();
            fileTarget.FileName = "DiagnosticLog.txt";
            fileTarget.Layout = "${time} ${logger}[${threadid}][${level}] ${event-properties:item=TypeInfo}.${event-properties:item=MethodInfo} - ${message}";
            fileTarget.MaxArchiveFiles = 5;
            fileTarget.ArchiveOldFileOnStartup = true;

            config.AddTarget("file", fileTarget);
            config.AddRuleForAllLevels(fileTarget);

            LogManager.Configuration = config;
            
        }

        
    }
}
