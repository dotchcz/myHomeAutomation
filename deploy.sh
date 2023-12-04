#!/bin/bash

git fetch
git pull
docker-compose -f ./docker-compose.override.yaml -f ./docker-compose.ArmV7.yaml stop
docker-compose -f ./docker-compose.override.yaml -f ./docker-compose.ArmV7.yaml pull
docker-compose -f ./docker-compose.override.yaml -f ./docker-compose.ArmV7.yaml up -d