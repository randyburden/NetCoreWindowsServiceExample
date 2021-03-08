using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting.WindowsServices;
using Microsoft.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.ServiceProcess;
using ExampleWindowsService.Services;
using NLog;

namespace ExampleWindowsService
{
    /*
    Windows Service Install, Start, Stop, Query, Delete Commands (Run Command Prompt as Admin):
    ============================================================================================
    sc create "Example Service" binPath= "C:\Users\Randy\source\repos\ExampleWindowsService\ExampleWindowsService\bin\Release\netcoreapp3.1\publish\ExampleWindowsService.exe" start= auto
    sc start "Example Service"
    sc stop "Example Service"
    sc query "Example Service"
    sc delete "Example Service"
    */
    public class Program
    {
        private const string ApplicationName = "Example Service";

        public static void Main(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                })
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                    logging.AddEventLog(settings =>
                    {
                        settings.LogName = "Application";
                        settings.SourceName = ApplicationName;
                        settings.MachineName = Environment.MachineName;
                    });
                })
                .UseNLog()
                .UseStartup<Startup>()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<TimedHostedService>();
                });

            if (WindowsServiceHelpers.IsWindowsService())
            {
                // ReSharper disable once PossibleNullReferenceException
                var pathToExe = Process.GetCurrentProcess().MainModule.FileName;
                var pathToContentRoot = Path.GetDirectoryName(pathToExe);
                builder.UseContentRoot(pathToContentRoot);
                // ReSharper disable once AssignNullToNotNullAttribute
                LogManager.LogFactory.SetCandidateConfigFilePaths(new List<string> { $"{Path.Combine(pathToContentRoot, "nlog.config")}" });
                var host = builder.Build();
                var webHostService = new ExampleWebHostService(host, ApplicationName);
                ServiceBase.Run(webHostService);
            }
            else
            {
                NLogBuilder.ConfigureNLog("nlog.config");
                LogManager.GetCurrentClassLogger().Info("Application starting");
                builder.Build().Run();
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }
    }
}
