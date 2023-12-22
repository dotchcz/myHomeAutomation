#!/bin/bash

git fetch
git pull

docker-compose stop myhomeautomation.webapp myhomeautomation.webapi
docker-compose rm -f myhomeautomation.webapp myhomeautomation.webapi
docker-compose pull myhomeautomation.webapp myhomeautomation.webapi
docker-compose -f ./docker-compose.override.yaml -f ./docker-compose.ArmV7.yaml up -d