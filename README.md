# OnlineLearningPlatform

## Overview

An ASP.NET Core MVC application for comprehensive online course management. This platform enables educators to create and manage courses while providing students with an interactive learning experience through structured content and quizzes.

## Features

### User Management

- Role-based authentication (Admin, Instructor, Student)
- User profile management with profile pictures
- Secure identity management using ASP.NET Core Identity

### Course Management

- Create, edit, and delete courses
- Organize content into modules and lessons
- Rich text content support
- Video integration capabilities
- Course pricing and enrollment management

### Payment Integration

- Secure payment processing with Stripe
- Course pricing management
- Payment history tracking
- Refund handling capabilities

### Technical Features

- Entity Framework Core for data management
- SQL Server database backend
- CRUD operations for all entities
- Custom authorization policies
- Unite Of Work

## Prerequisites

- .NET 9.0 SDK
- SQL Server 2019 or later
- Stripe account for payment processing (optional)
- Visual Studio 2022 or VS Code
- Node.js (for frontend asset management)

## Getting Started

1. Clone the repository:

```bash
git clone https://github.com/yourusername/OnlineLearningPlatform.git
```

2. Configure the database connection in appsettings.json:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Your-Connect-string"
  }
}
```

3. Run database migrations:

```bash
dotnet ef database update
```

4. Configure Stripe settings (optional):

```json
{
  "Stripe": {
    "SecretKey": "your_stripe_secret_key",
    "PublishableKey": "your_stripe_publishable_key"
  }
}
```

5. Build and run the project:

```bash
dotnet build
dotnet run
```

## Project Structure

- [`Context/`](Context/): Entity Framework DbContext and configurations
- [`Controllers/`](Controllers/): MVC controllers handling application logic
- [`Models/`](Models/): Domain models representing business entities
- [`ViewModels/`](ViewModels/): DTOs for view-specific data
- [`Views/`](Views/): Razor views for the user interface
- [`Repositories/`](Repositories/): Data access layer implementation
- [`Policy/`](Policy/): Custom authorization policies
- [`wwwroot/`](wwwroot/): Static files (CSS, JavaScript, images)
