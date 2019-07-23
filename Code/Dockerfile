FROM microsoft/dotnet:2.2-aspnetcore-runtime-alpine3.8 AS base
RUN apk --update add openjdk8-jre
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk-alpine3.8 AS build
WORKDIR /src

COPY ["./Service/ApplicationService.EventBroker/ApplicationService.EventConsumer/*.*", "ApplicationService.EventBroker/ApplicationService.EventConsumer/"]
COPY ["./Service/ApplicationService.EventBroker/ClientLibrary/*.*", "ApplicationService.EventBroker/ClientLibrary/"]
COPY ["./Service/ApplicationService.EventBroker/ClientLibrary/**/*.*", "ApplicationService.EventBroker/ClientLibrary/"]
COPY ["./Service/ApplicationService.EventBroker/EventConsumer/*.*", "ApplicationService.EventBroker/EventConsumer/"]

COPY ["./Service/ApplicationService.DataAccess/*.*", "ApplicationService.DataAccess/"]
COPY ["./Service/ApplicationService.EventBroker/ApplicationService.EventProducer/*.*", "ApplicationService.EventBroker/ApplicationService.EventProducer/"]
COPY ["./Service/ApplicationService.API/*.*", "ApplicationService.API/"]

WORKDIR "/src/ApplicationService.EventBroker/ApplicationService.EventConsumer"
RUN dotnet publish "ApplicationService.EventConsumer.csproj" -c Release -o /app  

WORKDIR "/src/ApplicationService.EventBroker/EventConsumer"
RUN dotnet publish "EventConsumer.csproj" -c Release -o /app

WORKDIR "/src/ApplicationService.API"
RUN dotnet publish "ApplicationService.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
#ENTRYPOINT ["dotnet", "ApplicationService.API.dll"]
ENTRYPOINT ["dotnet", "ApplicationService.EventConsumer.dll", "--properties", "kcl.properties", "--execute"]

