FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["ServiceC/ServiceC.csproj", "./"]
RUN dotnet restore "ServiceC/ServiceC.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "ServiceC/ServiceC.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServiceC/ServiceC.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServiceC/ServiceC.dll"]