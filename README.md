# 📚 BookStore API

A RESTful web service for managing books and authors built with **.NET 10** and **PostgreSQL**.

## 🚀 Tech Stack

- **.NET 10 Web API** — REST API
- **Entity Framework Core** — ORM for PostgreSQL
- **AutoMapper** — Entity-to-DTO mapping
- **FluentValidation** — Request validation
- **PostgreSQL** — Database (Docker)
- **Swagger** — API documentation

## 📁 Architecture
```
BookStore.sln
├── BookStore.Domain # Entities (Author, Book, User)
├── BookStore.Application # DTOs, Services, Validators, AutoMapper
├── BookStore.Infrastructure # EF Core, DbContext, Data Seeder
└── BookStore.WebApi # Controllers, DI, Program.cs
```
## 🔧 Getting Started

### 1. Start PostgreSQL
```bash
docker compose up -d
```

### 2. Apply Migrations
```bash
dotnet ef database update --project BookStore.Infrastructure --startup-project BookStore.WebApi
```
### 3. Run the Application
```bash
dotnet run --project BookStore.WebApi
```

### 4. Open Swagger
http://localhost:5223/swagger


📡 API Endpoints
---
Books
---
| Method |	URL	| Description |  
|-------|-----|----------|
GET	| /api/books |	Get all books
GET	| /api/books/{id} |	Get book by ID 
GET	| /api/books/search?title=&author= |	Search books
GET	| /api/books/paginated?page=1&pageSize=10 |	Get paginated books
POST |	/api/books	| Create a book  
PUT	| /api/books/{id}	| Update a book  
DELETE |	/api/books/{id}	| Delete a book
---
Authors
---
| Method |	URL	| Description |
|-------|-----|----------|    
GET	| /api/authors	| Get all authors
GET	| /api/authors/{id}	| Get author by ID
POST |	/api/authors	| Create an author
PUT	| /api/authors/{id} |	Update an author  
DELETE |	/api/authors/{id} |	Delete an author
---

### 📦 Seed Data
On first run, the application automatically creates 20 authors and 100 books via IDataSeeder.

### ✅ Roadmap

- Book CRUD
- Author CRUD
- DTOs + AutoMapper
- FluentValidation
- Service layer
- Clean architecture
- PostgreSQL + Migrations
- Search & Pagination
- JWT Authentication
- Docker for WebApi 