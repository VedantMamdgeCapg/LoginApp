# LoginApp

A simple ASP.NET Core MVC login application using Identity and Entity Framework Core.

## Features

- User registration and login
- Password reset and forgot password flows
- Email-based account verification support via a configurable email sender
- Identity management with unique email enforcement
- SQL Server-backed user store
- Basic dashboard with account settings

## Technologies

- ASP.NET Core 8.0
- Entity Framework Core 8.0
- ASP.NET Core Identity
- SQL Server

## Getting Started

### Prerequisites

- .NET 8.0 SDK
- SQL Server or Azure SQL Database
- Optional: an SMTP-compatible email service for password reset emails

### Setup

1. Clone the repository.
2. Update `appsettings.json` and `appsettings.Development.json` with your SQL Server connection string under `ConnectionStrings:DefaultConnection`.
3. Update the `EmailSettings` section with your SMTP values if you want email sending enabled.
4. Apply database migrations:

```bash
cd LoginApp
dotnet ef database update
```

5. Run the application:

```bash
dotnet run --project LoginApp.csproj
```

6. Open the app in your browser at `https://localhost:5001` or the displayed URL.

## Tests

The solution includes a test project in `LoginApp.Tests`.

Run tests with:

```bash
dotnet test LoginApp.Tests/LoginApp.Tests.csproj
```

## Notes

- The app uses ASP.NET Core Identity with custom password policy settings.
- The default authentication cookie is configured for a 60-minute session with sliding expiration.

## License

This project is provided as-is.
