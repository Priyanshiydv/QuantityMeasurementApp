# Quantity Measurement Application

## Project Overview
The **Quantity Measurement Application** is a C# console-based project designed to compare, convert, and perform arithmetic operations on different measurement units.

**Supported Categories:**
- **Length:** Feet, Inches, Yards, Centimeters
- **Weight:** Kilograms, Grams, Pounds
- **Volume:** Liters, Milliliters, Gallons
- **Temperature:** Celsius, Fahrenheit, Kelvin

---

## Key Features
- Type-safe operations via generics  
- Unit conversion across all categories  
- Arithmetic operations with validation and selective support  
- Scalable architecture for new measurement types  
- Follows SOLID principles (DRY, SRP, Interface Segregation)  
- Modular UC-wise branch development for maintainability  

---

## Project Structure


```text
=======
# Key Features:

Type-safe operations via generics

Unit conversion across all categories

Arithmetic operations with validation and selective support

Scalable architecture for new measurement types

Follows SOLID principles (DRY, SRP, Interface Segregation)

Modular UC-wise branch development for maintainability

Project Structure

QuantityMeasurementApp
├── QuantityMeasurementApp (Main Application)
│   ├── Interface
│   │   └── IQuantityMeasurementService.cs
│   ├── Menu
│   │   └── AppMenu.cs
│   ├── Models
│   │   ├── Feet.cs
│   │   ├── Inches.cs
│   │   ├── LengthUnit.cs
│   │   ├── WeightUnit.cs
│   │   ├── VolumeUnit.cs
│   │   ├── TemperatureUnit.cs
│   │   ├── TemperatureQuantity.cs
│   │   ├── TemperatureUnitWrapper.cs
│   │   ├── Quantity.cs
│   │   ├── QuantityGeneric.cs
│   │   ├── QuantityWeight.cs
│   │   └── SupportsArithmetic.cs
│   ├── Services
│   │   └── QuantityMeasurementService.cs
│   └── Program.cs
├── QuantityMeasurementApp.Tests (Unit Tests)

     └── QuantityMeasurementServiceTests.cs
        └── All UC1–UC14 test cases
=======
    └── QuantityMeasurementServiceTests.cs
        └── All UC1–UC14 test cases


Branches and Features (UC1–UC14)

UC1: feature/UC1-FeetMeasurementEquality – Compare equality between Feet quantities

UC2: feature/UC2-InchMeasurementEquality – Compare equality between Inch quantities

UC3: feature/UC3-GenericQuantityLength – Compare generic Length quantities (Feet & Inch)

UC4: feature/UC4-ExtendedUnitSupport – Extend length comparison to Yards & Centimeters

UC5: feature/UC5-UnitToUnitConversion – Conversion between any supported length units

UC6: feature/UC6-UnitAddition – Add two quantities in the first operand unit

UC7: feature/UC7-TargetUnitAddition – Add two quantities and return result in target unit

UC8: feature/UC8-StandaloneUnit – Refactor unit enums for SRP, maintain conversion logic

UC9: feature/UC9-WeightMeasurementSupport – Add Weight measurements: Kilogram, Gram, Pound

UC10: feature/UC10-GenericMeasurementRefactor – Introduce generic class Quantity for multi-category support

UC11: feature/UC11-VolumeMeasurementSupport – Add Volume measurements: Liters, Milliliters, Gallons

UC12: feature/UC12-QuantitySubtractionDivision – Add subtraction/division support for length, weight, and volume; temperature restricted

UC13: feature/UC13-ArithmeticValidation – Centralized arithmetic validation logic for all categories

UC14: feature/UC14-TemperatureMeasurementSupport – Add Temperature measurements: Celsius, Fahrenheit, Kelvin with selective arithmetic

Use Case Details
Length (UC1–UC8)

Equality and conversion between Feet, Inches, Yards, Centimeters

Supports addition, subtraction, and cross-unit conversions

Weight (UC9)

Equality, conversion, addition for Kilograms, Grams, Pounds

Prevents cross-category comparison with Length

Generic Refactor (UC10)

Introduces Quantity generic class

Works for Length, Weight, Volume

Enforces type safety

Volume (UC11)

Supports Liters, Milliliters, Gallons

Equality, conversion, and arithmetic validated

Base unit: Liters

Arithmetic Validation (UC12–UC13)

Centralized validation for addition, subtraction, division

Quantity class checks for operation support

Temperature excluded from unsupported operations

Temperature (UC14)

Supports Celsius, Fahrenheit, Kelvin

Equality & conversion allowed

Addition/subtraction restricted to temperature differences

Unsupported operations throw descriptive exceptions

IMeasurable Interface Refactor

Functional interface for arithmetic support

Default methods for selective operation validation

Testing

Framework: MSTest

Covers all UCs including:

Equality & conversion accuracy

Addition, subtraction, division for supported units

Unsupported operation handling (temperature)

Cross-category type safety

Edge cases and rounding precision

How to Run
cd QuantityMeasurementApp
dotnet run
Run Unit Tests
dotnet test
Technologies

C# | .NET 7+

Console Application

MSTest

Git & GitHub (UC-wise branch strategy)

Author

Priyanshi Yadav
