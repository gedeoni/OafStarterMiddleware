# Oaf Starter Template For Middleware Team

## Description
 This is a dotnet template that aims at reducing the amount of setup and boiler plate work that a developper needs to do in order to get started with any dotnet project in the middleware team. It helps with increasing the productivity of developpers as well as creating coding standards that every team members should abide by.


## Tech Stack
### DotnetCore 5
The main programming language is C#. MediatR is one of the main dependencies that help with executing Commands and Queries

### Couchbase 6.6
The main database
### RabbitMQ
The main EventBus
### Tye
For running the project smoothly

## Architecture
The Project uses the Architecture also known as Onion Architectue. It uses the Command Query Responsibility Segregation (CQRS) Pattern to achieve decoupling. The Repository Pattern is also used to better encapsulate the database access logic.

## Installation Instructions
### Requirements
- [Docker](https://www.docker.com/get-started)
- [Tye](https://github.com/dotnet/tye)
- [Dotnet 5](https://dotnet.microsoft.com/download/dotnet/5.0)
- Access to Oaf Azure Account
- [Azure Credential Provider](https://github.com/microsoft/artifacts-credprovider#azure-artifacts-credential-provider)

### Installation Commands
- git clone https://github.com/one-acre-fund/payments/
- cd OafStarterMiddleware
- dotnet restore --Interactive

### Running tests
- tye run tyeTest.yaml
- dotnet test

### Running the application
- tye run # run the rest of the application
