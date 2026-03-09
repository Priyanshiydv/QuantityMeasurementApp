## Quantity Measurement Application
# Project Overview

The Quantity Measurement Application is a C# console-based project designed to compare, convert, and perform arithmetic operations on different measurement units.

The system currently supports:

Length: Feet, Inches, Yards, Centimeters

Weight: Kilograms, Grams, Pounds

Volume: Liters, Milliliters, Gallons

Temperature: Celsius, Fahrenheit, Kelvin

# Key Features:

Type-safe operations via generics

Unit conversion across all categories

Arithmetic operations with validation and selective support

Scalable architecture for new measurement types

Follows SOLID principles (DRY, SRP, Interface Segregation)

Modular UC-wise branch development for maintainability

Project Structure
QuantityMeasurementApp
│
├── QuantityMeasurementApp (Main Application)
│   ├── Interface
│   │   └── IQuantityMeasurementService.cs
│   │
│   ├── Menu
│   │   └── AppMenu.cs
│   │
│   ├── Models
│   │   ├── Feet.cs
│   │   ├── Inches.cs
│   │   ├── LengthUnit.cs
│   │   ├── WeightUnit.cs
│   │   ├── VolumeUnit.cs
│   │   ├── IMeasurable.cs
│   │   ├── Quantity.cs
│   │   ├── QuantityGeneric.cs
│   │   ├── QuantityWeight.cs
│   │   ├── SupportsArithmetic.cs
│   │   ├── TemperatureQuantity.cs
│   │   ├── TemperatureUnit.cs
│   │   └── TemperatureUnitWrapper.cs
│   │
│   ├── Services
│   │   └── QuantityMeasurementService.cs
│   │
│   └── Program.cs
│
└── QuantityMeasurementApp.Tests (Unit Tests)
    ├── QuantityMeasurementServiceTests.cs //All UCs test case inside this

# Branches and Features (UC1–UC14)
Branch	Use Case	Description
feature/UC1-FeetMeasurementEquality	UC1	Compare equality between Feet quantities.
feature/UC2-InchMeasurementEquality	UC2	Compare equality between Inch quantities.
feature/UC3-GenericQuantityLength	UC3	Compare generic Length quantities (Feet & Inch).
feature/UC4-ExtendedUnitSupport	UC4	Extend length comparison to Yards & Centimeters.
feature/UC5-UnitToUnitConversion	UC5	Conversion between any supported length units.
feature/UC6-UnitAddition	UC6	Add two quantities in first operand unit.
feature/UC7-TargetUnitAddition	UC7	Add two quantities and return result in target unit.
feature/UC8-StandaloneUnit	UC8	Refactor unit enums for SRP, maintain conversion logic.
feature/UC9-WeightMeasurementSupport	UC9	Add Weight measurements: Kilogram, Gram, Pound with conversion & addition.
feature/UC10-GenericMeasurementRefactor	UC10	Introduce generic class Quantity<U> for multi-category support.
feature/UC11-VolumeMeasurementSupport	UC11	Add Volume measurements: Liters, Milliliters, Gallons.
feature/UC12-QuantitySubtractionDivision	UC12	Add subtraction/division support for length, weight, and volume; temperature restricted.
feature/UC13-ArithmeticValidation	UC13	Centralized arithmetic validation logic for all categories.
feature/UC14-TemperatureMeasurementSupport	UC14	Add Temperature measurements: Celsius, Fahrenheit, Kelvin with selective arithmetic.

#Use Case Details
Length (UC1–UC8)

Equality and conversion between Feet, Inches, Yards, Centimeters

Supports addition, subtraction, and cross-unit conversions

Weight (UC9)

Equality, conversion, addition for Kilograms, Grams, Pounds

Prevents cross-category comparison with Length

Generic Refactor (UC10)

Introduces Quantity<U> generic class

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

IMeasurable interface refactored with:

Functional interface for arithmetic support

Default methods for selective operation validation

# Testing

MSTest framework

Covers all UCs including:

Equality & conversion accuracy

Addition, subtraction, division for supported units

Unsupported operation handling (temperature)

Cross-category type safety

Edge cases and rounding precision

# How to Run
cd QuantityMeasurementApp
dotnet run
Run Unit Tests
dotnet test
Technologies

C# | .NET 7+

Console Application

MSTest

Git & GitHub (Branch strategy for UC-wise development)

# Author

Priyanshi Yadav