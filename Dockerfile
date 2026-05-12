FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["TuPenca.API/TuPenca.API.csproj", "TuPenca.API/"]
COPY ["TuPenca.Application/TuPenca.Application.csproj", "TuPenca.Application/"]
COPY ["TuPenca.Domain/TuPenca.Domain.csproj", "TuPenca.Domain/"]
COPY ["TuPenca.Infrastructure/TuPenca.Infrastructure.csproj", "TuPenca.Infrastructure/"]

RUN dotnet restore "TuPenca.API/TuPenca.API.csproj"

COPY . .
WORKDIR "/src/TuPenca.API"
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "TuPenca.API.dll"]