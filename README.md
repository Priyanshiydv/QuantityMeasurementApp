# Quantity Measurement App

# Description
Quantity Measurement App is a C# console application used to compare different length measurements like Feet and Inches.  
The project is designed using OOP concepts, Service Layer, and MSTest unit testing as per UC requirements.

# UC1 – Feet Measurement Equality

## Objective
Ability to compare two lengths in Feet and check whether they are equal.

## Implementation Details (UC1)
- Created Feet model class
- Stored value using private readonly variable
- Overrode Equals() method for value-based comparison
- Overrode GetHashCode() method
- Created QuantityMeasurementService for comparison logic
- Added MSTest unit test cases for Feet equality

## Test Cases Covered (UC1)
1. Same values should return True  
2. Different values should return False  
3. Null comparison should return False  
4. Same reference should return True  
5. Different object type should return False  

All UC1 test cases are passing successfully.

# UC2 – Inches Measurement Equality

## Objective
Ability to compare two lengths in Inches and check whether they are equal.

## Implementation Details (UC2)
- Created Inches model class
- Implemented Equals() method for Inches comparison
- Added comparison methods in QuantityMeasurementService
- Followed service layer architecture
- Added MSTest unit test cases for Inches equality

## Test Cases Covered (UC2)
1. Same inch values should return True  
2. Different inch values should return False  
3. Null inch comparison should return False  
4. Same reference should return True  

All UC2 test cases are passing successfully.

# Project Structure
QuantityMeasurementApp  
 ├── QuantityMeasurementApp (Main Project)  
 │    ├── Models  
 │    │     ├── Feet.cs  
 │    │     └── Inches.cs  
 │    ├── Services  
 │    │     └── QuantityMeasurementService.cs  
 │    ├── Menu  
 │    │     └── AppMenu.cs  
 │    └── Program.cs  
 │  
 └── QuantityMeasurementApp.Tests (Test Project)  
      └── QuantityMeasurementServiceTests.cs  

# Features
- Compare Feet values
- Compare Inches values
- Console-based menu system
- Clean folder structure (Models, Services, Menu)
- Unit Testing using MSTest
- OOP concepts (Encapsulation, Equality, Service Layer)

# How to Run the Application
Open terminal inside main project folder and run:
dotnet run

# How to Run Unit Tests
Run tests from solution root folder:
dotnet test

# Technologies Used
- C#
- .NET Console Application
- MSTest Framework
- Object Oriented Programming (OOP)
- Git & GitHub Branching (UC wise)

# Branch Naming Convention
- feature/UC1-FeetMeasurementEquality
- feature/UC2-InchMeasurementEquality
- feature/UC3-GenericQuantityLength

# Author
Priyanshi Yadav