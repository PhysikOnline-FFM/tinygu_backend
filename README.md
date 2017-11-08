# Backend for tinygu 2.0

## Installation

* .Net Core 2.0 SDK
* clone repository
* copy `appsettings.default.json` to `appsettings.json` and enter your connection string
* run `dotnet(.exe) restore` inside folder `Tinygubackend to install all dependencies
* run `dotnet(.exe) run` to start the application on port 5000
* on linux run `env ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5000 dotnet watch run` to enable development environment

## Swagger

API documentation is available under `/swagger`.
