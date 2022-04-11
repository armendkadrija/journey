# Journey

This service/API subscribes to telemetry data from the MQQT server `mqtt.hsl.fi` which publishes positions of vehicles in the HSL area, and stores those telemetry data for later consumption.

I used those telemetry data by utilizing MQQTNet which is a client library, then created a background worker creates connection to `mqtt.hsl.fi` and subscribes to topic `/hfp/v2/journey/ongoing/vp/bus/#`. This topic produces events related to Bus positions and provides information about the location, direction, next stop etc.

Due to the high amount of events produced by this topic, persisting events directly to the database was very costly and affected performance, therefore I implemented a `Queue` mechanism which it`s main purpose is to queue events (messages) first in queue then after the queue has reached its configured limit the queue is persisted in database.

## Technologies

- .NET 6
- Entity Framework 6
- Docker
- Patterns
  - CQRS
  - MediatR
- Database
  - PostgreSQL & PostGIS

### Docker Configuration

In order to get Docker working, you will need to add a temporary SSL cert and mount a volume to hold that cert.

##### For Windows:

The following will need to be executed from your terminal to create a cert

```sh
dotnet dev-certs https -ep %USERPROFILE%\.aspnet\https\aspnetapp.pfx -p CertPassword
dotnet dev-certs https --trust
```

##### FOR MacOS:

```sh
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p CertPassword`
dotnet dev-certs https --trust
```

##### FOR Linux:

```sh
dotnet dev-certs https -ep ${HOME}/.aspnet/https/aspnetapp.pfx -p CertPassword
```

In order to build and run the docker containers, execute `docker-compose up -d --build` from the root of the solution.

### Features & Testing

After the docker containers are up & running, imediately background worker is accepting events and storing them.

To test & view api documentation open `Swagger` link
[https://localhost:6001/api/index.html?url=/api/specification.json](https://localhost:6001/api/index.html?url=/api/specification.json#/VehiclePosition/VehiclePosition_GetNearestBuses)

### Improvements

- Besides queue size limit, add a time constraint limit which would compensate for cases when queue takes time to fill.
- Add postgress replicas designated only for read.
