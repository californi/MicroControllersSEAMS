FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MetaController.Monitor/MetaController.Monitor.csproj", "MetaController.Monitor/"]
RUN dotnet restore "MetaController.Monitor/MetaController.Monitor.csproj"
COPY . .
WORKDIR "/src/MetaController.Monitor"
RUN dotnet build "MetaController.Monitor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MetaController.Monitor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetaController.Monitor.dll"]