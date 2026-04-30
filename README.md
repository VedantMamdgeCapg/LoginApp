# LoginApp – ASP.NET Core 8 MVC

A secure **ASP.NET Core 8 MVC** application implementing authentication and authorization using **ASP.NET Core Identity**, **Entity Framework Core**, **Azure App Configuration**, and **Azure Key Vault**.

This project follows security best practices and is designed to be GitHub-safe, cloud-friendly, and production-ready.

---

## Features

- ASP.NET Core 8 MVC
- ASP.NET Core Identity
- Login and registration functionality
- Role-based authorization
- Entity Framework Core
- SQL Server / Azure SQL support
- Azure App Configuration
- Azure Key Vault integration
- Email support
- Environment-based configuration
- GitHub-safe configuration with no secrets committed

---

## Tech Stack

- .NET 8
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server / Azure SQL
- Azure App Configuration
- Azure Key Vault
- Visual Studio 2022 / VS Code

---

## Project Structure

```text
LoginApp/
│
├── Controllers/
├── Models/
├── Views/
├── Data/
├── Services/
├── wwwroot/
│
├── appsettings.json
├── Program.cs
├── LoginApp.csproj
├── README.md
└── .gitignore
```

---

## Configuration and Secrets

This project uses environment-based configuration.

### Tracked in GitHub

The following file is safe to commit:

```text
appsettings.json
```

`appsettings.json` should contain only safe, non-sensitive default settings.

Example:

```json
{
  "AllowedHosts": "*",
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Ignored by Git

The following files should not be committed:

```text
appsettings.Development.json
appsettings.Production.json
appsettings.Staging.json
```

These files may contain environment-specific values such as:

- Database connection strings
- API keys
- Email credentials
- Azure service connection details
- Client secrets
- Certificates

---

## Azure App Configuration and Key Vault

This application is designed to avoid storing secrets directly in source control.

Recommended approach:

- Use **Azure App Configuration** for non-secret application settings.
- Use **Azure Key Vault** for sensitive values such as:
  - Database connection strings
  - Email passwords
  - API keys
  - Client secrets
  - Certificates

For local development, use one of the following options:

- .NET User Secrets
- Environment variables
- Azure App Configuration with Key Vault references

---

## Prerequisites

Before running this project, make sure you have the following installed:

- .NET 8 SDK
- SQL Server or Azure SQL Database
- Visual Studio 2022 or Visual Studio Code
- Entity Framework Core CLI tools

Install EF Core CLI tools if required:

```bash
dotnet tool install --global dotnet-ef
```

If already installed, update it using:

```bash
dotnet tool update --global dotnet-ef
```

---

## Getting Started

### 1. Clone the Repository

```bash
git clone <repository-url>
cd LoginApp
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Configure Secrets

Do not add secrets directly into `appsettings.json`.

For local development, you can use .NET User Secrets:

```bash
dotnet user-secrets init
```

Example for setting a database connection string:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-local-connection-string"
```

Example for setting email configuration:

```bash
dotnet user-secrets set "EmailSettings:SmtpServer" "your-smtp-server"
dotnet user-secrets set "EmailSettings:SmtpPort" "587"
dotnet user-secrets set "EmailSettings:Username" "your-email@example.com"
dotnet user-secrets set "EmailSettings:Password" "your-email-password"
```

### 4. Apply Database Migrations

```bash
dotnet ef database update
```

### 5. Run the Application

```bash
dotnet run
```

Open the application in your browser:

```text
https://localhost:5001
```

or

```text
http://localhost:5000
```

---

## Entity Framework Core Migrations

To create a new migration:

```bash
dotnet ef migrations add InitialCreate
```

To update the database:

```bash
dotnet ef database update
```

To remove the last migration:

```bash
dotnet ef migrations remove
```

---

## Authentication and Authorization

This project uses **ASP.NET Core Identity** for user management.

Implemented features may include:

- User registration
- User login
- Password hashing
- Cookie authentication
- Role-based authorization
- Secure logout
- Identity database tables

---

## Email Configuration

Email functionality should be configured using secure settings.

Do not store email passwords in source control.

Recommended storage:

- Azure Key Vault
- Environment variables
- .NET User Secrets for local development

Example configuration keys:

```text
EmailSettings:SmtpServer
EmailSettings:SmtpPort
EmailSettings:Username
EmailSettings:Password
EmailSettings:FromEmail
```

---

## Source Control Guidelines

This repository should not contain:

- Passwords
- API keys
- Database connection strings
- Azure client secrets
- Certificates
- `.pfx`, `.snk`, `.key`, `.cer` files
- `bin/`
- `obj/`
- `.vs/`

Before pushing to GitHub, check your files:

```bash
git status
```

If a sensitive file was accidentally tracked, remove it from Git tracking:

```bash
git rm --cached appsettings.Development.json
git commit -m "Remove sensitive config from tracking"
```

---

## Recommended `.gitignore` Rules

Make sure your `.gitignore` includes:

```gitignore
bin/
obj/
.vs/

appsettings.*.json
!appsettings.json

secrets.json
*.pfx
*.snk
*.key
*.cer

logs/
*.log
```

---

## Build

To build the project:

```bash
dotnet build
```

---

## Publish

To publish the project:

```bash
dotnet publish -c Release -o ./publish
```

The published output will be generated inside:

```text
publish/
```

---

## Deployment Notes

For production deployment:

- Store secrets in Azure Key Vault.
- Store non-secret settings in Azure App Configuration.
- Use Managed Identity where possible.
- Do not deploy local development configuration files.
- Configure production connection strings outside the repository.
- Enable HTTPS.
- Review authentication and authorization settings.

---

## Best Practices Followed

- No secrets committed to source control
- Secure configuration management
- Environment-based settings
- ASP.NET Core Identity for authentication
- Entity Framework Core for database access
- Azure-ready architecture
- Clean MVC project structure

---

## Author

**Vedant Mamdge**  
Software Engineer
``