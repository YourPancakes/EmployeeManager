FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["EmployeeManager.Server.csproj", "./"]
RUN dotnet restore "EmployeeManager.Server.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "EmployeeManager.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EmployeeManager.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmployeeManager.Server.dll"] 