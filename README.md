Quantity Measurement App
Description

Quantity Measurement App is a C# Console Application used to compare and perform operations on different length measurements like Feet, Inches, Yards, and Centimeters.

The project is implemented using:

Object-Oriented Programming (OOP)

Service Layer Architecture

Refactoring Principles (Single Responsibility Principle)

MSTest Unit Testing

UC-wise feature branch development

Each User Case (UC) is implemented in separate feature branches as per requirement.

UC1 – Feet Measurement Equality
Objective

Ability to compare two length values in Feet and check whether they are equal.

Implementation Details (UC1)

Created Feet model class

Stored value using private readonly variable (Encapsulation)

Overrode Equals() method for value-based comparison

Overrode GetHashCode() method

Created QuantityMeasurementService for comparison logic

Added MSTest unit test cases for Feet equality

Test Cases Covered (UC1)

Same values should return True

Different values should return False

Null comparison should return False

Same reference should return True

Different object type should return False

All UC1 test cases are passing successfully.

UC2 – Inches Measurement Equality
Objective

Ability to compare two length values in Inches and check whether they are equal.

Implementation Details (UC2)

Created Inches model class

Implemented Equals() method for Inches comparison

Extended QuantityMeasurementService for Inches equality logic

Followed Service Layer Architecture (Separation of Concerns)

Added MSTest unit test cases for Inches equality

Test Cases Covered (UC2)

Same inch values should return True

Different inch values should return False

Null inch comparison should return False

Same reference should return True

Different object type should return False

All UC2 test cases are passing successfully.

UC3 – Generic Quantity Length Equality (Feet & Inches)
Objective

Ability to compare two length measurements with different units (Feet and Inches) using a generic Quantity model with unit conversion.

Implementation Details (UC3)

Created Quantity model class with:

Value

LengthUnit

Created LengthUnit enum (Feet, Inches)

Implemented generic equality using overridden Equals() method

Added internal unit conversion logic (Feet ↔ Inches)

Refactored service to use DRY principle (single comparison method)

Updated Console Menu to support Generic Quantity comparison

Reused existing Service Layer instead of duplicating logic

Added MSTest unit test cases for generic quantity comparison

Test Cases Covered (UC3)

1 Feet == 12 Inches should return True

Different unit unequal values should return False

Same unit same value should return True

Null quantity comparison should return False

Cross-unit comparison using conversion should work correctly

All UC3 test cases are passing successfully.

UC4 – Extended Length Equality (Yards & Centimeters)
Objective

Ability to compare length measurements across multiple units including Feet, Inches, Yards, and Centimeters using a generic Quantity model with full unit conversion support.

Implementation Details (UC4)

Extended LengthUnit enum to include:

YARDS

CENTIMETERS

Updated Quantity model to support additional conversion factors

Implemented conversion logic:

1 Yard = 3 Feet

1 Yard = 36 Inches

1 Inch = 2.54 Centimeters

Ensured equality works across all unit combinations

Maintained DRY principle by keeping conversion logic centralized

No duplication in service layer

Added comprehensive MSTest unit test cases for new units

Ensured Reflexive, Symmetric, and Transitive properties are validated

Test Cases Covered (UC4)

Same Yard values should return True

Different Yard values should return False

1 Yard == 3 Feet should return True

1 Yard == 36 Inches should return True

1 Centimeter == 0.393701 Inches should return True

Non-equivalent cross-unit values should return False

Transitive property validation across Yards, Feet, and Inches

Null unit handling validation

Same reference should return True

Comparison with null should return False

Complex multi-unit equality scenarios

All UC4 test cases are passing successfully.

UC5 – Unit-to-Unit Conversion
Objective

Provide ability to explicitly convert a length value from one unit to another.

Implementation Details (UC5)

Added static conversion method in Quantity:

Convert(value, sourceUnit, targetUnit)

Added instance method:

ConvertTo(targetUnit)

Implemented Base Unit normalization (Feet as base unit)

Centralized conversion logic

Added validation for:

NaN values

Infinity

Invalid unit

Maintained immutability (returns new Quantity object)

Added MSTest cases for conversion accuracy

Test Cases Covered (UC5)

Feet → Inches

Inches → Feet

Yards → Inches

Centimeters → Feet

Same unit conversion

Zero value conversion

Negative value conversion

Large value conversion

NaN should throw exception

Infinity should throw exception

All UC5 test cases are passing successfully.

UC6 – Addition of Two Length Quantities
Objective

Allow addition of two Quantity objects and return result in the unit of the first operand.

Example:
1 Foot + 12 Inches = 2 Feet

Implementation Details (UC6)

Added Add(Quantity other) method

Converted both operands to Base Unit (Feet)

Added values

Converted result back to first operand’s unit

Maintained immutability

Ensured commutative property validation

Test Cases Covered (UC6)

Same unit addition

Cross-unit addition

Adding zero

Negative value addition

Large value addition

Null operand validation

All UC6 test cases are passing successfully.

UC7 – Addition with Explicit Target Unit
Objective

Allow addition of two quantities and return result in a specified target unit.

Example:
1 Foot + 12 Inches in Yards

Implementation Details (UC7)

Added overloaded method:

Add(Quantity other, LengthUnit targetUnit)

Converted both operands to Base Unit

Added values

Converted result into explicit target unit

Maintained DRY principle

Preserved backward compatibility

Test Cases Covered (UC7)

Result in first unit

Result in second unit

Result in third unit

Zero addition with explicit unit

Negative values

Commutative property

Target unit validation

All UC7 test cases are passing successfully.

UC8 – Refactoring LengthUnit to Standalone Responsibility
Objective

Refactor architecture to follow Single Responsibility Principle by shifting conversion responsibility from Quantity to LengthUnit.

Implementation Details (UC8)

Extracted LengthUnit into standalone file

Implemented conversion logic inside LengthUnit using:

ToBase(value)

FromBase(baseValue)

Removed conversion switch logic from Quantity

Delegated conversion responsibility to enum

Simplified Quantity class

Maintained backward compatibility (UC1–UC7 still working)

No changes required in public API

All previous test cases passed without modification

Benefits Achieved

Single Responsibility Principle followed

Reduced coupling

Improved maintainability

Cleaner architecture

Easier future extension (Weight, Volume, etc.)

No circular dependency risk

All UC8 test cases and previous UC test cases are passing successfully.

Project Structure
QuantityMeasurementApp
│
├── QuantityMeasurementApp (Main Console Project)
│   ├── Models
│   │   ├── Feet.cs
│   │   ├── Inches.cs
│   │   ├── Quantity.cs
│   │   ├── LengthUnit.cs
│   │          ├──LengthUnitExtensions.cs
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
    ├── QuantityMeasurementServiceTests.cs //All UCs test case inside this
   
Features

Compare Feet values (UC1)

Compare Inches values (UC2)

Generic Quantity comparison with conversion (UC3)

Extended unit comparison (UC4)

Explicit unit conversion (UC5)

Addition of quantities (UC6)

Addition with explicit target unit (UC7)

Refactored scalable architecture (UC8)

Console-based interactive menu

Clean layered architecture (Models, Services, Menu)

Unit Testing using MSTest

DRY Principle

Encapsulation

Enum-based unit abstraction

Immutability

How to Run the Application

Open terminal inside the main project folder and run:

dotnet run
How to Run Unit Tests

From solution root folder:

dotnet test
Technologies Used

C#

.NET Console Application

MSTest Framework

Object-Oriented Programming (OOP)

Service Layer Architecture

Refactoring (SRP, DRY)

Git & GitHub (UC-wise Branching)

Branch Naming Convention
feature/UC1-FeetMeasurementEquality
feature/UC2-InchMeasurementEquality
feature/UC3-GenericQuantityLength
feature/UC4-ExtendedUnitSupport
feature/UC5-UnitToUnitConversion
feature/UC6-UnitAddition
feature/UC7-TargetUnitAddition
feature/UC8-StandaloneUnit

Each UC is developed in a separate feature branch and pushed independently as per project guidelines.

Author

Priyanshi Yadav