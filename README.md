```
# E-Commerce Admin Management Portal

A robust, full-stack E-Commerce Administration Dashboard built with ASP.NET Core 8 MVC and PostgreSQL. This project features a completely custom Role-Based Access Control (RBAC) system using ASP.NET Core Identity, secure JWT cookie-based authentication, and a responsive modern UI.

## 🚀 Tech Stack

* **Framework:** .NET 8 / ASP.NET Core MVC
* **Database:** PostgreSQL (via Npgsql)
* **ORM:** Entity Framework Core (Code-First)
* **Authentication:** ASP.NET Core Identity & JWT (JSON Web Tokens) via Cookies
* **Frontend:** HTML5, CSS3, Bootstrap, Chart.js

## ✨ Key Features

* **Custom Authentication & Authorization:** Extended Identity models, granular permissions, and secure JWT HTTP-only cookies.
* **Automated Database Management:** EF Core Code-First architecture with automated migrations and data seeding on startup.
* **E-Commerce Data Structure:** Fully relational schema mapping Customers, Addresses, Categories, Products, Orders, Order Details, and Payments.
* **Modern Dashboard UI:** Professional, responsive UI tailored for data management with real-time analytical layouts.

## 🛠️ Local Project Setup (After Cloning)

Follow these steps to get the project running on your local machine after cloning the repository.

### 1. Verify Prerequisites
Before proceeding, ensure you have the required tools installed on your system.
* Check your **.NET version** (you need .NET 8 or higher). Open your terminal and run:
  ```bash
  dotnet --version



* Ensure you have **PostgreSQL** installed and running on your local machine.

### 2. Configure Database Credentials

Open `appsettings.json` (or `appsettings.Development.json`) in the root of the project. Check the `ConnectionStrings` section and ensure the `Database` name, `Username`, and `Password` match your local PostgreSQL server configuration:

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=ECommerceAdminDb;Username=YOUR_USERNAME;Password=YOUR_PASSWORD"
}



### 3. Restore Packages and Run the Project

Open your terminal in the project directory, restore the required NuGet packages, and start the application:

```bash
dotnet restore
dotnet run



* **Automatic Setup:** Running the project will automatically create the database, generate the tables, and seed the default Admin user, Explorer user, and role permissions.

### 4. Database Troubleshooting & Mock Data

* **Manual Database Creation:** If the application fails to automatically create the database upon running, you can manually execute the provided `CreateDb.sql` (EF Core creation script) directly in your PostgreSQL database tool (like pgAdmin or DBeaver).
* **Mock Data Initialization:** If you want to populate your dashboard with sample data for testing (categories, products, orders, and dummy customers), execute the provided `InsertMockdata.sql` in your database after the initial setup is complete.
* **Note:** These scripts can be found in the following path: `/src/DBConnection/`

## 🔐 Default Credentials

Upon successful database creation and seeding, use the following credentials to log in:

**System Administrator (Full Access):**

* **Email:** `admin@system.com`
* **Password:** `SuperSecret@123!`

**Explorer / Staff (Default Test User):**

* **Email:** `explore@system.com`
* **Password:** `SuperSecret@123!`
