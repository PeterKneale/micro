# micro
- A demo of two micro services with necessary support infrastructure running within docker containers and launched by docker compose

[![Build status](https://ci.appveyor.com/api/projects/status/e29quxiplixm9v7x?svg=true)](https://ci.appveyor.com/project/PeterKneale/micro)

# Execution

## Run the cluster entirely in docker

- Bring up the cluster
```sh
	docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build --force-recreate
```

- Exploring the endpoints
    - Tenants [http://localhost:5001](http://localhost:5001)
    - Content [http://localhost:5002](http://localhost:5002)
    - Logs [http://localhost:5003](http://localhost:5003)
        - Try searching for `AppName = 'Micro.Service.Tenants'`
        - Try searching for `select count(1) from stream group by AppName`
    - SqlServer -> `Server=localhost,51433;Database=Tenants;User Id=sa;Password=Password123*;`
    - SqlServer -> `Server=localhost,51433;Database=Content;User Id=sa;Password=Password123*;`

## Run the infrastructure in docker while running the services in visual studio
```sh
	docker-compose -f docker-compose-infra.yml up --build --force-recreate
```

## Run the tests (unit, integration and acceptance)
```sh
	./tests.sh
```

## DockerHub

### Links
- https://ci.appveyor.com/project/PeterKneale/micro
- https://cloud.docker.com/repository/docker/peterkneale/micro.services.tenants
- https://cloud.docker.com/repository/docker/peterkneale/micro.services.content
