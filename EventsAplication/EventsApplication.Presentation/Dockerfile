#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EventsApplication.Presentation/EventsApplication.Presentation.csproj", "EventsApplication.Presentation/"]
COPY ["EventsApplication.Application/EventsApplication.Application.csproj", "EventsApplication.Application/"]
COPY ["EventsApplication.Domain/EventsApplication.Domain.csproj", "EventsApplication.Domain/"]
COPY ["EventsApplication.Infrastructure/EventsApplication.Infrastructure.csproj", "EventsApplication.Infrastructure/"]
COPY ["EventsApplication.Persistence/EventsApplication.Persistence.csproj", "EventsApplication.Persistence/"]
RUN dotnet restore "./EventsApplication.Presentation/EventsApplication.Presentation.csproj"
COPY . .
WORKDIR "/src/EventsApplication.Presentation"
RUN dotnet build "./EventsApplication.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EventsApplication.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventsApplication.Presentation.dll"]