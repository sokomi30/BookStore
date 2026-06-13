FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

# Копируем проекты
COPY BookStore.Domain/BookStore.Domain.csproj BookStore.Domain/
COPY BookStore.Application/BookStore.Application.csproj BookStore.Application/
COPY BookStore.Infrastructure/BookStore.Infrastructure.csproj BookStore.Infrastructure/
COPY BookStore.WebApi/BookStore.WebApi.csproj BookStore.WebApi/

# Восстанавливаем зависимости
RUN dotnet restore BookStore.WebApi/BookStore.WebApi.csproj

# Копируем весь код
COPY . .

# Собираем и публикуем
WORKDIR /src/BookStore.WebApi
RUN dotnet publish -c Release -o /app

# Финальный образ
FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

ENTRYPOINT ["dotnet", "BookStore.WebApi.dll"]