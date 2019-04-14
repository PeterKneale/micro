# micro
- A demo of two micro services with necessary support infrastructure running within docker containers and launched by docker compose

[![Build status](https://ci.appveyor.com/api/projects/status/e29quxiplixm9v7x?svg=true)](https://ci.appveyor.com/project/PeterKneale/micro)

# Demo's

1. Run the cluster entirely in docker
2. Run the infrastructure in docker while running the services in visual studio
3. Run the tests (unit, integration and acceptance)

## 1. Run everything in docker

```
# Start up the cluster
docker-compose -f docker-compose.yml -f docker-compose.override.yml up

# Shut down the cluster
docker-compose -f docker-compose.yml -f docker-compose.override.yml down

# Rebuild and start up the cluster
docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build
```

## 2. Run just the infrastructure in docker

```
# Start up the infrastructure
docker-compose -f docker-compose-infra.yml -f docker-compose-infra.override.yml up --build

# Shut down the infrastructure
docker-compose -f docker-compose.yml -f docker-compose.override.yml down
```

## Running the tests
```
# Running unit tests for service a
docker-compose -f docker-compose-unit-a.yml -f docker-compose-unit-a.override.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.unit.a

# Running unit tests for service b
docker-compose -f docker-compose-unit-b.yml -f docker-compose-unit-b.override.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.unit.b

# Running integration tests for service a
docker-compose -f docker-compose-integration-a.yml -f docker-compose-integration-a.override.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.integration.a

# Running integration tests for service b
docker-compose -f docker-compose-integration-b.yml -f docker-compose-integration-b.override.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.integration.b

# Running acceptance tests
docker-compose -f docker-compose-acceptance.yml -f docker-compose-acceptance.override.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.acceptance
```


# Exploring the endpoints

- The endpoints exposed
    - Service A [http://localhost:5001](http://localhost:5001)
    - Service B [http://localhost:5002](http://localhost:5002)
    - Logs [http://localhost:5003](http://localhost:5003)
        - Try searching for `AppName = 'Micro.ServiceA'`
        - Try searching for `select count(1) from stream group by AppName`
    - SqlServer -> `Server=localhost,51433;Database=master;User Id=sa;Password=Pass@word;`

