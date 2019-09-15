#FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
FROM mcr.microsoft.com/dotnet/core/aspnet:3.0.0-preview9 AS base
WORKDIR /app
EXPOSE 80

#FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
FROM mcr.microsoft.com/dotnet/core/sdk:3.0.100-preview9 AS build
WORKDIR /src
COPY ["latency-web.csproj", ""]
RUN dotnet restore "./latency-web.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "latency-web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "latency-web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "latency-web.dll"]