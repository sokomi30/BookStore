# 📚 BookStore API

Веб-сервис для управления книгами и авторами на **.NET 10** с **PostgreSQL**.

## 🚀 Технологии

- **.NET 10 Web API** — REST API
- **Entity Framework Core** — ORM для PostgreSQL
- **AutoMapper** — маппинг сущностей в DTO
- **FluentValidation** — валидация входящих данных
- **PostgreSQL** — база данных (Docker)
- **Swagger** — документация API

## 📁 Архитектура
```
BookStore.sln
├── BookStore.Domain # Сущности (Author, Book, User)
├── BookStore.Application # DTO, сервисы, валидаторы, AutoMapper
├── BookStore.Infrastructure # EF Core, DbContext, сидер
└── BookStore.WebApi # Контроллеры, DI, Program.cs
```
## 🔧 Запуск

### 1. Поднять PostgreSQL
```bash
docker compose up -d
```

### 2.Применить миграции
```bash
dotnet ef database update --project BookStore.Infrastructure --startup-project BookStore.WebApi
```
### 3. Запустить приложение
```bash
dotnet run --project BookStore.WebApi
```

### 4. Открыть Swagger
http://localhost:5223/swagger


📡 API
---
Книги
---
| Метод |	URL |	Описание |  
|-------|-----|----------|
GET	| /api/books |	Все книги  
GET	| /api/books/{id} |	Книга по ID  
POST |	/api/books	| Создать книгу  
PUT	| /api/books/{id}	| Обновить книгу  
DELETE |	/api/books/{id}	| Удалить книгу  
---
Авторы
---
| Метод	| URL	| Описание|
|-------|-----|----------|    
GET	| /api/authors	| Все авторы  
GET	| /api/authors/{id}	| Автор по ID  
POST |	/api/authors	| Создать автора  
PUT	| /api/authors/{id} |	Обновить автора  
DELETE |	/api/authors/{id} |	Удалить автора  
---

### 📦 Тестовые данные
При запуске автоматически создаётся 20 авторов и 100 книг через IDataSeeder.

### ✅ Что готово

- CRUD для книг  
- CRUD для авторов  
- DTO + AutoMapper  
- Валидация FluentValidation  
- Сервисный слой  
- Чистая архитектура  
- PostgreSQL + миграции  
- Поиск книг  
- JWT авторизация  
- Docker для WebApi  