# Quantity Measurement Application

## Project Overview
The Quantity Measurement Application is a full-stack C# project that supports comparing, converting, and performing arithmetic operations on measurement units. It follows clean N-Tier Architecture and exposes a REST API with JWT authentication, Redis caching, and a modern frontend.

## Supported Categories
- **Length:** Feet, Inches, Yards, Centimeters
- **Weight:** Kilograms, Grams, Pounds
- **Volume:** Liters, Milliliters, Gallons
- **Temperature:** Celsius, Fahrenheit, Kelvin

---

## Project Structure
```
QuantityMeasurementApp
├── QuantityMeasurementApp              ← Console App (UC1–UC14)
├── QuantityMeasurementApp.Tests        ← Unit Tests (MSTest)
├── QuantityMeasurement.Models          ← DTOs, Entities, Exceptions
├── QuantityMeasurement.Repository      ← EF Core, ADO.NET, Redis
├── QuantityMeasurement.Service         ← Business Logic, Auth, Encryption
└── QuantityMeasurement.WebAPI          ← ASP.NET Core REST API
```

---

## Branches and Use Cases

| UC | Branch | Description |
|----|--------|-------------|
| UC1 | `feature/UC1-FeetMeasurementEquality` | Compare equality between Feet quantities |
| UC2 | `feature/UC2-InchMeasurementEquality` | Compare equality between Inch quantities |
| UC3 | `feature/UC3-GenericQuantityLength` | Generic Length comparison (Feet & Inch) |
| UC4 | `feature/UC4-ExtendedUnitSupport` | Extend to Yards & Centimeters |
| UC5 | `feature/UC5-UnitToUnitConversion` | Conversion between length units |
| UC6 | `feature/UC6-UnitAddition` | Add two quantities in first operand unit |
| UC7 | `feature/UC7-TargetUnitAddition` | Add and return result in target unit |
| UC8 | `feature/UC8-StandaloneUnit` | Refactor unit enums for SRP |
| UC9 | `feature/UC9-WeightMeasurementSupport` | Weight: Kilogram, Gram, Pound |
| UC10 | `feature/UC10-GenericMeasurementRefactor` | Generic Quantity class for multi-category |
| UC11 | `feature/UC11-VolumeMeasurementSupport` | Volume: Liters, Milliliters, Gallons |
| UC12 | `feature/UC12-QuantitySubtractionDivision` | Subtraction and Division support |
| UC13 | `feature/UC13-ArithmeticValidation` | Centralized arithmetic validation |
| UC14 | `feature/UC14-TemperatureMeasurementSupport` | Temperature with selective arithmetic |
| UC15 | `feature/UC15-NTierArchitectureRefactoring` | N-Tier Architecture refactoring |
| UC16 | `feature/UC16-DatabaseIntegrationWithADONet` | ADO.NET + MS SQL Server integration |
| UC17 | `feature/UC17-ASPNETFrameworkIntegration` | ASP.NET Core REST API + EF Core |
| UC18 | `feature/UC18-JWTAuthentication` | JWT Auth, BCrypt, AES Encryption, Redis, Google OAuth |

---

## Key Features

### UC1–UC14: Core Measurement Logic
- Type-safe operations via generics and interfaces
- Unit conversion across all 4 categories
- Arithmetic operations with validation
- Selective arithmetic (Temperature restricted)
- Follows SOLID principles — DRY, SRP, ISP
- Custom exception hierarchy

### UC15: N-Tier Architecture
- **Controller** → **Service** → **Repository** → **Model** layers
- Singleton cache repository with JSON disk persistence
- Dependency Injection pattern
- 29 new test cases — Total: 213 tests

### UC16: Database Integration
- ADO.NET with MS SQL Server (SSMS)
- Connection pooling for efficient resource management
- Parameterized queries for SQL injection prevention
- Transaction management — atomic saves
- Audit trail via History table
- Runtime repository switching — Cache or Database
- 29 new test cases — Total: 242 tests

### UC17: ASP.NET Core REST API
- Full REST API with ASP.NET Core + EF Core
- SQL Server with EF Core migrations
- Swagger UI with annotations
- NLog structured logging
- Global exception middleware
- CORS support
- 5 endpoints: Compare, Convert, Add, Subtract, Divide
- History, filter, count endpoints

**API Endpoints:**
```
POST /api/v1/quantities/compare
POST /api/v1/quantities/convert
POST /api/v1/quantities/add
POST /api/v1/quantities/subtract
POST /api/v1/quantities/divide
GET  /api/v1/quantities/history
GET  /api/v1/quantities/history/operation/{type}
GET  /api/v1/quantities/history/measurement/{type}
GET  /api/v1/quantities/count
GET  /api/v1/quantities/count/{operationType}
```

### UC18: Security — JWT, BCrypt, AES, Redis, Google OAuth
- **JWT Authentication** — HMAC-SHA256, 60 min expiry, Bearer token
- **BCrypt Hashing + Salting** — work factor 12, salt auto-embedded in hash
- **Refresh Token** — 256-bit cryptographic, 7 day expiry, rotates on every use
- **AES-256-GCM Encryption/Decryption** — random nonce per encryption
- **Redis Caching** — 5 min TTL, Cache HIT/MISS logging, auto fallback to memory
- **Google OAuth 2.0** — auto-register new Google users
- **Users table** in SQL Server with BCrypt password hash

**Auth Endpoints:**
```
POST /api/v1/users/register
POST /api/v1/users/login
POST /api/v1/users/refresh
POST /api/v1/users/google-login
GET  /api/v1/users/profile
GET  /api/v1/quantities/history/my
```

**Redis Cache Keys:**
```
QM:qm:all              → All measurements (5 min TTL)
QM:qm:op:{OPERATION}   → By operation type
QM:qm:cat:{CATEGORY}   → By category
QM:qm:user:{userId}    → By user
```

---

## Architecture Overview
```
Frontend (HTML/CSS/JS)
        ↓
QuantityMeasurement.WebAPI (ASP.NET Core)
        ↓
QuantityMeasurement.Service (Business Logic + Auth + Encryption)
        ↓
QuantityMeasurement.Repository (EF Core + Redis + ADO.NET)
        ↓
MS SQL Server + Redis Cache
```

## Data Flow (UC17–UC18)
```
HTTP Request
    ↓
JWT Middleware (Auth)
    ↓
Controller (QuantityMeasurementController / UserController)
    ↓
Service (QuantityMeasurementServiceImpl / AuthService)
    ↓
Repository (EFQuantityMeasurementRepository)
    ↓
Redis Cache → HIT: return cached data
           → MISS: query SQL Server → cache result
```

---

## Database Schema
```sql
QuantityMeasurementDB
├── QuantityMeasurementEntity   ← Operations history with UserId
├── Users                       ← User accounts with BCrypt hash
└── __EFMigrationsHistory       ← EF Core migration tracking
```

---

## Technologies

| Technology | Purpose |
|-----------|---------|
| C# / .NET 10 | Core language and runtime |
| ASP.NET Core | REST API framework |
| Entity Framework Core | ORM for SQL Server |
| MS SQL Server | Primary database |
| ADO.NET | Low-level DB access (UC16) |
| Redis (StackExchange) | Distributed caching |
| BCrypt.Net | Password hashing + salting |
| JWT (HMAC-SHA256) | Authentication tokens |
| AES-256-GCM | Encryption/Decryption |
| Google OAuth 2.0 | Social login |
| NLog | Structured logging |
| MSTest + Moq | Unit testing |
| Swagger / OpenAPI | API documentation |
| Git + GitHub | UC-wise branch strategy |

---

## How to Run

### Prerequisites
- .NET 10 SDK
- MS SQL Server Express (SSMS)
- Redis (localhost:6379)

### Backend API
```bash
cd QuantityMeasurement.WebAPI
dotnet run
```
Swagger UI → `http://localhost:5092/swagger`

### Console App
```bash
cd QuantityMeasurementApp
dotnet run
```

### Unit Tests
```bash
dotnet test
```

### Update Database
```bash
dotnet ef database update --project QuantityMeasurement.Repository --startup-project QuantityMeasurement.WebAPI
```

---

## Testing

- **Framework:** MSTest + Moq
- **Total Tests:** 242+
- **Coverage:** UC1–UC18 including API integration tests
- Equality, conversion, arithmetic accuracy
- Cross-category type safety
- Database CRUD operations
- JWT token generation and validation
- Redis cache HIT/MISS scenarios
- BCrypt hash verification

---

## Author
**Priyanshi Yadav**