FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MicroController.ErrorDetection.Executor/MicroController.ErrorDetection.Executor.csproj", "MicroController.ErrorDetection.Executor/"]
RUN dotnet restore "MicroController.ErrorDetection.Executor/MicroController.ErrorDetection.Executor.csproj"
COPY . .
WORKDIR "/src/MicroController.ErrorDetection.Executor"
RUN dotnet build "MicroController.ErrorDetection.Executor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroController.ErrorDetection.Executor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroController.ErrorDetection.Executor.dll"]