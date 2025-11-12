FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["student-task-tracker-metrics/student-task-tracker-metrics.csproj", "student-task-tracker-metrics/"]
RUN dotnet restore "./student-task-tracker-metrics/student-task-tracker-metrics.csproj"
COPY . .
WORKDIR "/src/student-task-tracker-metrics"
RUN dotnet build "./student-task-tracker-metrics.csproj" -c $BUILD_CONFIGURATION -o /app/build
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./student-task-tracker-metrics.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "student-task-tracker-metrics.dll"]