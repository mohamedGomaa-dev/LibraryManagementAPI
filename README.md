# 📚 Library Management System API

A robust, enterprise-grade RESTful API built with **.NET 8** and **C#** to manage a physical library's operations. This project demonstrates advanced backend architecture, focusing heavily on security, clean code principles, and efficient data access.

## 🚀 Features

* **Advanced Authentication & Security:** * JWT-based authentication.
    * Password hashing using BCrypt.
    * **Role-Based Authorization:** Strict access control distinguishing between `Admin` (librarians) and members.
    * **Policy-Based Authorization (Ownership Rules):** Enforces Zero-Trust architecture; users can only access or modify their own active borrowing records and fines.
* **Book & Inventory Management:** Complete CRUD operations for books and tracking physical copies.
* **Borrowing & Reservations:** Complex business logic handling book check-outs, returns, and reservation queues.
* **Fine Management:** Automated calculation and payment processing for overdue books.

## 🛠️ Tech Stack & Architecture

* **Framework:** .NET 8 (ASP.NET Core Web API)
* **Language:** C#
* **Database:** SQL Server & Entity Framework Core (Code-First)
* **Architecture Patterns:** * N-Tier / Clean Architecture concepts
    * Repository Pattern & Unit of Work (for abstracting data access)
* **Libraries/Tools:** AutoMapper (for DTOs), Swagger/OpenAPI (for documentation)

## 🧠 Key Concepts Demonstrated

As a backend developer, this project was built to showcase understanding of enterprise patterns:
* **Secure by Default:** Utilizing global authorization filters and selectively opening endpoints using `[AllowAnonymous]`.
* **Data Transfer Objects (DTOs):** Preventing over-posting and protecting domain entities from external exposure.
* **Separation of Concerns:** Decoupling business logic into dedicated Service layers, keeping Controllers thin and focused purely on HTTP routing.
* **Token ID Enforcement:** Extracting user claims directly from the JWT rather than trusting client-provided payloads for sensitive operations.

## ⚙️ Getting Started

### Prerequisites
* [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
* SQL Server (LocalDB or standard instance)

### Installation
1. Clone the repository:
   ```bash
   git clone [https://github.com/mohamedGomaa-dev/LibraryManagementAPI.git](https://github.com/mohamedGomaa-dev/LibraryManagementAPI.git)
Navigate to the project directory:

Bash
cd LibraryManagementAPI
Update the Database Connection String in appsettings.json to match your local SQL Server instance.

Apply Entity Framework Migrations to create the database:

Bash
dotnet ef database update
Run the application:

Bash
dotnet run
Open your browser and navigate to https://localhost:[PORT]/swagger to interact with the API endpoints.

🤝 Contact
Created by Mohamed Gomaa - Feel free to contact me on LinkedIn!
