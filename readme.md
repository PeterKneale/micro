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
    - Service A [http://localhost:5001](http://localhost:5001)
    - Service B [http://localhost:5002](http://localhost:5002)
    - Logs [http://localhost:5003](http://localhost:5003)
        - Try searching for `AppName = 'Micro.ServiceA'`
        - Try searching for `select count(1) from stream group by AppName`
    - SqlServer -> `Server=localhost,51433;Database=master;User Id=sa;Password=Pass@word;`

## Run the infrastructure in docker while running the services in visual studio
```sh
	docker-compose -f docker-compose-infra.yml up --build --force-recreate
```

## Run the tests (unit, integration and acceptance)
```sh
	./tests.sh
```

