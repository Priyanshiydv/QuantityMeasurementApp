using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;
using QuantityMeasurement.Service.Service;      // ← Quantity, LengthUnit, WeightUnit, VolumeUnit
using QuantityMeasurement.Service.Interfaces;   // ← IMeasurable

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementServiceTests
    {
        private const double EPSILON = 0.000001; //UC5
        private QuantityMeasurementService service = null!;

        private Quantity _quantity = new Quantity(0, LengthUnit.FEET);

        // runs before each test
        [TestInitialize]
        public void Setup() 
        {
            service = new QuantityMeasurementService();
        }
//=======================================UC1============================================
        // TC1: testEquality_SameValue
        [TestMethod]
        public void GivenSameFeetValues_ShouldReturnTrue()
        {
            // Arrange
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(1.0);

            // Act
            bool result = service.AreEqual(feet1, feet2);

            // Assert
            Assert.IsTrue(result);
        }

        // TC2: testEquality_DifferentValue
        [TestMethod]
        public void GivenDifferentFeetValues_ShouldReturnFalse()
        {
            // Arrange
            Feet feet1 = new Feet(1.0);
            Feet feet2 = new Feet(2.0);

            // Act
            bool result = service.AreEqual(feet1, feet2);

            // Assert
            Assert.IsFalse(result);
        }

        // TC3: testEquality_NullComparison
        [TestMethod]
        public void GivenNullValue_ShouldReturnFalse()
        {
            // Arrange
            Feet feet1 = new Feet(1.0);
            Feet? feet2 = null;

            // Act
            bool result = service.AreEqual(feet1, feet2!);

            // Assert
            Assert.IsFalse(result);
        }

        // TC4: testEquality_SameReference (Reflexive property)
        [TestMethod]
        public void GivenSameReference_ShouldReturnTrue()
        {
            // Arrange
            Feet feet1 = new Feet(1.0);

            // Act
            bool result = service.AreEqual(feet1, feet1);

            // Assert
            Assert.IsTrue(result);
        }

        // TC5: testEquality_NonNumericInput (type safety)
        [TestMethod]
        public void GivenDifferentObjectType_ShouldReturnFalse()
        {
            // Arrange
            Feet feet1 = new Feet(1.0);
            object obj = new object();

            // Act
            bool result = feet1.Equals(obj);

            // Assert
            Assert.IsFalse(result);
        }

//=======================================UC2===================================================

        // TC1: testEquality_SameValue for Inches
        [TestMethod]
        public void GivenSameInchValues_ShouldReturnTrue()
        {
            // Arrange
            Inches inch1 = new Inches(5.0);
            Inches inch2 = new Inches(5.0);

            // Act
            bool result = service.AreEqual(inch1, inch2);

            // Assert
            Assert.IsTrue(result);
        }

        // TC2: testEquality_DifferentValue for Inches
        [TestMethod]
        public void GivenDifferentInchValues_ShouldReturnFalse()
        {
            // Arrange
            Inches inch1 = new Inches(5.0);
            Inches inch2 = new Inches(10.0);

            // Act
            bool result = service.AreEqual(inch1, inch2);

            // Assert
            Assert.IsFalse(result);
        }

        // TC3: testEquality_NullComparison for Inches
        [TestMethod]
        public void GivenNullInchValue_ShouldReturnFalse()
        {
            // Arrange
            Inches inch1 = new Inches(5.0);
            Inches? inch2 = null;

            // Act
            bool result = service.AreEqual(inch1, inch2!);

            // Assert
            Assert.IsFalse(result);
        }

        // TC4: testEquality_SameReference for Inches
        [TestMethod]
        public void GivenSameInchReference_ShouldReturnTrue()
        {
            // Arrange
            Inches inch1 = new Inches(5.0);

            // Act
            bool result = service.AreEqual(inch1, inch1);

            // Assert
            Assert.IsTrue(result);
        }



//=====================================UC3=============================================

        // TC1: Same Unit Equality (Feet to Feet)
        [TestMethod]
        public void GivenSameQuantityFeetValues_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(1.0, LengthUnit.FEET);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // TC2: Same Unit Equality (Inch to Inch)
        [TestMethod]
        public void GivenSameQuantityInchValues_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(12.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // TC3: Cross Unit Equality (1 Feet == 12 Inch)
        [TestMethod]
        public void GivenEquivalentFeetAndInch_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // TC4: Cross Unit Equality (12 Inch == 1 Feet) – Symmetry
        [TestMethod]
        public void GivenEquivalentInchAndFeet_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(12.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(1.0, LengthUnit.FEET);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // TC5: Different Values Should Return False
        [TestMethod]
        public void GivenDifferentQuantityValues_ShouldReturnFalse()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(2.0, LengthUnit.FEET);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsFalse(result);
        }

        // TC6: Null Comparison
        [TestMethod]
        public void GivenNullQuantity_ShouldReturnFalse()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity? q2 = null;

            // Act
            bool result = service.AreEqual(q1, q2!);

            // Assert
            Assert.IsFalse(result);
        }

        // TC7: Same Reference (Reflexive Property)
        [TestMethod]
        public void GivenSameQuantityReference_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(5.0, LengthUnit.FEET);

            // Act
            bool result = service.AreEqual(q1, q1);

            // Assert
            Assert.IsTrue(result);
        }
//=================================UC4==============================================
        // TC1: Yard to Yard (Same Value)
        [TestMethod]
        public void GivenSameYardValues_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(1.0, LengthUnit.YARDS);

            bool result = service.AreEqual(q1, q2);

            Assert.IsTrue(result);
        }

        // TC2: Yard to Yard (Different Value)
        [TestMethod]
        public void GivenDifferentYardValues_ShouldReturnFalse()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(2.0, LengthUnit.YARDS);

            bool result = service.AreEqual(q1, q2);

            Assert.IsFalse(result);
        }

        // TC3: Yard to Feet (1 Yard = 3 Feet)
        [TestMethod]
        public void GivenYardAndEquivalentFeet_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(3.0, LengthUnit.FEET);

            bool result = service.AreEqual(q1, q2);

            Assert.IsTrue(result);
        }

        // TC4: Feet to Yard (Symmetry)
        [TestMethod]
        public void GivenFeetAndEquivalentYard_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(3.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(1.0, LengthUnit.YARDS);

            bool result = service.AreEqual(q1, q2);

            Assert.IsTrue(result);
        }

        // TC5: Yard to Inches (1 Yard = 36 Inches)
        [TestMethod]
        public void GivenYardAndEquivalentInches_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(36.0, LengthUnit.INCHES);

            bool result = service.AreEqual(q1, q2);

            Assert.IsTrue(result);
        }

        // TC6: Inches to Yard (Symmetry)
        [TestMethod]
        public void GivenInchesAndEquivalentYard_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(36.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(1.0, LengthUnit.YARDS);

            bool result = service.AreEqual(q1, q2);

            Assert.IsTrue(result);
        }

        // TC7: Yard to Feet (Non Equivalent)
        [TestMethod]
        public void GivenYardAndNonEquivalentFeet_ShouldReturnFalse()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(2.0, LengthUnit.FEET);

            bool result = service.AreEqual(q1, q2);

            Assert.IsFalse(result);
        }

        // TC8: Centimeter to Inches (1 cm = 0.393701 inch)
        [TestMethod]
        public void GivenCentimeterAndEquivalentInches_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.CENTIMETERS);
            Quantity q2 = new Quantity(0.393701, LengthUnit.INCHES);

            bool result = service.AreEqual(q1, q2);

            Assert.IsTrue(result);
        }

        // TC9: Centimeter to Feet (Non Equivalent)
        [TestMethod]
        public void GivenCentimeterAndNonEquivalentFeet_ShouldReturnFalse()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.CENTIMETERS);
            Quantity q2 = new Quantity(1.0, LengthUnit.FEET);

            bool result = service.AreEqual(q1, q2);

            Assert.IsFalse(result);
        }

        // TC10: Transitive Property (1 Yard = 3 Feet = 36 Inches)
        [TestMethod]
        public void GivenYardFeetInches_ShouldSatisfyTransitiveProperty()
        {
            Quantity yard = new Quantity(1.0, LengthUnit.YARDS);
            Quantity feet = new Quantity(3.0, LengthUnit.FEET);
            Quantity inches = new Quantity(36.0, LengthUnit.INCHES);

            Assert.IsTrue(service.AreEqual(yard, feet));
            Assert.IsTrue(service.AreEqual(feet, inches));
            Assert.IsTrue(service.AreEqual(yard, inches));
        }

        // TC11: Yard Null Comparison
        [TestMethod]
        public void GivenYardAndNull_ShouldReturnFalse()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity? q2 = null;

            bool result = service.AreEqual(q1, q2!);

            Assert.IsFalse(result);
        }

        // TC12: Yard Same Reference
        [TestMethod]
        public void GivenSameYardReference_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(2.0, LengthUnit.YARDS);

            bool result = service.AreEqual(q1, q1);

            Assert.IsTrue(result);
        }

        // TC13: Centimeter Same Reference
        [TestMethod]
        public void GivenSameCentimeterReference_ShouldReturnTrue()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.CENTIMETERS);

            bool result = service.AreEqual(q1, q1);

            Assert.IsTrue(result);
        }

        // TC14: Complex Scenario (2 Yard = 6 Feet = 72 Inches)
        [TestMethod]
        public void GivenAllUnitsComplexScenario_ShouldReturnTrue()
        {
            Quantity yard = new Quantity(2.0, LengthUnit.YARDS);
            Quantity feet = new Quantity(6.0, LengthUnit.FEET);
            Quantity inches = new Quantity(72.0, LengthUnit.INCHES);

            Assert.IsTrue(service.AreEqual(yard, feet));
            Assert.IsTrue(service.AreEqual(feet, inches));
            Assert.IsTrue(service.AreEqual(yard, inches));
        }


// ============================== UC5 ======================================

        // TC1: Feet → Inches
        [TestMethod]
        public void GivenFeet_WhenConverteDTOInches_ShouldReturnCorrectValue()
        {
            double result = _quantity.Convert(1.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(12.0, result, EPSILON);
        }

        // TC2: Inches → Feet
        [TestMethod]
        public void GivenInches_WhenConverteDTOFeet_ShouldReturnCorrectValue()
        {
            double result = _quantity.Convert(24.0, LengthUnit.INCHES, LengthUnit.FEET);
            
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC3: Yards → Inches
        [TestMethod]
        public void GivenYards_WhenConverteDTOInches_ShouldReturnCorrectValue()
        {
            double result = _quantity.Convert(1.0, LengthUnit.YARDS, LengthUnit.INCHES);
            
            Assert.AreEqual(36.0, result, EPSILON);
        }

        // TC4: Inches → Yards
        [TestMethod]
        public void GivenInches_WhenConverteDTOYards_ShouldReturnCorrectValue()
        {
            double result = _quantity.Convert(72.0, LengthUnit.INCHES, LengthUnit.YARDS);
            
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC5: Centimeter → Inches
        [TestMethod]
        public void GivenCentimeter_WhenConverteDTOInches_ShouldReturnApproxOne()
        {
            double result = _quantity.Convert(2.54, LengthUnit.CENTIMETERS, LengthUnit.INCHES);
            
            Assert.AreEqual(1.0, result, EPSILON);
        }

        // TC6: Feet → Yards
        [TestMethod]
        public void GivenFeet_WhenConverteDTOYards_ShouldReturnCorrectValue()
        {
            double result = _quantity.Convert(6.0, LengthUnit.FEET, LengthUnit.YARDS);
            
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC7: Round Trip Conversion
        [TestMethod]
        public void GivenValue_WhenConvertedBackAndForth_ShouldPreserveValue()
        {
            double original = 5.5;

            double toInches = _quantity.Convert(original, LengthUnit.FEET, LengthUnit.INCHES);
            double backToFeet = _quantity.Convert(toInches, LengthUnit.INCHES, LengthUnit.FEET);

            Assert.AreEqual(original, backToFeet, EPSILON);
        }

        // TC8: Zero Value
        [TestMethod]
        public void GivenZeroValue_WhenConverted_ShouldReturnZero()
        {
            double result = _quantity.Convert(0.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(0.0, result, EPSILON);
        }

        // TC9: Negative Value
        [TestMethod]
        public void GivenNegativeValue_WhenConverted_ShouldPreserveSign()
        {
            double result = _quantity.Convert(-1.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(-12.0, result, EPSILON);
        }

        // TC10: Same Unit Conversion
        [TestMethod]
        public void GivenSameUnit_WhenConverted_ShouldReturnSameValue()
        {
            double result = _quantity.Convert(5.0, LengthUnit.FEET, LengthUnit.FEET);
            
            Assert.AreEqual(5.0, result, EPSILON);
        }

        // TC11: Large Value
        [TestMethod]
        public void GivenLargeValue_WhenConverted_ShouldMaintainPrecision()
        {
            double result = _quantity.Convert(1000000.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(12000000.0, result, EPSILON);
        }

        // TC12: Small Value
        [TestMethod]
        public void GivenSmallValue_WhenConverted_ShouldMaintainPrecision()
        {
            double result = _quantity.Convert(0.0001, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(0.0012, result, EPSILON);
        }

        // TC13: NaN Should Throw
        [TestMethod]
        public void GivenNaNValue_WhenConverted_ShouldThrowException()
        {
            try
            {
                _quantity.Convert(double.NaN, LengthUnit.FEET, LengthUnit.INCHES);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test passes
            }
        }
            

        // TC14: Infinity Should Throw
        [TestMethod]
        public void GivenInfinityValue_WhenConverted_ShouldThrowException()
        {            
            try
            {
                _quantity.Convert(double.PositiveInfinity, LengthUnit.FEET, LengthUnit.INCHES);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test passes
            }
        }

        

        // TC15: Round Trip Multiple Units
        [TestMethod]
        public void GivenMultipleConversions_WhenRoundTrip_ShouldPreserveValue()
        {
            double original = 2.0;

            double toFeet = _quantity.Convert(original, LengthUnit.YARDS, LengthUnit.FEET);
            double toCm = _quantity.Convert(toFeet, LengthUnit.FEET, LengthUnit.CENTIMETERS);
            double backToYard = _quantity.Convert(toCm, LengthUnit.CENTIMETERS, LengthUnit.YARDS);

            Assert.AreEqual(original, backToYard, EPSILON);
        }

//===================================UC6===============================================

        // TC1: Same Unit Addition (Feet + Feet)
        [TestMethod]
        public void GivenFeetPlusFeet_WhenAdded_ShouldReturnSumInFeet()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(2.0, LengthUnit.FEET);

            // Act
            Quantity result = service.Add(q1, q2);

            // Assert
            Quantity expected = new Quantity(3.0, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC2: Same Unit Addition (Inches + Inches)
        [TestMethod]
        public void GivenInchPlusInch_WhenAdded_ShouldReturnSumInInches()
        {
            Quantity q1 = new Quantity(6.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(6.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(12.0, LengthUnit.INCHES);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC3: Cross Unit (Feet + Inches) → Result in Feet (first operand unit)
        [TestMethod]
        public void GivenFeetAndInches_WhenAdded_ShouldReturnResultInFeet()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(2.0, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC4: Cross Unit (Inches + Feet) → Result in Inches
        [TestMethod]
        public void GivenInchesAndFeet_WhenAdded_ShouldReturnResultInInches()
        {
            Quantity q1 = new Quantity(12.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(1.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(24.0, LengthUnit.INCHES);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC5: Yard + Feet (1 Yard + 3 Feet = 2 Yards)
        [TestMethod]
        public void GivenYardAndFeet_WhenAdded_ShouldReturnCorrectYardValue()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(3.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(2.0, LengthUnit.YARDS);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC6: Centimeter + Inch (Precision Test)
        [TestMethod]
        public void GivenCentimeterAndInch_WhenAdded_ShouldReturnAccurateCentimeter()
        {
            Quantity q1 = new Quantity(2.54, LengthUnit.CENTIMETERS);
            Quantity q2 = new Quantity(1.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(5.08, LengthUnit.CENTIMETERS);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC7: Commutativity (A + B == B + A)
        [TestMethod]
        public void GivenTwoQuantities_WhenOrderChanged_ShouldStillBeEqual()
        {
            Quantity feet = new Quantity(1.0, LengthUnit.FEET);
            Quantity inches = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result1 = service.Add(feet, inches);
            Quantity result2 = service.Add(inches, feet);

            // Convert both to feet for fair comparison
            Assert.IsTrue(result1.Equals(result2));
        }

        // TC8: Identity Element (Adding Zero)
        [TestMethod]
        public void GivenValueAndZero_WhenAdded_ShouldReturnSameValue()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(0.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(5.0, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC9: Negative Values Addition
        [TestMethod]
        public void GivenNegativeValue_WhenAdded_ShouldReturnCorrectResult()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(-2.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(3.0, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC10: Null Second Operand Should Throw Exception
        [TestMethod]
        public void testAddition_NullSecondOperand()
        {
            // Arrange
            Quantity first = new Quantity(1.0, LengthUnit.FEET);

            // Act + Assert (MSTest v4 compatible)
            Assert.Throws<ArgumentNullException>(() =>
            {
                service.Add(first, null);
            });
        }
        // TC11: Large Values Addition
        [TestMethod]
        public void GivenLargeValues_WhenAdded_ShouldMaintainPrecision()
        {
            Quantity q1 = new Quantity(1e6, LengthUnit.FEET);
            Quantity q2 = new Quantity(1e6, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(2e6, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC12: Small Values Addition
        [TestMethod]
        public void GivenSmallValues_WhenAdded_ShouldMaintainPrecision()
        {
            Quantity q1 = new Quantity(0.001, LengthUnit.FEET);
            Quantity q2 = new Quantity(0.002, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2);

            Quantity expected = new Quantity(0.003, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

//======================================UC7===========================================
        // TC1: Explicit Target Unit - Feet
        [TestMethod]
        public void GivenFeetAndInches_WhenAddedWithTargetFeet_ShouldReturnFeet()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2, LengthUnit.FEET);

            Quantity expected = new Quantity(2.0, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC2: Explicit Target Unit - Inches
        [TestMethod]
        public void GivenFeetAndInches_WhenAddedWithTargetInches_ShouldReturnInches()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2, LengthUnit.INCHES);

            Quantity expected = new Quantity(24.0, LengthUnit.INCHES);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC3: Explicit Target Unit - Yards
        [TestMethod]
        public void GivenFeetAndInches_WhenAddedWithTargetYards_ShouldReturnYards()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2, LengthUnit.YARDS);

            Quantity expected = new Quantity(0.6667, LengthUnit.YARDS);
            Assert.IsTrue(Math.Abs(result.Value - expected.Value) < 0.001);
        }

        // TC4: Explicit Target Unit - Centimeters
        [TestMethod]
        public void GivenInchesAndInches_WhenAddedWithTargetCentimeters_ShouldReturnCentimeters()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(1.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2, LengthUnit.CENTIMETERS);

            Quantity expected = new Quantity(5.08, LengthUnit.CENTIMETERS);
            Assert.IsTrue(Math.Abs(result.Value - expected.Value) < 0.001);
        }

        // TC5: Target Same As First Operand
        [TestMethod]
        public void GivenYardAndFeet_WhenAddedWithTargetYards_ShouldReturnYards()
        {
            Quantity q1 = new Quantity(2.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(3.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2, LengthUnit.YARDS);

            Quantity expected = new Quantity(3.0, LengthUnit.YARDS);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC6: Target Same As Second Operand
        [TestMethod]
        public void GivenYardAndFeet_WhenAddedWithTargetFeet_ShouldReturnFeet()
        {
            Quantity q1 = new Quantity(2.0, LengthUnit.YARDS);
            Quantity q2 = new Quantity(3.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2, LengthUnit.FEET);

            Quantity expected = new Quantity(9.0, LengthUnit.FEET);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC7: Commutativity with Explicit Target Unit
        [TestMethod]
        public void GivenTwoQuantities_WhenOrderChangedWithSameTarget_ShouldBeEqual()
        {
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result1 = service.Add(q1, q2, LengthUnit.YARDS);
            Quantity result2 = service.Add(q2, q1, LengthUnit.YARDS);

            Assert.IsTrue(Math.Abs(result1.Value - result2.Value) < 0.001);
        }

        // TC8: Zero Value with Explicit Target Unit
        [TestMethod]
        public void GivenValueAndZero_WhenAddedWithTargetYards_ShouldReturnConvertedValue()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(0.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2, LengthUnit.YARDS);

            Quantity expected = new Quantity(1.6667, LengthUnit.YARDS);
            Assert.IsTrue(Math.Abs(result.Value - expected.Value) < 0.001);
        }

        // TC9: Negative Values with Explicit Target
        [TestMethod]
        public void GivenNegativeValue_WhenAddedWithTargetInches_ShouldReturnCorrectResult()
        {
            Quantity q1 = new Quantity(5.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(-2.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2, LengthUnit.INCHES);

            Quantity expected = new Quantity(36.0, LengthUnit.INCHES);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC10: Null Second Operand Should Throw Exception
        [TestMethod]
        public void GivenNullTargetUnit_WhenAdded_ShouldThrowArgumentException()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            // Act + Assert (MSTest v4 syntax)
            Assert.Throws<ArgumentException>(() =>
            {
                // Passing invalid target (simulate null case equivalent)
                service.Add(q1, q2, null!);
            });
        }

        // TC11: Large Values Converted to Smaller Unit
        [TestMethod]
        public void GivenLargeValues_WhenAddedWithTargetInches_ShouldMaintainPrecision()
        {
            Quantity q1 = new Quantity(1000.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(500.0, LengthUnit.FEET);

            Quantity result = service.Add(q1, q2, LengthUnit.INCHES);

            Quantity expected = new Quantity(18000.0, LengthUnit.INCHES);
            Assert.IsTrue(result.Equals(expected));
        }

        // TC12: Small To Large Scale Conversion
        [TestMethod]
        public void GivenInches_WhenAddedWithTargetYards_ShouldConvertCorrectly()
        {
            Quantity q1 = new Quantity(12.0, LengthUnit.INCHES);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            Quantity result = service.Add(q1, q2, LengthUnit.YARDS);

            Quantity expected = new Quantity(0.6667, LengthUnit.YARDS);
            Assert.IsTrue(Math.Abs(result.Value - expected.Value) < 0.001);
        }

        // TC13: Invalid Target Unit Should Throw Exception
        [TestMethod]
        public void GivenInvalidTargetUnit_WhenAdded_ShouldThrowArgumentException()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);

            // Act + Assert
            Assert.Throws<ArgumentException>(() =>
            {
                service.Add(q1, q2, null!);
            });
        }


        // TC14: Precision Tolerance Test (Multiple Explicit Target Conversions)
        [TestMethod]
        public void testAddition_ExplicitTargetUnit_PrecisionTolerance()
        {
            double epsilon = 0.001;

            // Case 1: 1 Feet + 12 Inches → Yards
            Quantity q1 = new Quantity(1.0, LengthUnit.FEET);
            Quantity q2 = new Quantity(12.0, LengthUnit.INCHES);
            Quantity result1 = service.Add(q1, q2, LengthUnit.YARDS);

            double expected1 = 0.6667;
            Assert.IsTrue(Math.Abs(result1.Value - expected1) < epsilon);


            // Case 2: 2.54 cm + 1 inch → Feet (2 inches total)
            Quantity q3 = new Quantity(2.54, LengthUnit.CENTIMETERS);
            Quantity q4 = new Quantity(1.0, LengthUnit.INCHES);
            Quantity result2 = service.Add(q3, q4, LengthUnit.FEET);

            double expected2 = 0.1667; // 2 inches = 0.1667 feet
            Assert.IsTrue(Math.Abs(result2.Value - expected2) < epsilon);


            // Case 3: 1 Yard + 3 Feet → Inches (6 feet total)
            Quantity q5 = new Quantity(1.0, LengthUnit.YARDS);
            Quantity q6 = new Quantity(3.0, LengthUnit.FEET);
            Quantity result3 = service.Add(q5, q6, LengthUnit.INCHES);

            double expected3 = 72.0; // 6 feet = 72 inches
            Assert.IsTrue(Math.Abs(result3.Value - expected3) < epsilon);
        }

// ===============================UC8=======================================

        // TC1: FEET to Base should return same value
        [TestMethod]
        public void GivenFeet_WhenConverteDTOBase_ShouldReturnSameValue()
        {
            double result = LengthUnit.FEET.ToBase(5.0);

            Assert.AreEqual(5.0, result, EPSILON);
        }

        // TC2: INCHES to Base conversion
        [TestMethod]
        public void GivenInches_WhenConverteDTOBase_ShouldReturnFeetValue()
        {
            double result = LengthUnit.INCHES.ToBase(12.0);

            Assert.AreEqual(1.0, result, EPSILON);
        }

        // TC3: YARDS to Base conversion
        [TestMethod]
        public void GivenYards_WhenConverteDTOBase_ShouldReturnFeetValue()
        {
            double result = LengthUnit.YARDS.ToBase(2.0);

            Assert.AreEqual(6.0, result, EPSILON);
        }

        // TC4: CENTIMETERS to Base conversion
        [TestMethod]
        public void GivenCentimeters_WhenConverteDTOBase_ShouldReturnFeetValue()
        {
            double result = LengthUnit.CENTIMETERS.ToBase(30.48);

            Assert.AreEqual(1.0, result, EPSILON);
        }

        // TC5: Base to INCHES conversion
        [TestMethod]
        public void GivenBaseValue_WhenConverteDTOInches_ShouldReturnCorrectValue()
        {
            double result = LengthUnit.INCHES.FromBase(1.0);

            Assert.AreEqual(12.0, result, EPSILON);
        }

        // TC6: Base to YARDS conversion
        [TestMethod]
        public void GivenBaseValue_WhenConverteDTOYards_ShouldReturnCorrectValue()
        {
            double result = LengthUnit.YARDS.FromBase(6.0);

            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC7: Base to CENTIMETERS conversion
        [TestMethod]
        public void GivenBaseValue_WhenConverteDTOCentimeters_ShouldReturnCorrectValue()
        {
            double result = LengthUnit.CENTIMETERS.FromBase(1.0);

            Assert.AreEqual(30.48, result, EPSILON);
        }

        // TC8: Invalid numeric value should throw exception (ToBase)
        [TestMethod]
        public void GivenInvalidValue_WhenConverteDTOBase_ShouldThrowException()
        {
            try
            {
                LengthUnit.FEET.ToBase(double.NaN);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                
            }
        }

        // TC9: Invalid numeric value should throw exception (FromBase)
        [TestMethod]
        public void GivenInvalidBaseValue_WhenConvertedFromBase_ShouldThrowException()
        {
            try
            {
                LengthUnit.FEET.FromBase(double.PositiveInfinity);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                
            }
        }

//=======================================UC9===============================================
        // ====================== EQUALITY TESTS ======================

        // TC1: Same Reference Equality (Reflexive Property)
        [TestMethod]
        public void TC1_GivenWeight_WhenComparedWithItself_ShouldReturnTrue()
        {
            QuantityWeight w = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsTrue(w.Equals(w));
        }

        // TC2: Kilogram to Kilogram Equality (Same Value)
        [TestMethod]
        public void TC2_GivenKilogram_WhenSameValue_ShouldReturnTrue()
        {
            QuantityWeight w1 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        // TC3: Kilogram to Kilogram Equality (Different Value)
        [TestMethod]
        public void TC3_GivenKilogram_WhenDifferentValue_ShouldReturnFalse()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(w1.Equals(w2));
        }

        // TC4: Kilogram to Gram Equality
        [TestMethod]
        public void TC4_GivenKilogramAndGram_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(w1.Equals(w2));
        }

        // TC5: Kilogram to Pound Equality
        [TestMethod]
        public void TC5_GivenKilogramAndPound_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.20462, WeightUnit.POUND);

            Assert.IsTrue(w1.Equals(w2));
        }

        // TC6: Gram to Pound Equality
        [TestMethod]
        public void TC6_GivenGramAndPound_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityWeight w1 = new QuantityWeight(453.592, WeightUnit.GRAM);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.POUND);

            Assert.IsTrue(w1.Equals(w2));
        }

        // TC7: Null Comparison
        [TestMethod]
        public void TC7_GivenWeight_WhenComparedWithNull_ShouldReturnFalse()
        {
            QuantityWeight w = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            Assert.IsFalse(w.Equals(null));
        }

        // TC8: Transitive Property
        [TestMethod]
        public void TC8_GivenThreeEquivalentWeights_ShouldSatisfyTransitiveProperty()
        {
            QuantityWeight a = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight b = new QuantityWeight(1000.0, WeightUnit.GRAM);
            QuantityWeight c = new QuantityWeight(2.20462, WeightUnit.POUND);

            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(b.Equals(c));
            Assert.IsTrue(a.Equals(c));
        }

        // ====================== CONVERSION TESTS ======================

        // TC9: Pound to Kilogram Conversion
        [TestMethod]
        public void TC9_GivenPound_WhenConverteDTOKilogram_ShouldReturnOne()
        {
            QuantityWeight w = new QuantityWeight(2.20462, WeightUnit.POUND);
            QuantityWeight result = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.IsTrue(Math.Abs(result.Value - 1.0) < 0.001);
        }

        // TC10: Kilogram to Pound Conversion
        [TestMethod]
        public void TC10_GivenKilogram_WhenConverteDTOPound_ShouldReturnCorrectValue()
        {
            QuantityWeight w = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight result = w.ConvertTo(WeightUnit.POUND);

            Assert.IsTrue(Math.Abs(result.Value - 2.20462) < 0.001);
        }

        // TC11: Same Unit Conversion
        [TestMethod]
        public void TC11_GivenSameUnitConversion_ShouldReturnSameValue()
        {
            QuantityWeight w = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            QuantityWeight result = w.ConvertTo(WeightUnit.KILOGRAM);

            Assert.AreEqual(5.0, result.Value);
        }

        // TC12: Round Trip Conversion
        [TestMethod]
        public void TC12_GivenRoundTripConversion_ShouldMaintainValue()
        {
            QuantityWeight w = new QuantityWeight(1.5, WeightUnit.KILOGRAM);

            QuantityWeight result = w
                .ConvertTo(WeightUnit.GRAM)
                .ConvertTo(WeightUnit.KILOGRAM);

            Assert.IsTrue(Math.Abs(result.Value - 1.5) < 0.001);
        }

        // ====================== ADDITION TESTS ======================

        // TC13: Addition Same Unit
        [TestMethod]
        public void TC13_GivenKilogramPlusKilogram_ShouldReturnSum()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(2.0, WeightUnit.KILOGRAM);

            QuantityWeight result = w1.Add(w2);

            Assert.AreEqual(3.0, result.Value);
        }

        // TC14: Addition Cross Unit (Kilogram + Gram)
        [TestMethod]
        public void TC14_GivenKilogramPlusGram_ShouldReturnKilogram()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            QuantityWeight result = w1.Add(w2);

            Assert.IsTrue(result.Equals(new QuantityWeight(2.0, WeightUnit.KILOGRAM)));
        }

        // TC15: Addition Cross Unit (Pound + Kilogram)
        [TestMethod]
        public void TC15_GivenPoundPlusKilogram_ShouldReturnPound()
        {
            QuantityWeight w1 = new QuantityWeight(2.20462, WeightUnit.POUND);
            QuantityWeight w2 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);

            QuantityWeight result = w1.Add(w2);

            Assert.IsTrue(Math.Abs(result.Value - 4.40924) < 0.01);
        }

        // TC16: Addition Explicit Target Unit
        [TestMethod]
        public void TC16_GivenExplicitTargetUnit_ShouldReturnInTargetUnit()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            QuantityWeight result = w1.Add(w2, WeightUnit.GRAM);

            Assert.AreEqual(2000.0, result.Value);
        }

        // TC17: Addition Commutativity
        [TestMethod]
        public void TC17_GivenAdditionOrderChanged_ShouldReturnSameResult()
        {
            QuantityWeight w1 = new QuantityWeight(1.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(1000.0, WeightUnit.GRAM);

            QuantityWeight r1 = w1.Add(w2, WeightUnit.GRAM);
            QuantityWeight r2 = w2.Add(w1, WeightUnit.GRAM);

            Assert.IsTrue(Math.Abs(r1.Value - r2.Value) < 0.001);
        }

        // TC18: Addition With Zero
        [TestMethod]
        public void TC18_GivenAdditionWithZero_ShouldReturnSameValue()
        {
            QuantityWeight w1 = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(0.0, WeightUnit.GRAM);

            QuantityWeight result = w1.Add(w2);

            Assert.AreEqual(5.0, result.Value);
        }

        // TC19: Addition With Negative Value
        [TestMethod]
        public void TC19_GivenNegativeWeightAddition_ShouldReturnCorrectResult()
        {
            QuantityWeight w1 = new QuantityWeight(5.0, WeightUnit.KILOGRAM);
            QuantityWeight w2 = new QuantityWeight(-2000.0, WeightUnit.GRAM);

            QuantityWeight result = w1.Add(w2);

            Assert.AreEqual(3.0, result.Value);
        }

        // ====================== VALIDATION TEST ======================

       
        // TC20: Invalid Numeric Value Should Throw ArgumentException
        [TestMethod]
        public void TC20_GivenInvalidNumericValue_ShouldThrowArgumentException()
        {
            try
            {
                QuantityWeight w = new QuantityWeight(double.NaN, WeightUnit.KILOGRAM);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            
            }
        }

//=======================================UC10===============================================
        //======================= GENERIC EQUALITY TESTS ======================

        // TC1: Length Equality (Feet vs Inches)
        [TestMethod]
        public void TC1_GivenFeetAndInches_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(1.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(12.0, LengthUnit.INCHES);

            Assert.IsTrue(q1.Equals(q2));
        }

        // TC2: Weight Equality (Kilogram vs Gram)
        [TestMethod]
        public void TC2_GivenKilogramAndGram_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityGeneric<WeightUnit> q1 = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            QuantityGeneric<WeightUnit> q2 = new QuantityGeneric<WeightUnit>(1000.0, WeightUnit.GRAM);

            Assert.IsTrue(q1.Equals(q2));
        }

        // TC3: Cross Category Comparison (Length vs Weight)
        [TestMethod]
        public void TC3_GivenLengthAndWeight_WhenCompared_ShouldReturnFalse()
        {
            QuantityGeneric<LengthUnit> length = new QuantityGeneric<LengthUnit>(1.0, LengthUnit.FEET);
            QuantityGeneric<WeightUnit> weight = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(length.Equals(weight));
        }

        // TC4: Null Comparison
        [TestMethod]
        public void TC4_GivenQuantity_WhenComparedWithNull_ShouldReturnFalse()
        {
            QuantityGeneric<WeightUnit> q = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(q.Equals(null));
        }

        //====================== GENERIC CONVERSION TESTS ======================
        // TC5: Length Conversion (Feet to Inch)
        [TestMethod]
        public void TC5_GivenFeet_WhenConverteDTOInch_ShouldReturnTwelve()
        {
            QuantityGeneric<LengthUnit> q = new QuantityGeneric<LengthUnit>(1.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> result = q.ConvertTo(LengthUnit.INCHES);

            Assert.IsTrue(Math.Abs(result.Value - 12.0) < 0.001);
        }

        // TC6: Weight Conversion (Kilogram to Gram)
        [TestMethod]
        public void TC6_GivenKilogram_WhenConverteDTOGram_ShouldReturnThousand()
        {
            QuantityGeneric<WeightUnit> q = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            QuantityGeneric<WeightUnit> result = q.ConvertTo(WeightUnit.GRAM);

            Assert.AreEqual(1000.0, result.Value);
        }

        // TC7: Round Trip Conversion
        [TestMethod]
        public void TC7_GivenRoundTripConversion_ShouldMaintainValue()
        {
            QuantityGeneric<LengthUnit> q = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> result = q
                .ConvertTo(LengthUnit.INCHES)
                .ConvertTo(LengthUnit.FEET);

            Assert.IsTrue(Math.Abs(result.Value - 5.0) < 0.001);
        }
        //====================== GENERIC ADDITION TESTS ======================
        // TC8: Length Addition (Feet + Inch)
        [TestMethod]
        public void TC8_GivenFeetPlusInch_ShouldReturnCorrectResult()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(1.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(12.0, LengthUnit.INCHES);

            QuantityGeneric<LengthUnit> result = q1.Add(q2, LengthUnit.FEET);

            Assert.IsTrue(result.Equals(new QuantityGeneric<LengthUnit>(2.0, LengthUnit.FEET)));
        }

        // TC9: Weight Addition (Kilogram + Gram)
        [TestMethod]
        public void TC9_GivenKilogramPlusGram_ShouldReturnCorrectResult()
        {
            QuantityGeneric<WeightUnit> q1 = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            QuantityGeneric<WeightUnit> q2 = new QuantityGeneric<WeightUnit>(1000.0, WeightUnit.GRAM);

            QuantityGeneric<WeightUnit> result = q1.Add(q2, WeightUnit.KILOGRAM);

            Assert.IsTrue(result.Equals(new QuantityGeneric<WeightUnit>(2.0, WeightUnit.KILOGRAM)));
        }

       //====================== CONSTRUCTOR VALIDATION TESTS ======================
       // TC10: Null Unit Should Throw ArgumentException
        [TestMethod]
        public void TC10_GivenNullUnit_WhenCreatingQuantity_ShouldThrowArgumentException()
        {
            try
            {
                QuantityGeneric<LengthUnit> q = new QuantityGeneric<LengthUnit>(1.0, null!);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test Passed
            }
        }

        // TC11: Invalid Numeric Value (NaN) Should Throw ArgumentException
        [TestMethod]
        public void TC11_GivenInvalidNumericValue_ShouldThrowArgumentException()
        {
            try
            {
                QuantityGeneric<WeightUnit> q =
                    new QuantityGeneric<WeightUnit>(double.NaN, WeightUnit.KILOGRAM);

                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
                // Test Passed
            }
        }

        //====================== HASHCODE & EQUALITY CONTRACT ======================
        // TC12: Equal Objects Should Have Same HashCode
        [TestMethod]
        public void TC12_GivenEqualQuantities_ShouldHaveSameHashCode()
        {
            QuantityGeneric<WeightUnit> q1 = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);
            QuantityGeneric<WeightUnit> q2 = new QuantityGeneric<WeightUnit>(1000.0, WeightUnit.GRAM);

            Assert.AreEqual(q1.GetHashCode(), q2.GetHashCode());
        }

        // TC13: Reflexive Property
        [TestMethod]
        public void TC13_GivenQuantity_WhenComparedWithItself_ShouldReturnTrue()
        {
            QuantityGeneric<LengthUnit> q = new QuantityGeneric<LengthUnit>(2.0, LengthUnit.FEET);

            Assert.IsTrue(q.Equals(q));
        }

//=======================================UC11===============================================
        // ====================== VOLUME EQUALITY TESTS ======================
        // TC1: Litre to Litre Equality
        [TestMethod]
        public void TC1_GivenLitre_WhenSameValue_ShouldReturnTrue()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        // TC2: Litre to Litre Different Value
        [TestMethod]
        public void TC2_GivenLitre_WhenDifferentValue_ShouldReturnFalse()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(2.0, VolumeUnit.LITRE);

            Assert.IsFalse(v1.Equals(v2));
        }

        // TC3: Litre to Millilitre Equality
        [TestMethod]
        public void TC3_GivenLitreAndMillilitre_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Assert.IsTrue(v1.Equals(v2));
        }

        // TC4: Litre to Gallon Equality
        [TestMethod]
        public void TC4_GivenLitreAndGallon_WhenEquivalent_ShouldReturnTrue()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(0.264172, VolumeUnit.GALLON);

            Assert.IsTrue(Math.Abs(v1.ConvertTo(VolumeUnit.GALLON).Value - v2.Value) < 0.01);
        }

        // TC5: Null Comparison
        [TestMethod]
        public void TC5_GivenVolume_WhenComparedWithNull_ShouldReturnFalse()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);

            Assert.IsFalse(v.Equals(null));
        }

        // TC6: Same Reference Equality
        [TestMethod]
        public void TC6_GivenVolume_WhenComparedWithItself_ShouldReturnTrue()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(2.0, VolumeUnit.LITRE);

            Assert.IsTrue(v.Equals(v));
        }
        // ====================== VOLUME CONVERSION TESTS ======================

        // TC7: Litre to Millilitre
        [TestMethod]
        public void TC7_GivenLitre_WhenConverteDTOMillilitre_ShouldReturnThousand()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);

            QuantityGeneric<VolumeUnit> result = v.ConvertTo(VolumeUnit.MILLILITRE);

            Assert.AreEqual(1000.0, result.Value);
        }

        // TC8: Millilitre to Litre
        [TestMethod]
        public void TC8_GivenMillilitre_WhenConverteDTOLitre_ShouldReturnOne()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            QuantityGeneric<VolumeUnit> result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.IsTrue(Math.Abs(result.Value - 1.0) < 0.001);
        }

        // TC9: Gallon to Litre
        [TestMethod]
        public void TC9_GivenGallon_WhenConverteDTOLitre_ShouldReturnCorrectValue()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.GALLON);

            QuantityGeneric<VolumeUnit> result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.IsTrue(Math.Abs(result.Value - 3.78541) < 0.01);
        }

        // TC10: Same Unit Conversion
        [TestMethod]
        public void TC10_GivenSameUnitConversion_ShouldReturnSameValue()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(5.0, VolumeUnit.LITRE);

            QuantityGeneric<VolumeUnit> result = v.ConvertTo(VolumeUnit.LITRE);

            Assert.AreEqual(5.0, result.Value);
        }

        // TC11: Round Trip Conversion
        [TestMethod]
        public void TC11_GivenRoundTripConversion_ShouldMaintainValue()
        {
            QuantityGeneric<VolumeUnit> v = new QuantityGeneric<VolumeUnit>(1.5, VolumeUnit.LITRE);

            QuantityGeneric<VolumeUnit> result =
                v.ConvertTo(VolumeUnit.MILLILITRE)
                .ConvertTo(VolumeUnit.LITRE);

            Assert.IsTrue(Math.Abs(result.Value - 1.5) < 0.001);
        }

        // ====================== VOLUME ADDITION TESTS ======================
        // TC12: Same Unit Addition
        [TestMethod]
        public void TC12_GivenLitrePlusLitre_ShouldReturnSum()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(2.0, VolumeUnit.LITRE);

            QuantityGeneric<VolumeUnit> result = v1.Add(v2);

            Assert.AreEqual(3.0, result.Value);
        }

        // TC13: Cross Unit Addition
        [TestMethod]
        public void TC13_GivenLitrePlusMillilitre_ShouldReturnCorrectResult()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            QuantityGeneric<VolumeUnit> result = v1.Add(v2);

            Assert.IsTrue(result.Equals(new QuantityGeneric<VolumeUnit>(2.0, VolumeUnit.LITRE)));
        }

        // TC14: Explicit Target Unit
        [TestMethod]
        public void TC14_GivenExplicitTargetUnit_ShouldReturnMillilitre()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            QuantityGeneric<VolumeUnit> result = v1.Add(v2, VolumeUnit.MILLILITRE);

            Assert.AreEqual(2000.0, result.Value);
        }

        // TC15: Addition Commutativity
        [TestMethod]
        public void TC15_GivenAdditionOrderChanged_ShouldReturnSameResult()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            QuantityGeneric<VolumeUnit> r1 = v1.Add(v2, VolumeUnit.MILLILITRE);
            QuantityGeneric<VolumeUnit> r2 = v2.Add(v1, VolumeUnit.MILLILITRE);

            Assert.IsTrue(Math.Abs(r1.Value - r2.Value) < 0.001);
        }

        // TC16: Addition With Zero
        [TestMethod]
        public void TC16_GivenAdditionWithZero_ShouldReturnSameValue()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(5.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(0.0, VolumeUnit.MILLILITRE);

            QuantityGeneric<VolumeUnit> result = v1.Add(v2);

            Assert.AreEqual(5.0, result.Value);
        }

        // TC17: Negative Value Addition
        [TestMethod]
        public void TC17_GivenNegativeVolumeAddition_ShouldReturnCorrectResult()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(5.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(-2000.0, VolumeUnit.MILLILITRE);

            QuantityGeneric<VolumeUnit> result = v1.Add(v2);

            Assert.AreEqual(3.0, result.Value);
        }
        // ====================== CROSS CATEGORY TESTS ======================

        // TC18: Volume vs Length
        [TestMethod]
        public void TC18_GivenVolumeAndLength_WhenCompared_ShouldReturnFalse()
        {
            QuantityGeneric<VolumeUnit> volume = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<LengthUnit> length = new QuantityGeneric<LengthUnit>(1.0, LengthUnit.FEET);

            Assert.IsFalse(volume.Equals(length));
        }

        // TC19: Volume vs Weight
        [TestMethod]
        public void TC19_GivenVolumeAndWeight_WhenCompared_ShouldReturnFalse()
        {
            QuantityGeneric<VolumeUnit> volume = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<WeightUnit> weight = new QuantityGeneric<WeightUnit>(1.0, WeightUnit.KILOGRAM);

            Assert.IsFalse(volume.Equals(weight));
        }

        // TC20: HashCode Consistency
        [TestMethod]
        public void TC20_GivenEqualVolumes_ShouldHaveSameHashCode()
        {
            QuantityGeneric<VolumeUnit> v1 = new QuantityGeneric<VolumeUnit>(1.0, VolumeUnit.LITRE);
            QuantityGeneric<VolumeUnit> v2 = new QuantityGeneric<VolumeUnit>(1000.0, VolumeUnit.MILLILITRE);

            Assert.AreEqual(v1.GetHashCode(), v2.GetHashCode());
        }

//=======================================UC12===============================================
        // ====================== SUBTRACTION TESTS ======================

        // TC1: Same Unit Subtraction (Feet - Feet)
        [TestMethod]
        public void TC1_GivenFeetMinusFeet_ShouldReturnCorrectDifference()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.AreEqual(5.0, result.Value);
        }

        // TC2: Cross Unit Subtraction (Feet - Inches)
        [TestMethod]
        public void TC2_GivenFeetMinusInches_ShouldReturnCorrectValue()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(6.0, LengthUnit.INCHES);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.IsTrue(Math.Abs(result.Value - 9.5) < 0.01);
        }

        // TC3: Explicit Target Unit (Inches)
        [TestMethod]
        public void TC3_GivenExplicitTargetUnit_ShouldReturnInches()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(6.0, LengthUnit.INCHES);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2, LengthUnit.INCHES);

            Assert.AreEqual(114.0, result.Value);
        }

        // TC4: Subtraction Resulting in Negative Value
        [TestMethod]
        public void TC4_GivenSmallerMinusLarger_ShouldReturnNegative()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.AreEqual(-5.0, result.Value);
        }

        // TC5: Subtraction Resulting in Zero
        [TestMethod]
        public void TC5_GivenEquivalentQuantities_ShouldReturnZero()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(120.0, LengthUnit.INCHES);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.IsTrue(Math.Abs(result.Value) < 0.01);
        }

        // TC6: Subtraction With Zero Operand
        [TestMethod]
        public void TC6_GivenSubtractZero_ShouldReturnSameValue()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(0.0, LengthUnit.INCHES);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.AreEqual(5.0, result.Value);
        }

        // TC7: Subtraction Non-Commutativity
        [TestMethod]
        public void TC7_GivenOrderChanged_ShouldReturnDifferentResults()
        {
            QuantityGeneric<LengthUnit> a = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> b = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> r1 = a.Subtract(b);
            QuantityGeneric<LengthUnit> r2 = b.Subtract(a);

            Assert.AreNotEqual(r1.Value, r2.Value);
        }

        // ====================== DIVISION TESTS ======================

        // TC8: Same Unit Division
        [TestMethod]
        public void TC8_GivenFeetDividedByFeet_ShouldReturnRatio()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(2.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(5.0, result);
        }

        // TC9: Cross Unit Division
        [TestMethod]
        public void TC9_GivenInchesDividedByFeet_ShouldReturnOne()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(24.0, LengthUnit.INCHES);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(2.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.IsTrue(Math.Abs(result - 1.0) < 0.01);
        }

        // TC10: Division Result Greater Than One
        [TestMethod]
        public void TC10_GivenLargerDividedBySmaller_ShouldReturnGreaterThanOne()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(2.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(5.0, result);
        }

        // TC11: Division Result Less Than One
        [TestMethod]
        public void TC11_GivenSmallerDividedByLarger_ShouldReturnFraction()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(0.5, result);
        }

        // TC12: Division Result Equal To One
        [TestMethod]
        public void TC12_GivenEqualQuantities_ShouldReturnOne()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(1.0, result);
        }

        // ====================== ERROR HANDLING TESTS ======================

        // TC13: Division By Zero
        [TestMethod]
        public void TC13_GivenDivisionByZero_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(0.0, LengthUnit.FEET);
             try
            {
                q1.Divide(q2);
                Assert.Fail("Expected ArithmeticException was not thrown.");
            }
            catch (ArithmeticException)
            {
            }
        }

        // TC14: Subtraction With Null Operand
        [TestMethod]
        public void TC14_GivenNullOperand_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);

            try
            {
                q1.Subtract(null);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        // TC15: Cross Category Subtraction
        [TestMethod]
        public void TC15_GivenLengthAndWeightSubtraction_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> length = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<WeightUnit> weight = new QuantityGeneric<WeightUnit>(5.0, WeightUnit.KILOGRAM);
            try
            {
                length.Subtract(weight);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }
            
        // TC16: Cross Category Division
        [TestMethod]
        public void TC16_GivenLengthAndWeightDivision_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> length = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<WeightUnit> weight = new QuantityGeneric<WeightUnit>(5.0, WeightUnit.KILOGRAM);

             try
            {
                length.Divide(weight);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
        
            }
        }

//=======================================UC13===============================================
        // ====================== VALIDATION CONSISTENCY TESTS ======================

        // TC1: Add With Null Operand
        [TestMethod]
        public void TC1_GivenNullOperandInAdd_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);

            try
            {
                q1.Add(null);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        // TC2: Divide With Null Operand
        [TestMethod]
        public void TC2_GivenNullOperandInDivide_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);

            try
            {
                q1.Divide(null);
                Assert.Fail("Expected ArgumentException was not thrown.");
            }
            catch (ArgumentException)
            {
            }
        }

        // ====================== ROUNDING CONSISTENCY ======================

        // TC3: Subtraction Result Rounded To Two Decimals
        [TestMethod]
        public void TC3_GivenDecimalSubtraction_ShouldRounDTOTwoDecimals()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.25, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(5.13, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.AreEqual(5.12, result.Value);
        }

        // ====================== IMMUTABILITY TESTS ======================

        // TC4: Original Quantities Should Not Change After Add
        [TestMethod]
        public void TC4_GivenAddition_ShouldNotModifyOriginalObjects()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> result = q1.Add(q2);

            Assert.AreEqual(10.0, q1.Value);
            Assert.AreEqual(5.0, q2.Value);
        }

        // TC5: Original Quantities Should Not Change After Subtract
        [TestMethod]
        public void TC5_GivenSubtraction_ShouldNotModifyOriginalObjects()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(5.0, LengthUnit.FEET);

            QuantityGeneric<LengthUnit> result = q1.Subtract(q2);

            Assert.AreEqual(10.0, q1.Value);
            Assert.AreEqual(5.0, q2.Value);
        }

        // ====================== BEHAVIOR PRESERVATION ======================

        // TC6: UC12 Addition Behavior Still Works
        [TestMethod]
        public void TC6_GivenFeetAndInchesAddition_ShouldReturnTwoFeet()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(1.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(12.0, LengthUnit.INCHES);

            QuantityGeneric<LengthUnit> result = q1.Add(q2);

            Assert.IsTrue(Math.Abs(result.Value - 2.0) < 0.01);
        }

        // TC7: Division Should Return Raw Double (No Rounding)
        [TestMethod]
        public void TC7_GivenDivision_ShouldReturnExactRatio()
        {
            QuantityGeneric<LengthUnit> q1 = new QuantityGeneric<LengthUnit>(7.0, LengthUnit.FEET);
            QuantityGeneric<LengthUnit> q2 = new QuantityGeneric<LengthUnit>(2.0, LengthUnit.FEET);

            double result = q1.Divide(q2);

            Assert.AreEqual(3.5, result);
        }

        // ====================== CROSS CATEGORY CONSISTENCY ======================

        // TC8: Cross Category Add Should Throw Exception
        [TestMethod]
        public void TC8_GivenLengthAndWeightAddition_ShouldThrowException()
        {
            QuantityGeneric<LengthUnit> length = new QuantityGeneric<LengthUnit>(10.0, LengthUnit.FEET);
            QuantityGeneric<WeightUnit> weight = new QuantityGeneric<WeightUnit>(5.0, WeightUnit.KILOGRAM);

            try
            {
                length.Add((QuantityGeneric<LengthUnit>)(object)weight);
                Assert.Fail("Expected exception not thrown");
            }
            catch (InvalidCastException)
            {
                
            }
        }

//=======================================UC14===============================================
        // ====================== TEMPERATURE TESTS ======================
            // ---------------- EQUALITY TESTS ----------------

            // TC1: Celsius-to-Celsius Equality
            [TestMethod]
            public void TC1_TemperatureEquality_CelsiusToCelsius_SameValue()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));

                Assert.IsTrue(t1.Equals(t2));
            }

            // TC2: Fahrenheit-to-Fahrenheit Equality
            [TestMethod]
            public void TC2_TemperatureEquality_FahrenheitToFahrenheit_SameValue()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(32.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(32.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(t1.Equals(t2));
            }

            // TC3: Kelvin-to-Kelvin Equality
            [TestMethod]
            public void TC3_TemperatureEquality_KelvinToKelvin_SameValue()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(273.15, new TemperatureUnitWrapper(TemperatureUnit.KELVIN));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(273.15, new TemperatureUnitWrapper(TemperatureUnit.KELVIN));

                Assert.IsTrue(t1.Equals(t2));
            }

            // TC4: Cross-Unit Celsius to Fahrenheit (0°C = 32°F)
            [TestMethod]
            public void TC4_TemperatureEquality_CelsiusToFahrenheit_0CEquals32F()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tF = new QuantityGeneric<TemperatureUnitWrapper>(32.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(tC.Equals(tF));
            }

            // TC5: Cross-Unit Celsius to Fahrenheit (100°C = 212°F)
            [TestMethod]
            public void TC5_TemperatureEquality_CelsiusToFahrenheit_100CEquals212F()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(100.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tF = new QuantityGeneric<TemperatureUnitWrapper>(212.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(tC.Equals(tF));
            }

            // TC6: Cross-Unit Celsius to Kelvin (0°C = 273.15 K)
            [TestMethod]
            public void TC6_TemperatureEquality_CelsiusToKelvin_0CEquals273K()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tK = new QuantityGeneric<TemperatureUnitWrapper>(273.15, new TemperatureUnitWrapper(TemperatureUnit.KELVIN));

                Assert.IsTrue(tC.Equals(tK));
            }

            // TC7: Cross-Unit Fahrenheit to Kelvin (32°F = 273.15 K)
            [TestMethod]
            public void TC7_TemperatureEquality_FahrenheitToKelvin_32FEquals273K()
            {
                var tF = new QuantityGeneric<TemperatureUnitWrapper>(32.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                var tK = new QuantityGeneric<TemperatureUnitWrapper>(273.15, new TemperatureUnitWrapper(TemperatureUnit.KELVIN));

                Assert.IsTrue(tF.Equals(tK));
            }

            // TC8: Symmetric Equality
            [TestMethod]
            public void TC8_TemperatureEquality_SymmetricProperty()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(32.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(t1.Equals(t2));
                Assert.IsTrue(t2.Equals(t1));
            }

            // TC9: Reflexive Equality
            [TestMethod]
            public void TC9_TemperatureEquality_ReflexiveProperty()
            {
                var t = new QuantityGeneric<TemperatureUnitWrapper>(50.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                Assert.IsTrue(t.Equals(t));
            }

            // TC10: Transitive Equality
            [TestMethod]
            public void TC10_TemperatureEquality_TransitiveProperty()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(32.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                var t3 = new QuantityGeneric<TemperatureUnitWrapper>(273.15, new TemperatureUnitWrapper(TemperatureUnit.KELVIN));

                Assert.IsTrue(t1.Equals(t2));
                Assert.IsTrue(t2.Equals(t3));
                Assert.IsTrue(t1.Equals(t3));
            }

            // ---------------- CONVERSION TESTS ----------------

            // TC11: Celsius to Fahrenheit Conversion
            [TestMethod]
            public void TC11_TemperatureConversion_CelsiusToFahrenheit_VariousValues()
            {
                double[] celsius = { 0, 100, -40, 50 };
                double[] expectedF = { 32, 212, -40, 122 };

                for (int i = 0; i < celsius.Length; i++)
                {
                    var tC = new QuantityGeneric<TemperatureUnitWrapper>(celsius[i], new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                    var tF = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                    Assert.IsTrue(Math.Abs(tF.Value - expectedF[i]) < 0.01);
                }
            }

            // TC12: Fahrenheit to Celsius Conversion
            [TestMethod]
            public void TC12_TemperatureConversion_FahrenheitToCelsius_VariousValues()
            {
                double[] fahrenheit = { 32, 212, -40, 122 };
                double[] expectedC = { 0, 100, -40, 50 };

                for (int i = 0; i < fahrenheit.Length; i++)
                {
                    var tF = new QuantityGeneric<TemperatureUnitWrapper>(fahrenheit[i], new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                    var tC = tF.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                    Assert.IsTrue(Math.Abs(tC.Value - expectedC[i]) < 0.01);
                }
            }

            // TC13: Celsius to Kelvin Conversion
            [TestMethod]
            public void TC13_TemperatureConversion_CelsiusToKelvin()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tK = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.KELVIN));
                Assert.IsTrue(Math.Abs(tK.Value - 273.15) < 0.01);
            }

            // TC14: Kelvin to Celsius Conversion
            [TestMethod]
            public void TC14_TemperatureConversion_KelvinToCelsius()
            {
                var tK = new QuantityGeneric<TemperatureUnitWrapper>(273.15, new TemperatureUnitWrapper(TemperatureUnit.KELVIN));
                var tC = tK.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                Assert.IsTrue(Math.Abs(tC.Value - 0.0) < 0.01);
            }

            // TC15: Round Trip Conversion Preserves Value
            [TestMethod]
            public void TC15_TemperatureConversion_RoundTripPreservesValue()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(50.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tF = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                var tC2 = tF.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));

                Assert.IsTrue(Math.Abs(tC.Value - tC2.Value) < 0.01);
            }

            // TC16: Conversion of Zero Value
            [TestMethod]
            public void TC16_TemperatureConversion_ZeroValue()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(0.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tF = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.AreEqual(32.0, tF.Value);
            }

            // TC17: Absolute Zero Edge Case
            [TestMethod]
            public void TC17_TemperatureConversion_AbsoluteZero()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(-273.15, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tK = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.KELVIN));
                var tF = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(Math.Abs(tK.Value - 0.0) < 0.01);
                Assert.IsTrue(Math.Abs(tF.Value - (-459.67)) < 0.01);
            }

            // TC18: Intersection Point (-40°C = -40°F)
            [TestMethod]
            public void TC18_TemperatureConversion_IntersectionNegative40()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(-40.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tF = new QuantityGeneric<TemperatureUnitWrapper>(-40.0, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(tC.Equals(tF));
            }

            // ---------------- UNSUPPORTED OPERATION TESTS ----------------

            [TestMethod]
            public void TC19_TemperatureUnsupported_Add_ShouldThrowException()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(100.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(50.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));

                 try
                {
                    t1.Add(t2);
                    Assert.Fail("Expected NotSupportedException was not thrown.");
                }
                catch (NotSupportedException)
                {
                    // Test passes
                }
            }

            [TestMethod]
            public void TC20_TemperatureUnsupported_Subtract_ShouldThrowException()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(100.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(50.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));

                try
                {
                    t1.Subtract(t2);
                    Assert.Fail("Expected NotSupportedException was not thrown.");
                }
                catch (NotSupportedException)
                {
                    // Test passes
                }
            }

            [TestMethod]
            public void TC21_TemperatureUnsupported_Divide_ShouldThrowException()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(100.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(50.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                try
                {
                    t1.Divide(t2);
                    Assert.Fail("Expected NotSupportedException was not thrown.");
                }
                catch (NotSupportedException)
                {
                    // Test passes
                }
            }

            // ---------------- NULL AND CROSS-CATEGORY TESTS ----------------

            [TestMethod]
            public void TC22_TemperatureNullOperandValidation()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(100.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                Assert.IsFalse(t1.Equals(null));
            }

            [TestMethod]
            public void TC23_TemperatureVsLengthIncompatibility()
            {
                var t = new QuantityGeneric<TemperatureUnitWrapper>(100.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var length = new QuantityGeneric<LengthUnit>(100.0, LengthUnit.FEET);
                Assert.IsFalse(t.Equals((object)length));
            }

            [TestMethod]
            public void TC24_TemperatureVsWeightIncompatibility()
            {
                var t = new QuantityGeneric<TemperatureUnitWrapper>(50.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var weight = new QuantityGeneric<WeightUnit>(50.0, WeightUnit.KILOGRAM);
                Assert.IsFalse(t.Equals((object)weight));
            }

            [TestMethod]
            public void TC25_TemperatureVsVolumeIncompatibility()
            {
                var t = new QuantityGeneric<TemperatureUnitWrapper>(25.0, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var volume = new QuantityGeneric<VolumeUnit>(25.0, VolumeUnit.LITRE);
                Assert.IsFalse(t.Equals((object)volume));
            }

            // ---------------- PRECISION AND ROUNDING ----------------

            [TestMethod]
            public void TC26_TemperatureConversionPrecision_Epsilon()
            {
                var tC = new QuantityGeneric<TemperatureUnitWrapper>(0.123456, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var tF = tC.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));
                var tC2 = tF.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));

                Assert.IsTrue(Math.Abs(tC.Value - tC2.Value) < 0.01);
            }

            [TestMethod]
            public void TC27_TemperatureConversionEdgeCase_VerySmallDifference()
            {
                var t1 = new QuantityGeneric<TemperatureUnitWrapper>(0.0001, new TemperatureUnitWrapper(TemperatureUnit.CELSIUS));
                var t2 = new QuantityGeneric<TemperatureUnitWrapper>(32.00018, new TemperatureUnitWrapper(TemperatureUnit.FAHRENHEIT));

                Assert.IsTrue(Math.Abs(t1.Value - t2.ConvertTo(new TemperatureUnitWrapper(TemperatureUnit.CELSIUS)).Value) < 0.01);
            }
        }
}