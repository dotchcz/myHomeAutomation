FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MyHomeAutomation.WebApi/MyHomeAutomation.WebApi.csproj", "MyHomeAutomation.WebApi/"]
RUN dotnet restore "MyHomeAutomation.WebApi/MyHomeAutomation.WebApi.csproj"
COPY . .
WORKDIR "/src/MyHomeAutomation.WebApi"
RUN dotnet build "MyHomeAutomation.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MyHomeAutomation.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MyHomeAutomation.WebApi.dll"]
