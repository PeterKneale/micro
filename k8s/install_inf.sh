#!/bin/bash
set -e

kubectl delete pods             --namespace micro-inf --all
kubectl delete deployments      --namespace micro-inf --all
kubectl delete services         --namespace micro-inf --all
kubectl delete ingress          --namespace micro-inf --all
kubectl apply -f inf.yml        --namespace micro-inf
