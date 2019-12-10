FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["Controller.Knowledge/Controller.Knowledge.csproj", "Controller.Knowledge/"]
RUN dotnet restore "Controller.Knowledge/Controller.Knowledge.csproj"
COPY . .
WORKDIR "/src/Controller.Knowledge"
RUN dotnet build "Controller.Knowledge.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Controller.Knowledge.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Controller.Knowledge.dll"]