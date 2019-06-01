#!/bin/bash  
set -e

DOCKERHUB_REPO_GATEWAY=micro.services.gateway
DOCKERHUB_REPO_TENANTS=micro.services.tenants
DOCKERHUB_REPO_CONTENT=micro.services.content

echo building images...
docker build -f src/Micro.Services.Gateway/Dockerfile -t $DOCKERHUB_REPO_GATEWAY .
docker build -f src/Micro.Services.Tenants/Dockerfile -t $DOCKERHUB_REPO_TENANTS .
docker build -f src/Micro.Services.Content/Dockerfile -t $DOCKERHUB_REPO_CONTENT .
