FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["ApI.csproj", "ApI.csproj"]
RUN dotnet restore "ApI.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ApI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ApI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApI.dll"]