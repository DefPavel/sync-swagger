# Образ sdk
FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine

WORKDIR /var/www/html/asp

ENV DOTNET_USE_POLLING_FILE_WATCHER 1

COPY . /var/www/html/asp

WORKDIR /var/www/html/asp

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]
# Пересобирать если есть изменения в файлах
ENTRYPOINT dotnet watch run --no-restore --urls=http://+:5000
