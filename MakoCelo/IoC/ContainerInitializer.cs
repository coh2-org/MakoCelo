using MakoCelo.Scanner;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MakoCelo.IoC
{
    public class ContainerInitializer
    {
        public static Container BuildContainer()
        {
            // Create the container as usual.
            var container = new Container();

            // Register your types, for instance:
            container.RegisterSingleton<IFormOpener, FormOpener>();
            container.RegisterSingleton<Settings>();
            container.RegisterSingleton<MatchScanner>();
            container.RegisterSingleton<IRelicApiClient, RelicApiClient>();
            container.RegisterSingleton<ILogFileParser, LogFileParser>();
            container.RegisterSingleton<IRelicApiResponseMapper, RelicApiResponseMapper>();

            AutoRegisterWindowsForms(container);

            container.Verify();

            return container;
        }

        private static void AutoRegisterWindowsForms(Container container)
        {
            var types = container.GetTypesToRegister<frmMain>(typeof(Program).Assembly); //for now only main form is registered

            foreach (var type in types)
            {
                var registration =
                    Lifestyle.Transient.CreateRegistration(type, container);

                registration.SuppressDiagnosticWarning(
                    DiagnosticType.DisposableTransientComponent,
                    "Forms should be disposed by app code; not by the container.");

                container.AddRegistration(type, registration);
            }
        }
    }
}
