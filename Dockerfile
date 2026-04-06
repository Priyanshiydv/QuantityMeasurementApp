FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

COPY *.sln .
COPY QuantityMeasurement.Models/*.csproj QuantityMeasurement.Models/
COPY QuantityMeasurement.Repository/*.csproj QuantityMeasurement.Repository/
COPY QuantityMeasurement.Service/*.csproj QuantityMeasurement.Service/
COPY QuantityMeasurement.WebAPI/*.csproj QuantityMeasurement.WebAPI/
COPY QuantityMeasurementApp/*.csproj QuantityMeasurementApp/
COPY QuantityMeasurementApp.Tests/*.csproj QuantityMeasurementApp.Tests/

RUN dotnet restore

COPY . .

RUN dotnet publish QuantityMeasurement.WebAPI/QuantityMeasurement.WebAPI.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "QuantityMeasurement.WebAPI.dll"]