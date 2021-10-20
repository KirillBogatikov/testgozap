FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /app
EXPOSE 80
COPY . .
WORKDIR /app/ServiceC
RUN dotnet restore
RUN dotnet publish -c Release -o /app/publish
FROM build
WORKDIR /app
COPY --from=build /app/ServiceC/out .
ENTRYPOINT ["dotnet", "ServiceC.dll"]