#FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
#WORKDIR /app
#COPY . .
#RUN dotnet restore
#RUN dotnet publish -o /app/published-app

#FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
#WORKDIR /app
#COPY --from=build /app/published-app /app
#ENTRYPOINT [ "dotnet", "MyHomeAutomation.WebApi.dll" ]

# Build stage
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine as build
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY . .
RUN dotnet publish -c Release -o /app/published-app --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine as runtime
WORKDIR /app
COPY --from=build /app/published-app .

ENTRYPOINT ["dotnet", "MyHomeAutomation.WebApi.dll"]
