# 📚 BookStore API

[![.NET Build & Tests](https://github.com/sokomi30/BookStore/actions/workflows/dotnet.yml/badge.svg)](https://github.com/sokomi30/BookStore/actions/workflows/dotnet.yml)

A full-stack web service for managing books and authors built with **.NET 10**, **React + TypeScript**, and **PostgreSQL**.

## 🚀 Tech Stack

### Backend
- **.NET 10 Web API** — REST API
- **Entity Framework Core** — ORM for PostgreSQL
- **AutoMapper** — Entity-to-DTO mapping
- **FluentValidation** — Request validation
- **JWT Authentication** — Role-based access control (User / Admin)
- **Redis** — Distributed caching
- **Serilog** — Structured logging

### Frontend
- **React 19** — UI library
- **TypeScript** — Type safety
- **React Router** — Client-side routing
- **Tailwind CSS** — Utility-first styling
- **Axios** — HTTP client

### DevOps
- **PostgreSQL** — Database (Docker)
- **Docker Compose** — One-command startup
- **GitHub Actions** — CI/CD pipeline
- **Nginx** — Reverse proxy for frontend
- **xUnit + Moq** — 32 tests (unit + integration)

## 📁 Architecture

```
BookStore.sln
├── BookStore.Domain          # Entities (Author, Book, User)
├── BookStore.Application     # DTOs, Services, Validators, AutoMapper
├── BookStore.Infrastructure  # EF Core, DbContext, Data Seeder
├── BookStore.WebApi          # Controllers, Middleware, DI, Program.cs
└── BookStore.Web             # React + TypeScript frontend
```

## 🔧 Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)

### Quick Start (one command)
```bash
git clone https://github.com/sokomi30/BookStore.git
cd BookStore
docker compose up -d --build
```

- **Frontend:** http://localhost:3000
- **Swagger API:** http://localhost:5000/swagger

### Stop
```bash
docker compose down
```

### Development (run manually)

#### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Node.js 20+](https://nodejs.org/)

#### Backend
```bash
# Start infrastructure
docker compose up -d postgres redis

# Apply migrations
dotnet ef database update --project BookStore.Infrastructure --startup-project BookStore.WebApi

# Run API
dotnet run --project BookStore.WebApi
```
API: http://localhost:5000/swagger

#### Frontend
```bash
cd BookStore.Web
npm install
npm run dev
```
Frontend: http://localhost:5173

### Default Admin Account
```
Username: admin
Password: set in appsettings.Development.json
```

### Running Tests
```bash
dotnet test
```

## 📡 API Endpoints

### Auth
| Method | URL | Description | Access |
|--------|-----|-------------|--------|
| POST | `/api/auth/register` | Register new user | Public |
| POST | `/api/auth/login` | Login, get JWT tokens | Public |
| POST | `/api/auth/refresh` | Refresh access token | Public |

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
| POST | `/api/books/{id}/cover` | Upload book cover | Admin |

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
- **Refresh tokens** (7 days expiry)
- **BCrypt** password hashing
- **Rate limiting** on login (5 requests/min)
- **Input validation** via FluentValidation
- **Global exception handling** middleware
- **JWT key** stored in environment variables

## 📦 Seed Data

On first run, the application automatically creates **20 authors** and **100 books** via `IDataSeeder`.

## ✅ Roadmap

### Core
- [x] Book CRUD
- [x] Author CRUD
- [x] DTOs + AutoMapper
- [x] FluentValidation
- [x] Service layer
- [x] Clean architecture
- [x] PostgreSQL + Migrations

### API Features
- [x] Search & Pagination
- [x] JWT Authentication
- [x] Role-based authorization (User / Admin)
- [x] Refresh tokens
- [x] Rate limiting
- [x] Global exception handling
- [x] Structured logging (Serilog)
- [x] Redis caching

### Frontend
- [x] React + TypeScript SPA
- [x] Tailwind CSS styling
- [x] Dark / Light theme
- [x] Admin panel (books & authors)
- [x] Book cover upload

### DevOps
- [x] Docker Compose (one-command run)
- [x] CI/CD (GitHub Actions)
- [ ] Kubernetes deployment

### Testing
- [x] Unit tests (25)
- [x] Integration tests (7)
- [ ] API tests (Postman / Bruno collection)