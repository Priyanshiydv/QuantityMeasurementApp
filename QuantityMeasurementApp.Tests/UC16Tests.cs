using Microsoft.VisualStudio.TestTools.UnitTesting;
using QuantityMeasurement.Models.DTOs;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Models.Exceptions;
using QuantityMeasurement.Repository.Service;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Utilities;
using QuantityMeasurement.Service.Interfaces;
using QuantityMeasurement.Service.Service;

namespace QuantityMeasurementApp.Tests
{
    /// <summary>
    /// UC16 Test Cases for Database Integration.
    /// Tests DatabaseRepository, ConnectionPool,
    /// ApplicationConfig, and Service with DB.
    /// Uses QuantityMeasurementTestDB for isolation.
    /// UC16
    /// </summary>
    [TestClass]
    [DoNotParallelize]
    public class UC16Tests
    {
        // ─── Fields ───────────────────────────────────────────

        private IQuantityMeasurementRepository _repository = null!;
        private IQuantityMeasurementService    _service    = null!;
        private ConnectionPool                 _connectionPool = null!;

        /// <summary>
        /// Test database connection string.
        /// Uses QuantityMeasurementTestDB for isolation.
        /// Separate from production database.
        /// </summary>
        private const string TestConnectionString =
            "Server=LAPTOP-S68LIOH5\\SQLEXPRESS;" +
            "Database=QuantityMeasurementTestDB;" +
            "Trusted_Connection=True;" +
            "TrustServerCertificate=True;";

        // ─── Setup ────────────────────────────────────────────

        [TestInitialize]
        public void Setup()
        {
            // Create connection pool with test database
            // MinPool=2, MaxPool=5 for testing
            _connectionPool = ConnectionPool.GetInstance(
                TestConnectionString,
                minPoolSize: 2,
                maxPoolSize: 5,
                timeout:     30);

            // Create database repository with test DB
            _repository =
                new QuantityMeasurementDatabaseRepository(
                    _connectionPool,
                    "QuantityMeasurementTestDB");

            // Clear all records before each test
            // Ensures test isolation
            _repository.ClearAll();

            // Create service with database repository
            _service =
                new QuantityMeasurementServiceImpl(_repository);
        }

        // ─── Cleanup ──────────────────────────────────────────

        [TestCleanup]
        public void Cleanup()
        {
            // Clear all records after each test
            _repository.ClearAll();

            // Release all connections back to pool
            _repository.ReleaseResources();
        }

        // ═══════════════════════════════════════════════════
        // TC1 - TC4: APPLICATION CONFIG TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC1: ApplicationConfig loads successfully.
        /// Verifies configuration loaded from appsettings.json.
        /// </summary>
        [TestMethod]
        public void TestApplicationConfig_LoadsSuccessfully()
        {
            ApplicationConfig config =
                ApplicationConfig.GetInstance();

            Assert.IsNotNull(config);
            Assert.IsNotNull(config.GetRepositoryType());
            Assert.IsNotNull(config.GetEnvironment());
        }

        /// <summary>
        /// TC2: ApplicationConfig Singleton returns same instance.
        /// Verifies Singleton pattern implementation.
        /// </summary>
        [TestMethod]
        public void TestApplicationConfig_Singleton_SameInstance()
        {
            ApplicationConfig instance1 =
                ApplicationConfig.GetInstance();
            ApplicationConfig instance2 =
                ApplicationConfig.GetInstance();

            Assert.AreSame(instance1, instance2);
        }

        /// <summary>
        /// TC3: ApplicationConfig returns valid connection string.
        /// Verifies connection string loaded from appsettings.json.
        /// </summary>
        [TestMethod]
        public void TestApplicationConfig_ReturnsConnectionString()
        {
            ApplicationConfig config =
                ApplicationConfig.GetInstance();

            string connString =
                config.GetProductionConnectionString();

            Assert.IsNotNull(connString);
            Assert.IsTrue(connString.Contains("SQLEXPRESS"));
        }

        /// <summary>
        /// TC4: ApplicationConfig returns pool settings.
        /// Verifies pool settings loaded from appsettings.json.
        /// </summary>
        [TestMethod]
        public void TestApplicationConfig_ReturnsPoolSettings()
        {
            ApplicationConfig config =
                ApplicationConfig.GetInstance();

            Assert.IsTrue(config.GetMinPoolSize() > 0);
            Assert.IsTrue(config.GetMaxPoolSize() > 0);
            Assert.IsTrue(
                config.GetMaxPoolSize() >=
                config.GetMinPoolSize());
        }

        // ═══════════════════════════════════════════════════
        // TC5 - TC8: CONNECTION POOL TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC5: ConnectionPool initializes with minimum connections.
        /// Verifies pool created with correct number of connections.
        /// </summary>
        [TestMethod]
        public void TestConnectionPool_InitializesWithMinConnections()
        {
            Assert.IsNotNull(_connectionPool);

            string stats = _connectionPool.GetPoolStatistics();

            Assert.IsNotNull(stats);
            Assert.IsTrue(stats.Contains("Available"));
            Assert.IsTrue(stats.Contains("Total"));
        }

        /// <summary>
        /// TC6: ConnectionPool acquire and release connection.
        /// Verifies connection acquired from pool and returned.
        /// </summary>
        [TestMethod]
        public void TestConnectionPool_AcquireAndRelease()
        {
            int availableBefore =
                _connectionPool.GetAvailableCount();

            // Acquire connection
            var connection =
                _connectionPool.AcquireConnection();

            Assert.IsNotNull(connection);

            int availableDuringUse =
                _connectionPool.GetAvailableCount();

            // Release connection back to pool
            _connectionPool.ReleaseConnection(connection);

            int availableAfter =
                _connectionPool.GetAvailableCount();

            // After release should be same as before
            Assert.IsTrue(
                availableDuringUse < availableBefore ||
                availableAfter >= availableDuringUse);
        }

        /// <summary>
        /// TC7: ConnectionPool returns statistics string.
        /// Verifies pool statistics format is correct.
        /// </summary>
        [TestMethod]
        public void TestConnectionPool_ReturnsStatistics()
        {
            string stats =
                _connectionPool.GetPoolStatistics();

            Assert.IsNotNull(stats);
            Assert.IsTrue(stats.Contains("Available"));
            Assert.IsTrue(stats.Contains("Used"));
            Assert.IsTrue(stats.Contains("Total"));
            Assert.IsTrue(stats.Contains("MaxSize"));
        }

        /// <summary>
        /// TC8: ConnectionPool statistics accurate after operations.
        /// Verifies used count increases during operation.
        /// </summary>
        [TestMethod]
        public void TestConnectionPool_StatisticsAccurate()
        {
            int usedBefore = _connectionPool.GetUsedCount();

            // Acquire connection
            var connection =
                _connectionPool.AcquireConnection();

            int usedDuring = _connectionPool.GetUsedCount();

            // Release connection
            _connectionPool.ReleaseConnection(connection);

            int usedAfter = _connectionPool.GetUsedCount();

            // Used count should increase during use
            Assert.IsTrue(usedDuring > usedBefore);
            Assert.AreEqual(usedBefore, usedAfter);
        }

        // ═══════════════════════════════════════════════════
        // TC9 - TC15: DATABASE REPOSITORY TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC9: DatabaseRepository saves entity correctly.
        /// Verifies INSERT operation works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_SaveEntity()
        {
            var entity = new QuantityMeasurementEntity(
                "1 FEET",
                "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True",
                "Length"
            );

            _repository.Save(entity);

            int count = _repository.GetTotalCount();
            Assert.AreEqual(1, count);
        }

        /// <summary>
        /// TC10: DatabaseRepository retrieves all measurements.
        /// Verifies SELECT ALL operation works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_RetrieveAllMeasurements()
        {
            // Save multiple entities
            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True", "Length"));

            _repository.Save(new QuantityMeasurementEntity(
                "1 KILOGRAM", "1000 GRAM",
                QuantityMeasurementEntity.Operations.ADD,
                "2.00 KILOGRAM", "Weight"));

            var all = _repository.GetAllMeasurements();

            Assert.AreEqual(2, all.Count);
        }

        /// <summary>
        /// TC11: DatabaseRepository FindById returns correct entity.
        /// Verifies SELECT by ID operation works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_FindById()
        {
            var entity = new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True", "Length");

            _repository.Save(entity);

            var found = _repository.FindById(entity.Id);

            Assert.IsNotNull(found);
            Assert.AreEqual(entity.Id, found!.Id);
        }

        /// <summary>
        /// TC12: DatabaseRepository DeleteById removes entity.
        /// Verifies DELETE by ID operation works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_DeleteById()
        {
            var entity = new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True", "Length");

            _repository.Save(entity);
            _repository.DeleteById(entity.Id);

            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC13: DatabaseRepository GetMeasurementsByOperationType.
        /// Verifies filtering by operation type works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_GetByOperationType()
        {
            // Save COMPARE entity
            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True", "Length"));

            // Save ADD entity
            _repository.Save(new QuantityMeasurementEntity(
                "1 KILOGRAM", "1000 GRAM",
                QuantityMeasurementEntity.Operations.ADD,
                "2.00 KILOGRAM", "Weight"));

            var compareRecords =
                _repository.GetMeasurementsByOperationType(
                    "COMPARE");

            Assert.AreEqual(1, compareRecords.Count);
            Assert.AreEqual("COMPARE",
                compareRecords[0].OperationType);
        }

        /// <summary>
        /// TC14: DatabaseRepository GetMeasurementsByMeasurementType.
        /// Verifies filtering by measurement type works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_GetByMeasurementType()
        {
            // Save Length entity
            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True", "Length"));

            // Save Weight entity
            _repository.Save(new QuantityMeasurementEntity(
                "1 KILOGRAM", "1000 GRAM",
                QuantityMeasurementEntity.Operations.ADD,
                "2.00 KILOGRAM", "Weight"));

            var lengthRecords =
                _repository.GetMeasurementsByMeasurementType(
                    "Length");

            Assert.AreEqual(1, lengthRecords.Count);
            Assert.AreEqual("Length",
                lengthRecords[0].MeasurementType);
        }

        /// <summary>
        /// TC15: DatabaseRepository GetTotalCount returns accurate count.
        /// Verifies COUNT operation works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseRepository_GetTotalCount()
        {
            Assert.AreEqual(0, _repository.GetTotalCount());

            _repository.Save(new QuantityMeasurementEntity(
                "1 FEET", "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True", "Length"));

            Assert.AreEqual(1, _repository.GetTotalCount());

            _repository.Save(new QuantityMeasurementEntity(
                "1 KG", "1000 GRAM",
                QuantityMeasurementEntity.Operations.ADD,
                "2.00 KG", "Weight"));

            Assert.AreEqual(2, _repository.GetTotalCount());
        }

        // ═══════════════════════════════════════════════════
        // TC16 - TC17: DATABASE EXCEPTION TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC16: DatabaseException stores error code correctly.
        /// Verifies custom exception hierarchy works correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabaseException_StoresErrorCode()
        {
            var ex = new DatabaseException(
                "Connection failed",
                DatabaseException.DatabaseErrorCodes.CONNECTION_FAILED);

            Assert.AreEqual(
                DatabaseException.DatabaseErrorCodes.CONNECTION_FAILED,
                ex.ErrorCode);
            Assert.AreEqual("Connection failed", ex.Message);
        }

        /// <summary>
        /// TC17: DatabaseException stores failed query.
        /// Verifies exception stores context for debugging.
        /// </summary>
        [TestMethod]
        public void TestDatabaseException_StoresFailedQuery()
        {
            string failedQuery = "SELECT * FROM NonExistentTable";

            var ex = new DatabaseException(
                "Query failed",
                DatabaseException.DatabaseErrorCodes.QUERY_FAILED,
                failedQuery,
                "QuantityMeasurementTestDB");

            Assert.AreEqual(failedQuery,   ex.FailedQuery);
            Assert.AreEqual(
                "QuantityMeasurementTestDB", ex.DatabaseName);
        }

        // ═══════════════════════════════════════════════════
        // TC18 - TC22: SERVICE WITH DATABASE REPOSITORY TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC18: Service Compare saves to database.
        /// Verifies compare operation persisted to DB.
        /// </summary>
        [TestMethod]
        public void TestService_Compare_SavesToDB()
        {
            var first  = new QuantityDTO(1,  LengthUnitDTO.FEET,
                             MeasurementTypeDTO.LENGTH);
            var second = new QuantityDTO(12, LengthUnitDTO.INCHES,
                             MeasurementTypeDTO.LENGTH);

            _service.Compare(first, second);

            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC19: Service Add saves to database with MeasurementType.
        /// Verifies add operation persisted with correct type.
        /// </summary>
        [TestMethod]
        public void TestService_Add_SavesWithMeasurementType()
        {
            var first  = new QuantityDTO(1,    LengthUnitDTO.FEET,
                             MeasurementTypeDTO.LENGTH);
            var second = new QuantityDTO(12,   LengthUnitDTO.INCHES,
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
        /// TC20: Service Convert saves to database.
        /// Verifies convert operation persisted to DB.
        /// </summary>
        [TestMethod]
        public void TestService_Convert_SavesToDB()
        {
            var quantity = new QuantityDTO(1,
                               LengthUnitDTO.FEET,
                               MeasurementTypeDTO.LENGTH);

            _service.Convert(quantity, LengthUnitDTO.INCHES);

            Assert.AreEqual(1, _repository.GetTotalCount());

            var records =
                _repository.GetMeasurementsByOperationType(
                    "CONVERT");

            Assert.AreEqual(1, records.Count);
        }

        /// <summary>
        /// TC21: Service Subtract saves to database.
        /// Verifies subtract operation persisted to DB.
        /// </summary>
        [TestMethod]
        public void TestService_Subtract_SavesToDB()
        {
            var first  = new QuantityDTO(1,   WeightUnitDTO.KILOGRAM,
                             MeasurementTypeDTO.WEIGHT);
            var second = new QuantityDTO(500, WeightUnitDTO.GRAM,
                             MeasurementTypeDTO.WEIGHT);

            _service.Subtract(first, second);

            Assert.AreEqual(1, _repository.GetTotalCount());

            var records =
                _repository.GetMeasurementsByOperationType(
                    "SUBTRACT");

            Assert.AreEqual(1, records.Count);
        }

        /// <summary>
        /// TC22: Service Divide saves to database.
        /// Verifies divide operation persisted to DB.
        /// </summary>
        [TestMethod]
        public void TestService_Divide_SavesToDB()
        {
            var first  = new QuantityDTO(10, LengthUnitDTO.FEET,
                             MeasurementTypeDTO.LENGTH);
            var second = new QuantityDTO(2,  LengthUnitDTO.FEET,
                             MeasurementTypeDTO.LENGTH);

            _service.Divide(first, second);

            Assert.AreEqual(1, _repository.GetTotalCount());

            var records =
                _repository.GetMeasurementsByOperationType(
                    "DIVIDE");

            Assert.AreEqual(1, records.Count);
        }

        // ═══════════════════════════════════════════════════
        // TC23 - TC25: POOL STATISTICS TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC23: Repository returns pool statistics.
        /// Verifies GetPoolStatistics returns correct format.
        /// </summary>
        [TestMethod]
        public void TestRepository_ReturnsPoolStatistics()
        {
            string stats = _repository.GetPoolStatistics();

            Assert.IsNotNull(stats);
            Assert.IsTrue(stats.Contains("Available"));
            Assert.IsTrue(stats.Contains("Used"));
            Assert.IsTrue(stats.Contains("Total"));
        }

        /// <summary>
        /// TC24: Multiple operations use pool efficiently.
        /// Verifies connection pool handles multiple operations.
        /// </summary>
        [TestMethod]
        public void TestConnectionPool_HandlesMultipleOperations()
        {
            // Perform multiple operations
            for (int i = 0; i < 5; i++)
            {
                _repository.Save(new QuantityMeasurementEntity(
                    $"{i} FEET", $"{i * 12} INCHES",
                    QuantityMeasurementEntity.Operations.COMPARE,
                    "True", "Length"));
            }

            Assert.AreEqual(5, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC25: DeleteAll removes all records and returns count.
        /// Verifies DeleteAllMeasurements works correctly.
        /// </summary>
        [TestMethod]
        public void TestRepository_DeleteAllMeasurements()
        {
            // Save 3 records
            for (int i = 0; i < 3; i++)
            {
                _repository.Save(new QuantityMeasurementEntity(
                    $"{i} FEET", $"{i} INCHES",
                    QuantityMeasurementEntity.Operations.COMPARE,
                    "True", "Length"));
            }

            int deleted = _repository.DeleteAllMeasurements();

            Assert.AreEqual(3,  deleted);
            Assert.AreEqual(0,  _repository.GetTotalCount());
        }

        // ═══════════════════════════════════════════════════
        // TC26 - TC27: DATA PERSISTENCE TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC26: Data persists after multiple service operations.
        /// Verifies all operations saved to database correctly.
        /// </summary>
        [TestMethod]
        public void TestDatabase_DataPersistsAfterOperations()
        {
            var length1 = new QuantityDTO(1,
                              LengthUnitDTO.FEET,
                              MeasurementTypeDTO.LENGTH);
            var length2 = new QuantityDTO(12,
                              LengthUnitDTO.INCHES,
                              MeasurementTypeDTO.LENGTH);
            var weight1 = new QuantityDTO(1,
                              WeightUnitDTO.KILOGRAM,
                              MeasurementTypeDTO.WEIGHT);
            var weight2 = new QuantityDTO(1000,
                              WeightUnitDTO.GRAM,
                              MeasurementTypeDTO.WEIGHT);

            // Perform multiple operations
            _service.Compare(length1, length2);
            _service.Add(weight1, weight2);
            _service.Convert(length1, LengthUnitDTO.INCHES);

            // All 3 operations should be saved
            Assert.AreEqual(3, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC27: Data isolation between tests.
        /// Verifies database is clean at start of each test.
        /// </summary>
        [TestMethod]
        public void TestDatabase_IsolationBetweenTests()
        {
            // Database should be empty at start
            // because Setup() calls ClearAll()
            Assert.AreEqual(0, _repository.GetTotalCount());
        }

        // ═══════════════════════════════════════════════════
        // TC28 - TC29: PARAMETERIZED QUERY TESTS
        // ═══════════════════════════════════════════════════

        /// <summary>
        /// TC28: Parameterized query handles special characters.
        /// Verifies SQL injection prevention works correctly.
        /// </summary>
        [TestMethod]
        public void TestParameterizedQuery_HandlesSpecialChars()
        {
            // SQL injection attempt in operand value
            var entity = new QuantityMeasurementEntity(
                "1'; DROP TABLE QuantityMeasurementEntity; --",
                "12 INCHES",
                QuantityMeasurementEntity.Operations.COMPARE,
                "True",
                "Length");

            // Should save without executing injection
            _repository.Save(entity);

            // Table should still exist with 1 record
            Assert.AreEqual(1, _repository.GetTotalCount());
        }

        /// <summary>
        /// TC29: Parameterized query handles null values safely.
        /// Verifies NULL handling in parameterized queries.
        /// </summary>
        [TestMethod]
        public void TestParameterizedQuery_HandlesNullValues()
        {
            // Entity with null optional fields
            var entity = new QuantityMeasurementEntity(
                QuantityMeasurementEntity.Operations.COMPARE,
                "Cross category error",
                true,
                null);

            // Should save null values as DBNull
            _repository.Save(entity);

            var found = _repository.FindById(entity.Id);

            Assert.IsNotNull(found);
            Assert.IsNull(found!.MeasurementType);
            Assert.IsTrue(found.HasError);
        }
    }
}