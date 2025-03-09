FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

COPY . ./
RUN dotnet restore
RUN dotnet publish spliteasy.Api/spliteasy.Api.csproj -o out

EXPOSE 8080
ENV HTTP_PORT=8080
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "spliteasy.Api.dll"]
