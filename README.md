# QuantityMeasurementApp



\# Quantity Measurement App



--------------------------------------------------



\# UC1 – Compare Two Lengths in Feet



\# Objective:

Ability to compare two lengths in Feet and check whether they are equal.



--------------------------------------------------



\# Project Structure:



QuantityMeasurementApp

&nbsp; - Models

&nbsp;     - Feet.cs

&nbsp; - Services

&nbsp;     - QuantityMeasurementService.cs

&nbsp; - Program.cs



QuantityMeasurementApp.Tests

&nbsp; - QuantityMeasurementServiceTests.cs



--------------------------------------------------



\# Implementation Details:



\- Created Feet class.

\- Stored value using private readonly variable.

\- Overrode Equals() method.

\- Overrode GetHashCode() method.

\- Compared two feet objects for equality.

\- Added unit test cases.



--------------------------------------------------



\# Test Cases Covered:



1\. Same values should return True.

2\. Different values should return False.

3\. Null comparison should return False.



All test cases are passing successfully.



--------------------------------------------------



Author:

Priyanshi Yadav

Branch: feature/UC1-FeetMeasurementEquality

