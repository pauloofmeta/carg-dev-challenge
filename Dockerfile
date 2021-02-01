FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /app

COPY ./src/MyAddresses.Webapi/*.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish ./src/MyAddresses.Webapi/MyAddresses.Webapi.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine
WORKDIR /app
COPY --from=build /app/out .
EXPOSE 80 443
ENTRYPOINT [ "dotnet", "MyAddresses.Webapi.dll" ]