# NetCoreWindowsServiceExample
.NET Core 3.1 Windows Service Example

## Overview
- Runs a web app hosted in a Windows Service
  - Can run as console app or Windows Service (auto-detects)
- Uses an example Hosted Service (background task)
  - Writes to the log every 5 seconds
- Uses Razor Pages to host web pages
  - Example index page hosted at: https://localhost:5001/
- Uses Web API controller
  - Example API endpoint hosted at: https://localhost:5001/api/example/GetCurrentTime
- Logs to files using NLog
  - Adjust the LogLevel in the appsettings.json file
  - Adjust the destination of the logs in the nlog.config file
- Logs to EventLog
  - Adjust the LogLevel in the appsettings.json file
- Publishes to a single file (self-contained)
  - Right-click project and select Publish

## Windows Service Install, Start, Stop, Query, Delete Commands

Be sure to run the command prompt as Admin.
- sc create "Example Service" binPath= "C:\Users\Randy\source\repos\ExampleWindowsService\ExampleWindowsService\bin\Release\netcoreapp3.1\publish\ExampleWindowsService.exe" start= auto
- sc start "Example Service"
- sc stop "Example Service"
- sc query "Example Service"
- sc delete "Example Service"