using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using NLog;
using NLog.Web;
using System;
using System.Diagnostics;

namespace ExampleWindowsService
{
    public sealed class ExampleWebHostService : WebHostService
    {
        private readonly IWebHost _host;
        private Logger _logger;

        public ExampleWebHostService(IWebHost host, string applicationName) : base(host)
        {
            _host = host;
            ServiceName = applicationName;
            EventLog.Source = "Application";
            EventLog.Source = applicationName;
            EventLog.MachineName = Environment.MachineName;
        }

        protected override void OnStarting(string[] args)
        {
            NLogBuilder.ConfigureNLog("nlog.config");
            _logger = LogManager.GetCurrentClassLogger();
            _logger?.Info("Service starting");
        }

        protected override void OnStarted()
        {
            _logger?.Info("Service started");
        }

        protected override void OnStopping()
        {
            _logger?.Info("Service stopped");
            // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
            LogManager.Shutdown();
        }
    }
}