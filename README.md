# Backend for tinygu 2.0

[![CircleCI](https://circleci.com/gh/PhysikOnline/tinygu_backend/tree/develop.svg?style=svg)](https://circleci.com/gh/PhysikOnline/tinygu_backend/tree/develop)

## Installation

* .Net Core 2.0 SDK
* clone repository
* run `dotnet(.exe) restore` inside folder `Tinygubackend` to install all dependencies
* run `dotnet(.exe) run` to start the application on port 5000
* TODO Connection String
* see https://andrewlock.net/how-to-set-the-hosting-environment-in-asp-net-core/ to set env variables on windows
* on linux run `env ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=http://localhost:5000 dotnet watch run` to enable development environment

## Swagger

API documentation is available under `/swagger`.
