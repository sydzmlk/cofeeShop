# ☕ CoffeeShop

A full-stack coffee shop web application built with **C#**, **ASP.NET Core MVC**, **Entity Framework Core**, and **Microsoft SQL Server**.

CoffeeShop is a database-driven e-commerce style web application where users can browse products, manage their shopping cart, place orders, interact with blog content, and manage their accounts. The project also includes a dedicated administration panel for managing products, orders, users, blog posts, and contact messages.

---

## 🚀 Overview

CoffeeShop is a practical **ASP.NET Core MVC** project focused on building a structured, real-world web application using the **C# / .NET ecosystem**.

The application follows a clean MVC architecture with separate:

* Controllers
* Models
* ViewModels
* Services
* Data access
* Views
* Database migrations

The project was developed to practice and demonstrate:

* Backend development with C# and .NET
* ASP.NET Core MVC architecture
* Entity Framework Core
* Microsoft SQL Server
* Database design and migrations
* Server-side rendering with Razor Views
* Session management
* Role-based administration
* E-commerce functionality

---

## ✨ Features

### 🛍️ Shop

* Browse coffee shop products
* View product details
* Browse products by category
* Product information and content

### 🛒 Shopping Cart

* Add products to the cart
* Update product quantities
* Remove products from the cart
* Manage cart items
* Calculate cart totals

### 📦 Checkout & Orders

* Checkout workflow
* Order creation
* Order items
* Order status management
* Payment method selection
* Order data management

### 👤 User Management

* User accounts
* User information management
* Role-based access control
* Password reset request functionality
* Different administrative roles

### 🔐 Admin Panel

The application contains a dedicated administration area for managing the platform.

Admin functionality includes:

* User management
* User role management
* Product management
* Order management
* Blog management
* Contact message management
* Customer reply functionality
* Administrative operations

### 📝 Blog

* Create and manage blog posts
* View blog posts
* Blog comments
* Comment management
* Administrative blog management

### 📩 Contact System

* Contact messages
* Administrative message management
* Customer reply functionality

---

## 🏗️ Project Architecture

The project follows the **ASP.NET Core MVC** architectural pattern.

```text
CoffeeShop
│
├── Controllers
│   ├── AdminController.cs
│   ├── CartController.cs
│   ├── CheckoutController.cs
│   ├── HomeController.cs
│   └── ShopController.cs
│
├── Data
│   ├── AppDbContext.cs
│   └── DbInitializer.cs
│
├── Migrations
│
├── Models
│   ├── AdminUser.cs
│   ├── BlogComment.cs
│   ├── BlogPost.cs
│   ├── Category.cs
│   ├── ContactMessage.cs
│   ├── ContactReply.cs
│   ├── Order.cs
│   ├── OrderItem.cs
│   ├── OrderStatus.cs
│   ├── PasswordResetRequest.cs
│   ├── PaymentMethod.cs
│   ├── Product.cs
│   └── ServiceItem.cs
│
├── Services
│   ├── EmailSender.cs
│   └── SessionExtensions.cs
│
├── ViewModels
│
├── Views
│
├── wwwroot
│
├── Program.cs
├── appsettings.json
├── CoffeeShop.csproj
└── CoffeeShop.sln
```

---

## 🛠️ Technologies

### Backend

* **C#**
* **ASP.NET Core MVC**
* **.NET 9**
* **Entity Framework Core 9**
* **Microsoft SQL Server**

### Frontend

* **Razor Views**
* **HTML5**
* **CSS3**
* **JavaScript**

### Database

* **Microsoft SQL Server**
* **Entity Framework Core**
* **EF Core Migrations**

### Development Tools

* **Visual Studio**
* **Git**
* **GitHub**

---

## 🗄️ Database

The application uses **Microsoft SQL Server** with **Entity Framework Core** for database access and management.

Database-related functionality includes:

* Entity relationships
* Database migrations
* Database initialization
* CRUD operations
* Order and order item management
* User and role management
* Product and category management

---

## 📁 Main Project Components

| Component     | Description                                      |
| ------------- | ------------------------------------------------ |
| `Controllers` | Handles HTTP requests and application flow       |
| `Models`      | Represents database entities and domain data     |
| `ViewModels`  | Transfers and prepares data for views            |
| `Data`        | Contains `DbContext` and database initialization |
| `Services`    | Contains reusable application services           |
| `Views`       | Razor-based user interface                       |
| `Migrations`  | Entity Framework Core database migrations        |
| `wwwroot`     | Static files such as CSS, JavaScript, and images |

---

## ⚙️ Getting Started

### Prerequisites

Before running the project, make sure you have:

* **.NET 9 SDK**
* **Microsoft SQL Server**
* **Visual Studio 2022** or another compatible IDE
* **Git**

### Clone the Repository

```bash
git clone https://github.com/your-username/CoffeeShop.git
cd CoffeeShop
```

### Configure the Database

Update the connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=CoffeeShopDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

### Apply Migrations

Run:

```bash
dotnet ef database update
```

If Entity Framework CLI is not installed:

```bash
dotnet tool install --global dotnet-ef
```

### Run the Application

```bash
dotnet run
```

Or run the project directly through **Visual Studio**.

---

## 🔒 Security

The application includes role-based administrative functionality and account-related features.

Sensitive configuration such as database connection strings, email credentials, and other secrets should be stored securely and should not be committed to the repository.

---

## 🎯 Project Goals

This project was created to gain practical experience with:

* ASP.NET Core MVC
* C# backend development
* Entity Framework Core
* SQL Server
* MVC architecture
* Database-driven applications
* E-commerce workflows
* Authentication and authorization concepts
* Session management
* CRUD operations
* Role-based administration

---

## 📌 Future Improvements

Possible future improvements include:

* Online payment integration
* Product search and filtering
* Product reviews and ratings
* Improved authentication and authorization
* RESTful API integration
* Unit and integration testing
* Docker support
* Deployment to a cloud platform

---

## 📄 License

This project is created for **educational and portfolio purposes**.
