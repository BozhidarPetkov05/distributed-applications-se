## ФН: 2401321006
## Име: Божидар Петков

# CarSales

## Overview

CarSales is a web application for managing car listings and sales. It provides a dashboard where users can browse, add, edit, and delete car listings with photos, manage user accounts, and control application settings.

The project is split into two parts:

- **Backend** — an ASP.NET Core 10 Web API with JWT authentication, Entity Framework Core (SQL Server), a layered architecture (Controllers → Services → Repositories → Data), and Swagger UI for exploring endpoints.
- **Frontend** — a React 19 + Vite single-page application with a sidebar dashboard layout featuring three main pages: Cars, Users, and Settings.

### Key features

- JWT-based authentication (login / sign-up)
- Car listings with brand, model, year, price, fuel type, transmission, colour, power, engine volume, description, and photo gallery
- Photo upload and management per listing
- Paginated car and user listings
- Role-aware user management
- Global exception handling and structured error responses
- CORS configured for local React dev server

---

## Installation & Setup

### 1. Clone / extract the project

```bash
unzip CarSales-main.zip
cd CarSales-main
```

### 2. Configure the database connection

Open `CarSales/appsettings.json` and update the connection string to match your SQL Server instance and credentials:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\sqlexpress;Database=CarSalesDb;User Id=carsale;Password=carsale;TrustServerCertificate=True;"
}
```

### 3. Apply database migrations

Open Package Manager Console and type:

```bash
Update-Database
```

This creates the `CarSalesDb` database with all required tables.

### 4. Run the backend API

```bash
cd CarSales
dotnet run
```

The API starts on `https://localhost:xxxx` (the exact port is printed in the console). Swagger UI is available at `/swagger`.

### 5. Install frontend dependencies

In a separate terminal:

```bash
cd CarSales.Client
npm install
```

### 6. Run the frontend dev server

```bash
npm run dev
```

Vite starts on `http://localhost:5173` by default. Open that URL in your browser to use the application.

---

## Notes

- Uploaded photos are stored under `CarSales/wwwroot/uploads/` and served as static files.
- To build the frontend for production, run `npm run build` inside `CarSales.Client/`.
