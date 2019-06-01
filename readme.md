# micro
- A demo of two micro services with necessary support infrastructure running within docker containers and launched by docker compose

[![Build status](https://ci.appveyor.com/api/projects/status/e29quxiplixm9v7x?svg=true)](https://ci.appveyor.com/project/PeterKneale/micro)
[![Dependabot Status](https://api.dependabot.com/badges/status?host=github&repo=PeterKneale/micro)](https://dependabot.com)

# Execution

## Run the cluster entirely in docker

- Bring up the cluster using local code
    ```sh
    docker-compose -f docker-compose.yml -f docker-compose.override.yml up --build --force-recreate
    ```
- Bring up the cluster using dockerhub
    ```sh
    docker-compose -f docker-compose-dockerhub.yml -f docker-compose.override.yml up --force-recreate
    ```

- Exploring the endpoints
    - Gateway [http://localhost:5000](http://localhost:5000)
    - Tenants [http://localhost:5001](http://localhost:5001)
    - Content [http://localhost:5002](http://localhost:5002)
    - Logs [http://localhost:5003](http://localhost:5003)
        - Try searching for `AppName = 'Micro.Service.Tenants'`
        - Try searching for `select count(1) from stream group by AppName`
    - SqlServer -> `Server=localhost,51433;Database=Tenants;User Id=sa;Password=Password123;`
    - SqlServer -> `Server=localhost,51433;Database=Content;User Id=sa;Password=Password123;`

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
- https://cloud.docker.com/repository/docker/peterkneale/micro.services.gateway
- https://cloud.docker.com/repository/docker/peterkneale/micro.services.tenants
- https://cloud.docker.com/repository/docker/peterkneale/micro.services.content

## Kubernetes

### Once off configuration
- setup environment
    ```sh
    export KUBECONFIG=~/.kube/micro-kubeconfig.yaml
    ```
- get info
    ```sh
    kubectl cluster-info
    kubectl get nodes
    ```

### Install dashboard
- install
    ```sh
    kubectl create -f https://raw.githubusercontent.com/kubernetes/dashboard/v1.10.1/src/deploy/alternative/kubernetes-dashboard.yaml
    kubectl apply -f k8s/setup/dashboard-admin.yml
    ```

- run [proxy](http://localhost:8001/api/v1/namespaces/kube-system/services/kubernetes-dashboard/proxy)
    ```sh
    kubectl proxy
    ```

### Install ingress
- install
    ```sh
    kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/mandatory.yaml
    kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/master/deploy/provider/cloud-generic.yaml
    ```

- get it's ip
    ```sh
    kubectl get svc --namespace=ingress-nginx
    ```

### Install app
- install the components
    ```sh
    ./install-ns.sh
    ./install-inf.sh
    ./install-app.sh
    ```

- test connectivity
    ```sh
    kubectl port-forward $(kubectl get pod --selector="component=gateway-pod" --namespace="micro-dev" --output jsonpath='{.items[0].metadata.name}') --namespace="micro-dev" 8080:80
    kubectl port-forward $(kubectl get pod --selector="component=tenants-pod" --namespace="micro-dev" --output jsonpath='{.items[0].metadata.name}') --namespace="micro-dev" 8080:80
    kubectl port-forward $(kubectl get pod --selector="component=content-pod" --namespace="micro-dev" --output jsonpath='{.items[0].metadata.name}') --namespace="micro-dev" 8080:80
    ```

### Appveyor
- create service account
```sh
    kubectl apply -f k8s/appveyor-service-account.yml
```

- get login token
```sh
    TOKEN=$(kubectl get secret $(kubectl get secret | grep appveyor-token | awk '{print $1}') -o jsonpath='{.data.token}' | base64 --decode)
    echo $TOKEN
```

### Links

- [gateway](http://gateway.mycodeonline.com)
    - app
        - [name](http://gateway.mycodeonline.com/app/name)
        - [version](http://gateway.mycodeonline.com/app/version)
        - [config](http://gateway.mycodeonline.com/app/config)
    - health checks
        - [ui](http://gateway.mycodeonline.com/healthchecks-ui)
        - [alive](http://gateway.mycodeonline.com/health/alive)
        - [ready](http://gateway.mycodeonline.com/health/ready)

- [tenants](http://tenants.mycodeonline.com)
    - app
        - [name](http://tenants.mycodeonline.com/app/name)
        - [version](http://tenants.mycodeonline.com/app/version)
        - [config](http://tenants.mycodeonline.com/app/config)
    - health checks
        - [ui](http://tenants.mycodeonline.com/healthchecks-ui)
        - [alive](http://tenants.mycodeonline.com/health/alive)
        - [ready](http://tenants.mycodeonline.com/health/ready)
    - errors
        - [internal](http://tenants.mycodeonline.com/errors/internal)
        - [notfound](http://tenants.mycodeonline.com/errors/notfound)
        - [notunique](http://tenants.mycodeonline.com/errors/notunique)
		
- [content](http://content.mycodeonline.com)
    - app
        - [name](http://content.mycodeonline.com/app/name)
        - [version](http://content.mycodeonline.com/app/version)
        - [config](http://content.mycodeonline.com/app/config)
    - health checks
        - [ui](http://content.mycodeonline.com/healthchecks-ui)
        - [alive](http://content.mycodeonline.com/health/alive)
        - [ready](http://content.mycodeonline.com/health/ready)
    - errors
        - [internal](http://content.mycodeonline.com/errors/internal)
        - [notfound](http://content.mycodeonline.com/errors/notfound)
        - [notunique](http://content.mycodeonline.com/errors/notunique)

### APIs

- [tenants]
  - /tenants
  - /tenants/{id}/users
  - /users
  - /users/{id}/teams
  - /teams
  - /teams/{id}/roles
  - /roles
  - /roles/{id}/permissions
  
### GraphQL Gateway

- sample query
```json
	{
	  allteams: teams {name}
	  allusers: users {name}
	  team1: team(id:"1") {name users {name}}  
	  team2: team(id:"2") {name users {name}}
	  user1: user(id:"1") {name teams {name}}  
	  user2: user(id:"2") {name teams {name}}
	}
```