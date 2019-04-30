#!/bin/bash  
set -e

DOCKERHUB_USERNAME=peterkneale
DOCKERHUB_REPO_TENANTS=micro.services.tenants
DOCKERHUB_REPO_CONTENT=micro.services.content

echo tagging images...
docker tag $DOCKERHUB_REPO_TENANTS $DOCKERHUB_USERNAME/$DOCKERHUB_REPO_TENANTS:latest
docker tag $DOCKERHUB_REPO_CONTENT $DOCKERHUB_USERNAME/$DOCKERHUB_REPO_CONTENT:latest

echo login to dockerhub...
docker login -u="$DOCKERHUB_USERNAME"

echo pushing images...
docker push $DOCKERHUB_USERNAME/$DOCKERHUB_REPO_TENANTS
docker push $DOCKERHUB_USERNAME/$DOCKERHUB_REPO_CONTENT