# Etapa de construção
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /app

COPY PostgreSQL.Backup.AWS.S3.sln ./

COPY src/PostgreSQL.Backup.AWS.S3/PostgreSQL.Backup.AWS.S3.csproj ./src/PostgreSQL.Backup.AWS.S3/
RUN dotnet restore

COPY src/PostgreSQL.Backup.AWS.S3/. ./src/PostgreSQL.Backup.AWS.S3/
RUN dotnet publish src/PostgreSQL.Backup.AWS.S3/PostgreSQL.Backup.AWS.S3.csproj -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine
WORKDIR /app

RUN apk add --no-cache postgresql16-client

COPY --from=build /app/out .

CMD ["dotnet", "PostgreSQL.Backup.AWS.S3.dll"]