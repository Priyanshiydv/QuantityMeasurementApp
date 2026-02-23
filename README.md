# Quantity Measurement App

## Description

Quantity Measurement App is a C# Console Application used to compare different length measurements like Feet and Inches.
The project is implemented using Object-Oriented Programming (OOP), Service Layer Architecture, and MSTest Unit Testing following UC-wise development.

Each User Case (UC) is implemented in separate feature branches as per requirement.

---

## UC1 – Feet Measurement Equality

### Objective

Ability to compare two length values in Feet and check whether they are equal.

### Implementation Details (UC1)

* Created `Feet` model class
* Stored value using private readonly variable (Encapsulation)
* Overrode `Equals()` method for value-based comparison
* Overrode `GetHashCode()` method
* Created `QuantityMeasurementService` for comparison logic
* Added MSTest unit test cases for Feet equality

### Test Cases Covered (UC1)

* Same values should return True
* Different values should return False
* Null comparison should return False
* Same reference should return True
* Different object type should return False

All UC1 test cases are passing successfully.

---

## UC2 – Inches Measurement Equality

### Objective

Ability to compare two length values in Inches and check whether they are equal.

### Implementation Details (UC2)

* Created `Inches` model class
* Implemented `Equals()` method for Inches comparison
* Extended `QuantityMeasurementService` for Inches equality logic
* Followed Service Layer Architecture (Separation of Concerns)
* Added MSTest unit test cases for Inches equality

### Test Cases Covered (UC2)

* Same inch values should return True
* Different inch values should return False
* Null inch comparison should return False
* Same reference should return True
* Different object type should return False

All UC2 test cases are passing successfully.

---

## UC3 – Generic Quantity Length Equality (Feet & Inches)

### Objective

Ability to compare two length measurements with different units (Feet and Inches) using a generic Quantity model with unit conversion.

### Implementation Details (UC3)

* Created `Quantity` model class with Value and LengthUnit
* Created `LengthUnit` enum (Feet, Inch)
* Implemented generic equality using overridden `Equals()` method
* Added internal unit conversion logic (Feet ↔ Inches)
* Refactored service to use DRY principle (single comparison method)
* Updated Console Menu to support Generic Quantity comparison
* Reused existing Service Layer instead of duplicating logic
* Added MSTest unit test cases for generic quantity comparison

### Test Cases Covered (UC3)

* 1 Feet == 12 Inches should return True
* Different unit unequal values should return False
* Same unit same value should return True
* Null quantity comparison should return False
* Cross-unit comparison using conversion should work correctly

All UC3 test cases are passing successfully.

---
## UC4 – Extended Length Equality (Yards & Centimeters)

###Objective

Ability to compare length measurements across multiple units including Feet, Inches, Yards, and Centimeters using a generic Quantity model with full unit conversion support.

###Implementation Details (UC4)

* Extended LengthUnit enum to include:
  *YARDS
  *CENTIMETERS
* Updated Quantity model to support additional conversion factors
* Implemented conversion logic:
  *1 Yard = 3 Feet
  *1 Yard = 36 Inches
  *1 Inch = 2.54 Centimeters
* Ensured equality works across all unit combinations
* Maintained DRY principle by keeping conversion logic centralized
* No duplication in service layer
* Added comprehensive MSTest unit test cases for new units
* Ensured Reflexive, Symmetric, and Transitive properties are validated

###Test Cases Covered (UC4)

* Same Yard values should return True
* Different Yard values should return False
   *1 Yard == 3 Feet should return True
   *1 Yard == 36 Inches should return True
   *1 Centimeter == 0.393701 Inches should return True
* Non-equivalent cross-unit values should return False
* Transitive property validation across Yards, Feet, and Inches
* Null unit handling validation
* Same reference should return True
* Comparison with null should return False
* Complex multi-unit equality scenarios

All UC4 test cases are passing successfully.

---

## Project Structure

```
QuantityMeasurementApp
│
├── QuantityMeasurementApp (Main Console Project)
│   ├── Models
│   │   ├── Feet.cs
│   │   ├── Inches.cs
│   │   ├── Quantity.cs
│   │   └── LengthUnit.cs
│   │
│   ├── Services
│   │   └── QuantityMeasurementService.cs
│   │
│   ├── Menu
│   │   └── AppMenu.cs
│   │
│   └── Program.cs
│
└── QuantityMeasurementApp.Tests (MSTest Project)
    └── QuantityMeasurementServiceTests.cs
```

---

## Features

* Compare Feet values (UC1)
* Compare Inches values (UC2)
* Compare Generic Quantity (Feet & Inches) with Unit Conversion (UC3)
* Extended Generic Quantity comparison with Yards & Centimeters (UC4)
* Console-based interactive menu using Switch Case
* Clean layered architecture (Models, Services, Menu)
* Unit Testing using MSTest Framework
* OOP Concepts:

  * Encapsulation
  * Equality Comparison
  * Service Layer Pattern
  * DRY Principle
  * Enum Usage for Units

---

## How to Run the Application

Open terminal inside the main project folder and run:

```
dotnet run
```

---

## How to Run Unit Tests

Run tests from the solution root folder:

```
dotnet test
```

---

## Technologies Used

* C#
* .NET Console Application
* MSTest Framework
* Object-Oriented Programming (OOP)
* Service Layer Architecture
* Git & GitHub (UC-wise Branching)

---

## Branch Naming Convention

* `feature/UC1-FeetMeasurementEquality`
* `feature/UC2-InchMeasurementEquality`
* `feature/UC3-GenericQuantityLength`
* `feature/UC4-ExtendedUnitSupport`

Each UC is developed in a separate feature branch and pushed independently as per project guidelines.

---

## Author

Priyanshi Yadav
