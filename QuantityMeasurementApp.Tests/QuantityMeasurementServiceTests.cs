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

        // UC1: testEquality_SameValue
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

        // UC1: testEquality_DifferentValue
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

        // UC1: testEquality_NullComparison
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

        // UC1: testEquality_SameReference (Reflexive property)
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

        // UC1: testEquality_NonNumericInput (type safety)
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

//====================UC2===========================

        // UC2: testEquality_SameValue for Inches
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

        // UC2: testEquality_DifferentValue for Inches
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

        // UC2: testEquality_NullComparison for Inches
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

        // UC2: testEquality_SameReference for Inches
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



//======================UC3=============================

        // UC3: Same Unit Equality (Feet to Feet)
        [TestMethod]
        public void GivenSameQuantityFeetValues_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(1.0, LengthUnit.Feet);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // UC3: Same Unit Equality (Inch to Inch)
        [TestMethod]
        public void GivenSameQuantityInchValues_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(12.0, LengthUnit.Inch);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inch);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // UC3: Cross Unit Equality (1 Feet == 12 Inch)
        [TestMethod]
        public void GivenEquivalentFeetAndInch_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(12.0, LengthUnit.Inch);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // UC3: Cross Unit Equality (12 Inch == 1 Feet) – Symmetry
        [TestMethod]
        public void GivenEquivalentInchAndFeet_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(12.0, LengthUnit.Inch);
            Quantity q2 = new Quantity(1.0, LengthUnit.Feet);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsTrue(result);
        }

        // UC3: Different Values Should Return False
        [TestMethod]
        public void GivenDifferentQuantityValues_ShouldReturnFalse()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity q2 = new Quantity(2.0, LengthUnit.Feet);

            // Act
            bool result = service.AreEqual(q1, q2);

            // Assert
            Assert.IsFalse(result);
        }

        // UC3: Null Comparison
        [TestMethod]
        public void GivenNullQuantity_ShouldReturnFalse()
        {
            // Arrange
            Quantity q1 = new Quantity(1.0, LengthUnit.Feet);
            Quantity? q2 = null;

            // Act
            bool result = service.AreEqual(q1, q2!);

            // Assert
            Assert.IsFalse(result);
        }

        // UC3: Same Reference (Reflexive Property)
        [TestMethod]
        public void GivenSameQuantityReference_ShouldReturnTrue()
        {
            // Arrange
            Quantity q1 = new Quantity(5.0, LengthUnit.Feet);

            // Act
            bool result = service.AreEqual(q1, q1);

            // Assert
            Assert.IsTrue(result);
        }
    }
}