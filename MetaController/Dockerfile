FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["MetaController/MetaController.csproj", "MetaController/"]
RUN dotnet restore "MetaController/MetaController.csproj"
COPY . .
WORKDIR "/src/MetaController"
RUN dotnet build "MetaController.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MetaController.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MetaController.dll"]