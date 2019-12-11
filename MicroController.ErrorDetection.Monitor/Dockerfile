FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MicroController.ErrorDetection.Monitor/MicroController.ErrorDetection.Monitor.csproj", "MicroController.ErrorDetection.Monitor/"]
RUN dotnet restore "MicroController.ErrorDetection.Monitor/MicroController.ErrorDetection.Monitor.csproj"
COPY . .
WORKDIR "/src/MicroController.ErrorDetection.Monitor"
RUN dotnet build "MicroController.ErrorDetection.Monitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroController.ErrorDetection.Monitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroController.ErrorDetection.Monitor.dll"]