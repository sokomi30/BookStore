# 📚 BookStore API

A RESTful web service for managing books and authors built with **.NET 10** and **PostgreSQL**.

## 🚀 Tech Stack

- **.NET 10 Web API** — REST API
- **Entity Framework Core** — ORM for PostgreSQL
- **AutoMapper** — Entity-to-DTO mapping
- **FluentValidation** — Request validation
- **JWT Authentication** — Role-based access control (User / Admin)
- **Serilog** — Structured logging
- **PostgreSQL** — Database (Docker)
- **Swagger** — API documentation

## 📁 Architecture

```
BookStore.sln
├── BookStore.Domain          # Entities (Author, Book, User)
├── BookStore.Application     # DTOs, Services, Validators, AutoMapper
├── BookStore.Infrastructure  # EF Core, DbContext, Data Seeder
└── BookStore.WebApi          # Controllers, Middleware, DI, Program.cs
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
```
http://localhost:5000/swagger
```

## 🔐 Authentication

| Endpoint | Description | Access |
|----------|-------------|--------|
| `POST /api/auth/register` | Register new user | Public |
| `POST /api/auth/login` | Login, get JWT token | Public (rate limited: 5/min) |

### Default Admin (Development)
```
Username: admin
Password: from appsettings.Development.json
```

## 📡 API Endpoints

### Books
| Method | URL | Description | Access |
|--------|-----|-------------|--------|
| GET | `/api/books` | Get all books | Public |
| GET | `/api/books/{id}` | Get book by ID | Public |
| GET | `/api/books/search?title=&author=` | Search books | Public |
| GET | `/api/books/paginated?page=1&pageSize=10` | Get paginated books | Public |
| POST | `/api/books` | Create a book | Admin |
| PUT | `/api/books/{id}` | Update a book | Admin |
| DELETE | `/api/books/{id}` | Delete a book | Admin |

### Authors
| Method | URL | Description | Access |
|--------|-----|-------------|--------|
| GET | `/api/authors` | Get all authors | Public |
| GET | `/api/authors/{id}` | Get author by ID | Public |
| POST | `/api/authors` | Create an author | Admin |
| PUT | `/api/authors/{id}` | Update an author | Admin |
| DELETE | `/api/authors/{id}` | Delete an author | Admin |

## 🛡️ Security

- **JWT** with role-based authorization (User / Admin)
- **BCrypt** password hashing
- **Rate limiting** on login endpoint (5 requests/min)
- **Input validation** via FluentValidation
- **Global exception handling** middleware
- **JWT key** stored in environment variables (not in repository)

## 📦 Seed Data

On first run, the application automatically creates **20 authors** and **100 books** via `IDataSeeder`.

## ✅ Roadmap

- [x] Book CRUD
- [x] Author CRUD
- [x] DTOs + AutoMapper
- [x] FluentValidation
- [x] Service layer
- [x] Clean architecture
- [x] PostgreSQL + Migrations
- [x] Search & Pagination
- [x] JWT Authentication
- [x] Role-based authorization
- [x] Rate limiting
- [x] Structured logging (Serilog)
- [ ] Unit & Integration tests
- [ ] Docker for WebApi