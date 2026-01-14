# MyShop (Martin’s Books)

An ASP.NET Core Razor Pages web application for managing books, users, orders, and messages.  
Built using C# and running on .NET 6.

## Tech Stack

- **Framework:** ASP.NET Core
- **UI Model:** Razor Pages
- **Language:** C#
- **Runtime:** .NET 6
- **Data Access:** ADO.NET / SqlClient
- **Database:** SQL Server
- **Authentication:** Session-based authentication
- **IDE:** Visual Studio 2022

## Project Structure

- `Pages/` – Razor Pages organized by feature (Admin, Books, Orders, Users, etc.)
- `Pages/*.cshtml` – UI views using Razor syntax
- `Pages/*.cshtml.cs` – PageModel classes (request handling and logic)
- `wwwroot/` – Static assets
- `appsettings.json` – Application configuration
- `Program.cs` – Application startup and middleware pipeline

## Key Features

- Admin management of users, books, and orders
- Server-rendered pages using Razor
- Role-based authorization for admin routes
- SQL Server integration via SqlClient
- Clean separation of UI and request logic via Razor Pages

## Running Locally

1. Open the solution in **Visual Studio 2022**
2. Ensure a SQL Server instance is available
3. Update connection strings in `appsettings.json`

## Notes
-Enable/Disable the SQL Server Database:

In order to get the website to show the book items, you need to:
- go to the AppSettings.json file in Visual Studio.
- Comment out the connectionString to the SQLExpress local database.
- Then uncomment the connectionString to the Azure SQLServer database.
- Then Publish to Azure through Visual Studio! RtClick>Publish
- DON'T forget to swap it back so you do not use up your FREE database DTUs!!!
- Connecting to your Azure SQL Database through SSMS itself doesn't consume DTUs,
but the actions you perform while connected will affect your DTU usage.
