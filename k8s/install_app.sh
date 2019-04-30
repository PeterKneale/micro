#!/bin/bash
set -e

#kubectl delete namespace micro
#kubectl create namespace micro
kubectl apply -f inf.yml --namespace micro
kubectl apply -f app.yml --namespace micro