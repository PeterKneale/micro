#!/bin/bash
set -e

kubectl apply -f app.yml    --namespace micro-dev
kubectl apply -f app.yml    --namespace micro-qa
kubectl apply -f app.yml    --namespace micro-prod