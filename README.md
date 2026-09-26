# E-Commerce Admin Management Portal

A robust, full-stack E-Commerce Administration Dashboard built with ASP.NET Core 8 MVC and PostgreSQL. This project features a completely custom Role-Based Access Control (RBAC) system using ASP.NET Core Identity, secure JWT cookie-based authentication, and a responsive modern UI.

## 🚀 Tech Stack

* **Framework:** .NET 8 / ASP.NET Core MVC
* **Database:** PostgreSQL (via Npgsql)
* **ORM:** Entity Framework Core (Code-First)
* **Authentication:** ASP.NET Core Identity & JWT (JSON Web Tokens) via Cookies
* **Frontend:** HTML5, CSS3, Bootstrap, Chart.js (Dashboard Analytics)

## ✨ Key Features

* **Custom Authentication & Authorization:** 
  * Extended `ApplicationUser` and `ApplicationRole` models.
  * Granular Permission-based authorization (Read, Write, Delete).
  * JWT tokens securely stored and validated via HTTP-only cookies.
  * Custom Access Denied and Unauthorized redirection handling.
* **Automated Database Management:**
  * EF Core Code-First architecture with customized Identity table mappings.
  * Automated migration execution on application startup.
  * Automated data seeding for Admin users, Explorer users, and base E-Commerce data.
* **E-Commerce Data Structure:**
  * Fully relational schema mapping Customers, Addresses, Categories, Products, Orders, Order Details, and Payments.
* **Modern Dashboard UI:**
  * Professional, responsive UI tailored for data management.
  * Real-time analytical layout using Chart.js.

## 🛠️ Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
* [PostgreSQL](https://www.postgresql.org/download/) installed and running locally or remotely.

### 1. Clone the Repository
```bash
git clone https://github.com/nikaa-dev/ECommerceAdmin.git
cd ECommerceAdmin/src
