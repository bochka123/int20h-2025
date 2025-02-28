FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["./Int20h2025.WebAPI/Int20h2025.WebAPI.csproj", "./Int20h2025.WebAPI/"]
COPY ["./Int20h2025.Auth/Int20h2025.Auth.csproj", "./Int20h2025.Auth/"]
COPY ["./Int20h2025.BLL/Int20h2025.BLL.csproj", "./Int20h2025.BLL/"]
COPY ["./Int20h2025.DAL/Int20h2025.DAL.csproj", "./Int20h2025.DAL/"]
COPY ["./Int20h2025.Common/Int20h2025.Common.csproj", "./Int20h2025.Common/"]

RUN dotnet restore "./Int20h2025.WebAPI/Int20h2025.WebAPI.csproj"
COPY . .
WORKDIR "/src/Int20h2025.WebAPI"
RUN dotnet build "Int20h2025.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "./Int20h2025.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV ASPNETCORE_HTTP_PORTS 80
ENTRYPOINT ["dotnet", "Int20h2025.WebAPI.dll"]