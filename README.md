# eShop (DDD + CQRS + Event Sourcing)

> **NOTE:** Current project still in progress of implementation and contains only 
> base concept of architecture. However, the main idea should be clear enough.

Current repo contains the prototype of shop built with DDD, CQRS and Event Sourcing.

## Used technologies
For backend part the following technologies are used:
- ASP.NET Core 5
- Entity Framework Core 5
- Event Store
- Kafka (used only as example)
- Fluent Validation
- MediatR
- NSwag
- Identity Server 4
- Serilog

For front-end part Blazor WebAssembly is used.

All projects except Blazor UI are running in Docker with Docker Compose.
Blazor UI should be ran separetly for providing debugging experience.
As for now, Blazor Web Assembly do not support debugging in containers.

## Projects Overview

### Architecture

Current project implemented using Clean Architecture and inspired by [Jayson Tailors' repo](https://github.com/jasontaylordev/CleanArchitecture).
However, it is not following his approach completely due to some controversial decisions (which are pretty discussable).

Also, projects follows clean CQRS with Event Sourcing - commands are working with aggregates only, queries are working with EF Core (now with SQLite for easier testing).
The updating of read models are happening using `IDomainEvent` dispatching to the MediatR handlers.

For events storing and processing - EventStore is using.

### Settings

Solution is using `Directory.Build.props` as centralized storage for common project settings 
and centralized package versioning via `Directory.Packages.props`.

### eShop.Common

Current project contains base services, useful utils, etc., which can be used solution-wide.

### eShop.Domain

This project contains all domain entities, aggregates and interface `IAggregateStore` which provides the contract for how entities can be loaded and saved.
Also, this project contains `ValueObject` abstraction which can help to encapsulate solid value object. Example of it - `ValueObjects\Money`.

### eShop.Application

Application layer which contains the operational and functional logic. The structure of project follows "by-feature" structure - 
handlers, projections, models, validators are placed in same folder for one feature. 

For commands/queries and handlers MediatR is used with custom pipeline extensions (see `Behaviors` folder). In `Persistence` folder you can find interface for
EF Core context and entities which will be used for read models.

In `Projections` folder you can find wrappers around MeditR commands and handlers which contains logic for transferring `IDomainEvent` 
to specified handler for future processing - updating read database, putting messages to Kafka, etc.

### eShop.Infrastructure

Current project contains concrete implementations for Application services which are infrastructure-related - event store services (Event Store is used), 
EF Core implementation (migrations for SQLite) and other.

### eShop.Api

This is the presenter part which provides REST API for eShop and wires up everything together. For authentication JWT bearer is used with authority at eShop.Auth.
This API also supports serving of Swagger UI and OpenAPI specification via NSwag.

### eShop.Auth

This project contains Identity Server 4 and UI for login - the main idea is to provide centralized login/logout (also know as SSO). 
It is based on standard templates from Identity Server team and uses in-memory storage for clients, scopes and users (just for testing purposes).

### eShop.UI.Blazor

Basic Blazor UI project. Does not have any presentation logic for now, but supports auth via JWT Bearer and SSO via `eShop.Auth` service.

### eShop.Domain.UnitTests

Unit tests project for domain aggregates and entities. Now contains the example of unit test for different actions which can be done with `ProductWarehouse` aggregate.

### Common parts

Each project might have `DependencyInjection` class which encapsulates the setting-up and wiring of dependencies. 
For example, in `eShop.Application\DependencyInjection.cs` you can find extension for registering Application-related dependencies.

## Credits

- Inspiration - [Jayson Tailors' Clean Architecture](https://github.com/jasontaylordev/CleanArchitecture)
- Event Sourcing inspiration - [repo](https://github.com/alexeyzimarev/dotnext) by Alexey Zimarev from his [DotNext Event Sourcing workshop](https://www.youtube.com/watch?v=z20XGmEUzIw&list=PLtWrKx3nUGBfRvrRmicKedShARUrDU3sj&index=5&ab_channel=DotNext)

## License

This project is licensed with the [MIT license](LICENSE).