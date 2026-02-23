using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurementApp.Models;
using QuantityMeasurementApp.Services;

namespace QuantityMeasurementApp.Tests
{
    [TestClass]
    public class QuantityMeasurementServiceTests
    {
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
    }
}