set -x
echo "Building"
docker-compose -f docker-compose.yml build --force-recreate

echo "Running unit tests for service a"
docker-compose -f docker-compose-unit-a.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.unit.a

echo "Running unit tests for service b"
docker-compose -f docker-compose-unit-b.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.unit.b

echo "Running integration tests for service a"
docker-compose -f docker-compose-integration-a.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.integration.a

echo "Running integration tests for service b"
docker-compose -f docker-compose-integration-b.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.integration.b

echo "Running acceptance tests"
docker-compose -f docker-compose-acceptance.yml up --build --force-recreate \
    --abort-on-container-exit --exit-code-from micro.acceptance
