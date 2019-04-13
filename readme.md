# micro
- A demo of two micro services with necessary support infrastructure running within docker containers and launched by docker compose

# Running

The service can be run a few different ways
1. Run everything in docker (or)
2. Run just the infrastructure in docker
    1. Run the services in visual studio (or)
    2. Run the services on the console
## 1. Run everything in docker

- Start up the cluster
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up
```

- Shut down the cluster
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

- Rebuild and start up the cluster
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
```

## 2. Run just the infrastructure in docker

- Start up the infrastructure
```
docker-compose -f docker-compose-infra.yml -f docker-compose-infra.override.yml up --build
```
*Run the service either in visual studio or in a console*

### 2.1 Run the services in visual studio
    - Select `Set Startup Projects` and select both `Micro.ServiceA` and `Micro.ServiceB`
    - Press F5

### 2.2 Run services on console
    - Run on one console `dotnet run --project src/ServiceA/Micro.ServiceA/Micro.ServiceA.csproj`
    - Run on another console `dotnet run --project src/ServiceB/Micro.ServiceB/Micro.ServiceB.csproj`


- Shut down the infrastructure
```
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

# Exploring the endpoints

- The endpoints exposed
    - Service A [http://localhost:5001](http://localhost:5001)
    - Service B [http://localhost:5002](http://localhost:5002)
    - Logs [http://localhost:5003](http://localhost:5003)
        - Try searching for `AppName = 'Micro.ServiceA'`
        - Try searching for `select count(1) from stream group by AppName`
    - SqlServer -> `Server=localhost,51433;Database=master;User Id=sa;Password=Pass@word;`

