FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MetaController.Executor/MetaController.Executor.csproj", "MetaController.Executor/"]
RUN dotnet restore "MetaController.Executor/MetaController.Executor.csproj"
COPY . .
WORKDIR "/src/MetaController.Executor"
RUN dotnet build "MetaController.Executor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MetaController.Executor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetaController.Executor.dll"]