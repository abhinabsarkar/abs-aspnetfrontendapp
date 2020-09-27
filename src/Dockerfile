# https://hub.docker.com/_/microsoft-dotnet-core
FROM mcr.microsoft.com/dotnet/core/sdk:3.1-alpine AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY aspnetfrontendapp/*.csproj ./aspnetfrontendapp/
RUN dotnet restore

# copy everything else and build app
COPY aspnetfrontendapp/. ./aspnetfrontendapp/
WORKDIR /source/aspnetfrontendapp
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-alpine AS runtime
# Add bash
RUN apk update && apk add bash
# Add curl
RUN apk --no-cache add curl
# Copy the image built in previously
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "aspnetfrontendapp.dll"]
