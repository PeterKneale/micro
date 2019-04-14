set -x
echo "Building"
docker-compose -f docker-compose.yml -f docker-compose.override.yml build

echo "Running unit tests for service a"
docker-compose -f docker-compose-unit-a.yml -f docker-compose-unit-a.override.yml up --build \
    --abort-on-container-exit --exit-code-from micro.unit.a

echo "Running unit tests for service b"
docker-compose -f docker-compose-unit-b.yml -f docker-compose-unit-b.override.yml up --build \
    --abort-on-container-exit --exit-code-from micro.unit.b

echo "Running integration tests for service a"
docker-compose -f docker-compose-integration-a.yml -f docker-compose-integration-a.override.yml up --build \
    --abort-on-container-exit --exit-code-from micro.integration.a

echo "Running integration tests for service b"
docker-compose -f docker-compose-integration-b.yml -f docker-compose-integration-b.override.yml up --build \
    --abort-on-container-exit --exit-code-from micro.integration.b

echo "Running acceptance tests"
docker-compose -f docker-compose-acceptance.yml -f docker-compose-acceptance.override.yml up --build \
    --abort-on-container-exit --exit-code-from micro.acceptance