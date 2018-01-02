# Backend for tinygu 2.0

[![Build status](https://ci.appveyor.com/api/projects/status/v49wr1smusto73j4?svg=true)](https://ci.appveyor.com/project/Larsg7/tinygu-backend) [![Build status](https://ci.appveyor.com/api/projects/status/v49wr1smusto73j4/branch/master?svg=true)](https://ci.appveyor.com/project/Larsg7/tinygu-backend/branch/master)

## Installation

* .Net Core 2.0 SDK
* clone repository
* copy `appsettings.default.json` to `appsettings.json` and enter your connection string
* run `dotnet(.exe) restore` inside folder `Tinygubackend` to install all dependencies
* run `dotnet(.exe) run` to start the application on port 5000
* see https://andrewlock.net/how-to-set-the-hosting-environment-in-asp-net-core/ to set env variables on windows
* on linux run `env ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5000 dotnet watch run` to enable development environment

## Swagger

API documentation is available under `/swagger`.
