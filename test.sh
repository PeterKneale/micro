#!/bin/bash  
set -e

echo "Building"
docker-compose -f docker-compose.yml build

echo "Running unit tests for tenants"
docker-compose -f docker-compose-tests-unit-tenants.yml up --build --force-recreate

echo "Running unit tests for content"
docker-compose -f docker-compose-tests-unit-content.yml up --build --force-recreate

echo "Running integration tests for tenants"
docker-compose -f docker-compose-tests-integration-tenants.yml up --build --force-recreate --exit-code-from micro.services.tenants.integrationtests --abort-on-container-exit

echo "Running integration tests for content"
docker-compose -f docker-compose-tests-integration-content.yml up --build --force-recreate --exit-code-from micro.services.content.integrationtests --abort-on-container-exit

echo "Running acceptance tests"
docker-compose -f docker-compose-tests-acceptance.yml up --build --force-recreate --exit-code-from micro.services.acceptancetests --abort-on-container-exit

