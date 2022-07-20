git fetch
git pull

docker build -f ./MyHomeAutomation.WebApi/Dockerfile.ArmV7 -tmyhomeautomationwebapi:latest .
docker build -t ./MyHomeAutomation.WebApp/Dockerfile -tmyhomeautomationwebapp:latest .


docker-compose 