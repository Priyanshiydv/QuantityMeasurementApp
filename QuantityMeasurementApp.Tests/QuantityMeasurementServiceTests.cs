using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementServiceTests
    {
        private const double EPSILON = 0.000001; //UC5
        private QuantityMeasurementService service = null!;

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
        public void GivenFeet_WhenConvertedToInches_ShouldReturnCorrectValue()
        {
            double result = Quantity.Convert(1.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(12.0, result, EPSILON);
        }

        // TC2: Inches → Feet
        [TestMethod]
        public void GivenInches_WhenConvertedToFeet_ShouldReturnCorrectValue()
        {
            double result = Quantity.Convert(24.0, LengthUnit.INCHES, LengthUnit.FEET);
            
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC3: Yards → Inches
        [TestMethod]
        public void GivenYards_WhenConvertedToInches_ShouldReturnCorrectValue()
        {
            double result = Quantity.Convert(1.0, LengthUnit.YARDS, LengthUnit.INCHES);
            
            Assert.AreEqual(36.0, result, EPSILON);
        }

        // TC4: Inches → Yards
        [TestMethod]
        public void GivenInches_WhenConvertedToYards_ShouldReturnCorrectValue()
        {
            double result = Quantity.Convert(72.0, LengthUnit.INCHES, LengthUnit.YARDS);
            
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC5: Centimeter → Inches
        [TestMethod]
        public void GivenCentimeter_WhenConvertedToInches_ShouldReturnApproxOne()
        {
            double result = Quantity.Convert(2.54, LengthUnit.CENTIMETERS, LengthUnit.INCHES);
            
            Assert.AreEqual(1.0, result, EPSILON);
        }

        // TC6: Feet → Yards
        [TestMethod]
        public void GivenFeet_WhenConvertedToYards_ShouldReturnCorrectValue()
        {
            double result = Quantity.Convert(6.0, LengthUnit.FEET, LengthUnit.YARDS);
            
            Assert.AreEqual(2.0, result, EPSILON);
        }

        // TC7: Round Trip Conversion
        [TestMethod]
        public void GivenValue_WhenConvertedBackAndForth_ShouldPreserveValue()
        {
            double original = 5.5;

            double toInches = Quantity.Convert(original, LengthUnit.FEET, LengthUnit.INCHES);
            double backToFeet = Quantity.Convert(toInches, LengthUnit.INCHES, LengthUnit.FEET);

            Assert.AreEqual(original, backToFeet, EPSILON);
        }

        // TC8: Zero Value
        [TestMethod]
        public void GivenZeroValue_WhenConverted_ShouldReturnZero()
        {
            double result = Quantity.Convert(0.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(0.0, result, EPSILON);
        }

        // TC9: Negative Value
        [TestMethod]
        public void GivenNegativeValue_WhenConverted_ShouldPreserveSign()
        {
            double result = Quantity.Convert(-1.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(-12.0, result, EPSILON);
        }

        // TC10: Same Unit Conversion
        [TestMethod]
        public void GivenSameUnit_WhenConverted_ShouldReturnSameValue()
        {
            double result = Quantity.Convert(5.0, LengthUnit.FEET, LengthUnit.FEET);
            
            Assert.AreEqual(5.0, result, EPSILON);
        }

        // TC11: Large Value
        [TestMethod]
        public void GivenLargeValue_WhenConverted_ShouldMaintainPrecision()
        {
            double result = Quantity.Convert(1000000.0, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(12000000.0, result, EPSILON);
        }

        // TC12: Small Value
        [TestMethod]
        public void GivenSmallValue_WhenConverted_ShouldMaintainPrecision()
        {
            double result = Quantity.Convert(0.0001, LengthUnit.FEET, LengthUnit.INCHES);
            
            Assert.AreEqual(0.0012, result, EPSILON);
        }

        // TC13: NaN Should Throw
        [TestMethod]
        public void GivenNaNValue_WhenConverted_ShouldThrowException()
        {
            try
            {
                Quantity.Convert(double.NaN, LengthUnit.FEET, LengthUnit.INCHES);
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
                Quantity.Convert(double.PositiveInfinity, LengthUnit.FEET, LengthUnit.INCHES);
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

            double toFeet = Quantity.Convert(original, LengthUnit.YARDS, LengthUnit.FEET);
            double toCm = Quantity.Convert(toFeet, LengthUnit.FEET, LengthUnit.CENTIMETERS);
            double backToYard = Quantity.Convert(toCm, LengthUnit.CENTIMETERS, LengthUnit.YARDS);

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
                service.Add(q1, q2, (LengthUnit)(-1));
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
                service.Add(q1, q2, (LengthUnit)(-1));
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
    }
}