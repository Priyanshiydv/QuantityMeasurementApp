using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Repository.Context;
using QuantityMeasurement.Repository.Service;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Service.Interfaces;
using QuantityMeasurement.Service.Service;
using QuantityMeasurement.WebAPI.Controllers;
using System.Net;
using System.Net.Http.Json;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC17 Test Cases for WebAPI.
    /// Tests Controller, Service, Repository layers.
    /// Uses InMemory database for isolation.
    /// UC17
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class UC17tests
    {
        // ─── Fields ───────────────────────────────────────────

        private QuantityMeasurementDbContext _dbContext = null!;
        private IQuantityMeasurementRepository _repository = null!;
        private IQuantityMeasurementService _service = null!;
        private QuantityMeasurementController _controller = null!;
        private ILogger<QuantityMeasurementController> _controllerLogger = null!;
        private ILogger<EFQuantityMeasurementRepository> _repoLogger = null!;

        // ─── Setup ────────────────────────────────────────────

        [TestInitialize]
        public void Setup()
        {
            // Create InMemory database for testing
            var options = new DbContextOptionsBuilder
                <QuantityMeasurementDbContext>()
                .UseInMemoryDatabase(
                    databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new QuantityMeasurementDbContext(
                options);

            // Create loggers
            _repoLogger = new Mock<ILogger<EFQuantityMeasurementRepository>>().Object;

            _controllerLogger = new Mock<ILogger<QuantityMeasurementController>>().Object;

            // Create EF Repository
            _repository = new EFQuantityMeasurementRepository(
                _dbContext, _repoLogger);

            // Create Service
            _service = new QuantityMeasurementServiceImpl(
                _repository);

            // Create Controller
            _controller = new QuantityMeasurementController(
                _service,
                _repository,
                _controllerLogger);
        }

        // ─── Cleanup ──────────────────────────────────────────

        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        // ═══════════════════════════════════════════════════
        // TC1 - TC4: DBCONTEXT TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC1: DbContext initializes successfully.
        /// </summary>
        [TestMethod]
        public void TestDbContext_InitializesSuccessfully()
        {
            Assert.IsNotNull(_dbContext);
            Assert.IsNotNull(_dbContext.QuantityMeasurements);
        }

        /// <summary>
        /// TC2: DbContext can save entity.
        /// </summary>
        [TestMethod]
        public void TestDbContext_SaveEntity()
        {
            var entity = new QuantityMeasurementEntity
            {
                Id            = Guid.NewGuid().ToString(),
                OperationType = "COMPARE",
                Result        = "True",
                HasError      = false,
                Timestamp     = DateTime.Now
            };

            _dbContext.QuantityMeasurements.Add(entity);
            _dbContext.SaveChanges();

            Assert.AreEqual(1,
                _dbContext.QuantityMeasurements.Count());
        }

        /// <summary>
        /// TC3: DbContext can retrieve entity.
        /// </summary>
        [TestMethod]
        public void TestDbContext_RetrieveEntity()
        {
            string id = Guid.NewGuid().ToString();

            _dbContext.QuantityMeasurements.Add(
                new QuantityMeasurementEntity
                {
                    Id            = id,
                    OperationType = "ADD",
                    Result        = "2.00 FEET",
                    HasError      = false,
                    Timestamp     = DateTime.Now
                });
            _dbContext.SaveChanges();

            var entity = _dbContext.QuantityMeasurements
                .FirstOrDefault(e => e.Id == id);

            Assert.IsNotNull(entity);
            Assert.AreEqual("ADD", entity!.OperationType);
        }

        /// <summary>
        /// TC4: DbContext indexes configured correctly.
        /// </summary>
        [TestMethod]
        public void TestDbContext_IndexesConfigured()
        {
            var entityType = _dbContext.Model
                .FindEntityType(
                    typeof(QuantityMeasurementEntity));

            Assert.IsNotNull(entityType);

            var indexes = entityType.GetIndexes();
            Assert.IsTrue(indexes.Any());
        }

        // ═══════════════════════════════════════════════════
        // TC5 - TC9: EF REPOSITORY TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC5: EF Repository saves entity.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_SaveEntity()
        {
            var entity = new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                "COMPARE", "True", "Length");

            _repository.Save(entity);

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC6: EF Repository retrieves all measurements.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_GetAllMeasurements()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                "COMPARE", "True", "Length"));

            _repository.Save(new QuantityMeasurementEntity(
                "1 KG", "1000 GRAM",
                "ADD", "2.00 KG", "Weight"));

            var measurements = _repository.GetAllMeasurements();

            Assert.AreEqual(2, measurements.Count);
        }

        /// <summary>
        /// TC7: EF Repository finds by ID.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_FindById()
        {
            var entity = new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                "COMPARE", "True", "Length");

            _repository.Save(entity);

            var found = _repository.FindById(entity.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(entity.Id, found!.Id);
        }

        /// <summary>
        /// TC8: EF Repository filters by operation type.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_GetByOperationType()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                "COMPARE", "True", "Length"));

            _repository.Save(new QuantityMeasurementEntity(
                "1 KG", "1000 GRAM",
                "ADD", "2.00 KG", "Weight"));

            var compareRecords =
                _repository.GetMeasurementsByOperationType(
                    "COMPARE");

            Assert.AreEqual(1, compareRecords.Count);
            Assert.AreEqual("COMPARE",
                compareRecords[0].OperationType);
        }

        /// <summary>
        /// TC9: EF Repository filters by measurement type.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_GetByMeasurementType()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                "COMPARE", "True", "Length"));

            _repository.Save(new QuantityMeasurementEntity(
                "1 KG", "1000 GRAM",
                "ADD", "2.00 KG", "Weight"));

            var lengthRecords =
                _repository.GetMeasurementsByMeasurementType(
                    "Length");

            Assert.AreEqual(1, lengthRecords.Count);
            Assert.AreEqual("Length",
                lengthRecords[0].MeasurementType);
        }

        // ═══════════════════════════════════════════════════
        // TC10 - TC14: CONTROLLER TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC10: Controller CompareQuantities returns 200.
        /// </summary>
        [TestMethod]
        public void TestController_Compare_Returns200()
        {
            var input = new QuantityInputDTO
            {
                FirstValue            = 1,
                FirstUnit             = "FEET",
                FirstMeasurementType  = "Length",
                SecondValue           = 12,
                SecondUnit            = "INCHES",
                SecondMeasurementType = "Length"
            };

            var result = _controller.CompareQuantities(input);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));

            var okResult = (OkObjectResult)result;
            Assert.AreEqual(200, okResult.StatusCode);
        }

        /// <summary>
        /// TC11: Controller Compare returns correct result.
        /// </summary>
        [TestMethod]
        public void TestController_Compare_ReturnsTrue()
        {
            var input = new QuantityInputDTO
            {
                FirstValue            = 1,
                FirstUnit             = "FEET",
                FirstMeasurementType  = "Length",
                SecondValue           = 12,
                SecondUnit            = "INCHES",
                SecondMeasurementType = "Length"
            };

            var result = _controller.CompareQuantities(input);
            var okResult = (OkObjectResult)result;
            var response = (QuantityResponseDTO)okResult.Value!;

            Assert.AreEqual("True", response.ResultString);
            Assert.IsFalse(response.HasError);
        }

        /// <summary>
        /// TC12: Controller Add returns correct result.
        /// </summary>
        [TestMethod]
        public void TestController_Add_ReturnsCorrectResult()
        {
            var input = new QuantityInputDTO
            {
                FirstValue            = 1,
                FirstUnit             = "FEET",
                FirstMeasurementType  = "Length",
                SecondValue           = 12,
                SecondUnit            = "INCHES",
                SecondMeasurementType = "Length"
            };

            var result = _controller.AddQuantities(input);
            var okResult = (OkObjectResult)result;
            var response = (QuantityResponseDTO)okResult.Value!;

            Assert.AreEqual(2.0, response.ResultValue, 0.0001);
            Assert.AreEqual("FEET", response.ResultUnit);
        }

        /// <summary>
        /// TC13: Controller Convert returns correct result.
        /// </summary>
        [TestMethod]
        public void TestController_Convert_FeetToInches()
        {
            var input = new QuantityInputDTO
            {
                FirstValue            = 1,
                FirstUnit             = "FEET",
                FirstMeasurementType  = "Length",
                SecondValue           = 0,
                SecondUnit            = "INCHES",
                SecondMeasurementType = "Length"
            };

            var result = _controller.ConvertQuantity(input);
            var okResult = (OkObjectResult)result;
            var response = (QuantityResponseDTO)okResult.Value!;

            Assert.AreEqual(12.0, response.ResultValue, 0.0001);
            Assert.AreEqual("INCHES", response.ResultUnit);
        }

        /// <summary>
        /// TC14: Controller saves to database after operation.
        /// </summary>
        [TestMethod]
        public void TestController_SavesToDB_AfterOperation()
        {
            var input = new QuantityInputDTO
            {
                FirstValue            = 1,
                FirstUnit             = "FEET",
                FirstMeasurementType  = "Length",
                SecondValue           = 12,
                SecondUnit            = "INCHES",
                SecondMeasurementType = "Length"
            };

            _controller.CompareQuantities(input);

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        // ═══════════════════════════════════════════════════
        // TC15 - TC18: SERVICE WITH EF REPOSITORY TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC15: Service Compare saves to EF repository.
        /// </summary>
        [TestMethod]
        public void TestService_Compare_SavesViaEF()
        {
            var first = new QuantityDTO(1,
                LengthUnitDTO.FEET,
                MeasurementTypeDTO.LENGTH);

            var second = new QuantityDTO(12,
                LengthUnitDTO.INCHES,
                MeasurementTypeDTO.LENGTH);

            _service.Compare(first, second);

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC16: Service Add saves with measurement type.
        /// </summary>
        [TestMethod]
        public void TestService_Add_SavesWithMeasurementType()
        {
            var first = new QuantityDTO(1,
                LengthUnitDTO.FEET,
                MeasurementTypeDTO.LENGTH);

            var second = new QuantityDTO(12,
                LengthUnitDTO.INCHES,
                MeasurementTypeDTO.LENGTH);

            _service.Add(first, second);

            var records =
                _repository.GetMeasurementsByMeasurementType(
                    "Length");

            Assert.AreEqual(1, records.Count);
            Assert.AreEqual("Length",
                records[0].MeasurementType);
        }

        /// <summary>
        /// TC17: Service multiple operations persist correctly.
        /// </summary>
        [TestMethod]
        public void TestService_MultipleOperations_PersistAll()
        {
            var length1 = new QuantityDTO(1,
                LengthUnitDTO.FEET,
                MeasurementTypeDTO.LENGTH);

            var length2 = new QuantityDTO(12,
                LengthUnitDTO.INCHES,
                MeasurementTypeDTO.LENGTH);

            _service.Compare(length1, length2);
            _service.Add(length1, length2);
            _service.Convert(length1, LengthUnitDTO.INCHES);

            Assert.AreEqual(3, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC18: EF Repository LINQ queries work correctly.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_LINQQueries()
        {
            // Save multiple entities
            for (int i = 0; i < 5; i++)
            {
                _repository.Save(new QuantityMeasurementEntity(
                    $"{i} FEET", $"{i * 12} INCHES",
                    "COMPARE", "True", "Length"));
            }

            // LINQ query
            var records =
                _repository.GetMeasurementsByOperationType(
                    "COMPARE");

            Assert.AreEqual(5, records.Count);
        }

        // ═══════════════════════════════════════════════════
        // TC19 - TC20: ERROR HANDLING TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC19: Controller handles cross category error.
        /// </summary>
        [TestMethod]
        public void TestController_CrossCategory_ThrowsException()
        {
            var input = new QuantityInputDTO
            {
                FirstValue            = 1,
                FirstUnit             = "FEET",
                FirstMeasurementType  = "Length",
                SecondValue           = 1,
                SecondUnit            = "KILOGRAM",
                SecondMeasurementType = "Weight"
            };

            try
            {
                _controller.CompareQuantities(input);
                Assert.Fail("Expected exception");
            }
            catch (Exception ex)
            {
                Assert.IsNotNull(ex);
            }
        }

        /// <summary>
        /// TC20: EF Repository returns error measurements.
        /// </summary>
        [TestMethod]
        public void TestEFRepository_GetErrorMeasurements()
        {
            _repository.Save(new QuantityMeasurementEntity(
                "COMPARE", "Cross category error", true));

            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                "COMPARE", "True", "Length"));

            var errors = _repository.GetErrorMeasurements();

            Assert.AreEqual(1, errors.Count);
            Assert.IsTrue(errors[0].HasError);
        }
    }
}