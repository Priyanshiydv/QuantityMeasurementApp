using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using QuantityMeasurement.Models.Entities;
using QuantityMeasurement.Models.Exceptions;
using QuantityMeasurement.Repository.Interfaces;
using QuantityMeasurement.Repository.Utilities;

namespace QuantityMeasurement.Repository.Service
{
    /// <summary>
    /// Database repository for quantity measurements.
    /// Uses ADO.NET SqlConnection to perform CRUD operations.
    /// Implements IQuantityMeasurementRepository interface.
    /// Uses ConnectionPool for efficient connection management.
    /// Uses parameterized queries to prevent SQL injection.
    /// UC16
    /// </summary>
    public class QuantityMeasurementDatabaseRepository
        : IQuantityMeasurementRepository
    {
        // ─── Fields ───────────────────────────────────────────

        /// <summary>
        /// Connection pool for managing database connections.
        /// </summary>
        private readonly ConnectionPool _connectionPool;

        /// <summary>
        /// Database name for error reporting.
        /// </summary>
        private readonly string _databaseName;

        // ─── SQL Query Constants ──────────────────────────────

        /// <summary>
        /// SQL query to insert a new measurement entity.
        /// Uses parameterized query to prevent SQL injection.
        /// </summary>
        private const string InsertQuery =
            @"INSERT INTO QuantityMeasurementEntity
              (Id, FirstOperand, SecondOperand, OperationType,
               Result, HasError, ErrorMessage, MeasurementType, Timestamp)
              VALUES
              (@Id, @FirstOperand, @SecondOperand, @OperationType,
               @Result, @HasError, @ErrorMessage, @MeasurementType, @Timestamp)";

        /// <summary>
        /// SQL query to select all measurement entities.
        /// </summary>
        private const string SelectAllQuery =
            @"SELECT Id, FirstOperand, SecondOperand, OperationType,
                     Result, HasError, ErrorMessage, MeasurementType, Timestamp
              FROM QuantityMeasurementEntity
              ORDER BY Timestamp DESC";

        /// <summary>
        /// SQL query to select entity by ID.
        /// </summary>
        private const string SelectByIdQuery =
            @"SELECT Id, FirstOperand, SecondOperand, OperationType,
                     Result, HasError, ErrorMessage, MeasurementType, Timestamp
              FROM QuantityMeasurementEntity
              WHERE Id = @Id";

        /// <summary>
        /// SQL query to delete entity by ID.
        /// </summary>
        private const string DeleteByIdQuery =
            @"DELETE FROM QuantityMeasurementEntity
              WHERE Id = @Id";

        /// <summary>
        /// SQL query to delete all entities.
        /// </summary>
        private const string DeleteAllQuery =
            @"DELETE FROM QuantityMeasurementEntity";

        /// <summary>
        /// SQL query to select entities by operation type.
        /// </summary>
        private const string SelectByOperationTypeQuery =
            @"SELECT Id, FirstOperand, SecondOperand, OperationType,
                     Result, HasError, ErrorMessage, MeasurementType, Timestamp
              FROM QuantityMeasurementEntity
              WHERE OperationType = @OperationType
              ORDER BY Timestamp DESC";

        /// <summary>
        /// SQL query to select entities by measurement type.
        /// </summary>
        private const string SelectByMeasurementTypeQuery =
            @"SELECT Id, FirstOperand, SecondOperand, OperationType,
                     Result, HasError, ErrorMessage, MeasurementType, Timestamp
              FROM QuantityMeasurementEntity
              WHERE MeasurementType = @MeasurementType
              ORDER BY Timestamp DESC";

        /// <summary>
        /// SQL query to get total count of entities.
        /// </summary>
        private const string CountQuery =
            @"SELECT COUNT(*) FROM QuantityMeasurementEntity";

        /// <summary>
        /// SQL query to insert record into history table.
        /// Every save operation also saves to history for audit trail.
        /// </summary>
        private const string InsertHistoryQuery =
            @"INSERT INTO QuantityMeasurementHistory
            (EntityId, FirstOperand, SecondOperand, OperationType,
            Result, HasError, ErrorMessage, MeasurementType, Timestamp)
            VALUES
            (@EntityId, @FirstOperand, @SecondOperand, @OperationType,
            @Result, @HasError, @ErrorMessage, @MeasurementType, @Timestamp)";

        // ─── Constructor ──────────────────────────────────────

        /// <summary>
        /// Constructor with dependency injection of ConnectionPool.
        /// Initializes database repository with connection pool.
        /// </summary>
        public QuantityMeasurementDatabaseRepository(
            ConnectionPool connectionPool,
            string databaseName = "QuantityMeasurementDB")
        {
            _connectionPool = connectionPool
                ?? throw new ArgumentNullException(
                    nameof(connectionPool));

            _databaseName = databaseName;

            Console.WriteLine(
                "[DatabaseRepository] Initialized successfully. " +
                $"Database: {_databaseName}");
        }

        // ─── Basic CRUD Operations ────────────────────────────

        /// <summary>
        /// Saves entity to database using parameterized query.
        /// Also saves to history table for audit trail.
        /// Uses transaction to ensure both saves succeed together.
        /// Throws DatabaseException if save fails.
        /// UC16
        /// </summary>
        public void Save(QuantityMeasurementEntity entity)
        {
            // Validate entity
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            // Acquire connection from pool
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            // Begin transaction for data consistency
            // Both entity and history saved together
            SqlTransaction? transaction = null;

            try
            {
                transaction =
                    connection.BeginTransaction();

                // ── Step 1: Save to Entity Table ──────────────
                using (SqlCommand entityCommand =
                    new SqlCommand(
                        InsertQuery, connection, transaction))
                {
                    // Add parameters to prevent SQL injection
                    AddEntityParameters(entityCommand, entity);
                    entityCommand.ExecuteNonQuery();

                    Console.WriteLine(
                        $"[DatabaseRepository] " +
                        $"Saved to Entity table: {entity.Id}");
                }

                // ── Step 2: Save to History Table ─────────────
                using (SqlCommand historyCommand =
                    new SqlCommand(
                        InsertHistoryQuery, connection, transaction))
                {
                    // Add history parameters
                    AddHistoryParameters(historyCommand, entity);
                    historyCommand.ExecuteNonQuery();

                    Console.WriteLine(
                        $"[DatabaseRepository] " +
                        $"Saved to History table: {entity.Id}");
                }

                // ── Step 3: Commit both saves together ─────────
                transaction.Commit();

                Console.WriteLine(
                    $"[DatabaseRepository] " +
                    $"Transaction committed successfully.");
            }
            catch (SqlException sqlEx)
            {
                // Rollback BOTH saves if either fails
                RollbackTransaction(transaction);

                throw new DatabaseException(
                    $"Failed to save entity: {entity.Id}",
                    DatabaseException.DatabaseErrorCodes.INSERT_FAILED,
                    sqlEx,
                    InsertQuery,
                    _databaseName);
            }
            catch (Exception ex)
            {
                RollbackTransaction(transaction);

                throw new DatabaseException(
                    $"Unexpected error saving entity: {entity.Id}",
                    DatabaseException.DatabaseErrorCodes.GENERAL_DB_ERROR,
                    ex,
                    InsertQuery,
                    _databaseName);
            }
            finally
            {
                // Always release connection back to pool
                _connectionPool.ReleaseConnection(connection);
            }
        }
        /// <summary>
        /// Returns all measurement entities from database.
        /// Maps SQL result set to entity objects.
        /// </summary>
        public List<QuantityMeasurementEntity> GetAllMeasurements()
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                List<QuantityMeasurementEntity> measurements =
                    new List<QuantityMeasurementEntity>();

                using SqlCommand command =
                    new SqlCommand(SelectAllQuery, connection);

                using SqlDataReader reader =
                    command.ExecuteReader();

                // Map each row to entity object
                while (reader.Read())
                {
                    measurements.Add(MapReaderToEntity(reader));
                }

                Console.WriteLine(
                    $"[DatabaseRepository] Retrieved " +
                    $"{measurements.Count} measurements.");

                return measurements;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    "Failed to retrieve all measurements.",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    SelectAllQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Finds a measurement entity by its unique ID.
        /// Returns null if not found.
        /// </summary>
        public QuantityMeasurementEntity? FindById(string id)
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                using SqlCommand command =
                    new SqlCommand(SelectByIdQuery, connection);

                // Parameterized query - prevents SQL injection
                command.Parameters.AddWithValue("@Id", id);

                using SqlDataReader reader =
                    command.ExecuteReader();

                if (reader.Read())
                {
                    return MapReaderToEntity(reader);
                }

                return null;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    $"Failed to find entity by ID: {id}",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    SelectByIdQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Deletes a measurement entity by its unique ID.
        /// Uses transaction for data consistency.
        /// </summary>
        public void DeleteById(string id)
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            SqlTransaction? transaction = null;

            try
            {
                transaction = connection.BeginTransaction();

                using SqlCommand command =
                    new SqlCommand(
                        DeleteByIdQuery, connection, transaction);

                command.Parameters.AddWithValue("@Id", id);

                command.ExecuteNonQuery();

                transaction.Commit();

                Console.WriteLine(
                    $"[DatabaseRepository] Deleted entity: {id}");
            }
            catch (SqlException sqlEx)
            {
                RollbackTransaction(transaction);

                throw new DatabaseException(
                    $"Failed to delete entity by ID: {id}",
                    DatabaseException.DatabaseErrorCodes.DELETE_FAILED,
                    sqlEx,
                    DeleteByIdQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Clears all stored measurements from database.
        /// Uses transaction to ensure complete deletion.
        /// </summary>
        public void ClearAll()
        {
            DeleteAllMeasurements();
        }

        // ─── UC16 New Query Methods ───────────────────────────

        /// <summary>
        /// Returns measurements filtered by operation type.
        /// e.g. "COMPARE", "ADD", "SUBTRACT", "DIVIDE", "CONVERT"
        /// Uses parameterized query to prevent SQL injection.
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByOperationType(string operationType)
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                List<QuantityMeasurementEntity> measurements =
                    new List<QuantityMeasurementEntity>();

                using SqlCommand command =
                    new SqlCommand(
                        SelectByOperationTypeQuery, connection);

                command.Parameters.AddWithValue(
                    "@OperationType", operationType);

                using SqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    measurements.Add(MapReaderToEntity(reader));
                }

                return measurements;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    $"Failed to get measurements by " +
                    $"operation type: {operationType}",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    SelectByOperationTypeQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Returns measurements filtered by measurement type.
        /// e.g. "Length", "Weight", "Volume", "Temperature"
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByMeasurementType(string measurementType)
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                List<QuantityMeasurementEntity> measurements =
                    new List<QuantityMeasurementEntity>();

                using SqlCommand command =
                    new SqlCommand(
                        SelectByMeasurementTypeQuery, connection);

                command.Parameters.AddWithValue(
                    "@MeasurementType", measurementType);

                using SqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    measurements.Add(MapReaderToEntity(reader));
                }

                return measurements;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    $"Failed to get measurements by " +
                    $"measurement type: {measurementType}",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    SelectByMeasurementTypeQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Returns total count of measurements in database.
        /// </summary>
        public int GetTotalCount()
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                using SqlCommand command =
                    new SqlCommand(CountQuery, connection);

                object? result = command.ExecuteScalar();

                return result != null
                    ? Convert.ToInt32(result)
                    : 0;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    "Failed to get total count.",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    CountQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Deletes all measurements and returns count deleted.
        /// Uses transaction to ensure complete deletion.
        /// </summary>
        public int DeleteAllMeasurements()
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            SqlTransaction? transaction = null;

            try
            {
                // Get count before deletion
                int count = GetTotalCount();

                transaction = connection.BeginTransaction();

                using SqlCommand command =
                    new SqlCommand(
                        DeleteAllQuery, connection, transaction);

                command.ExecuteNonQuery();

                transaction.Commit();

                Console.WriteLine(
                    $"[DatabaseRepository] Deleted " +
                    $"{count} measurements.");

                return count;
            }
            catch (SqlException sqlEx)
            {
                RollbackTransaction(transaction);

                throw new DatabaseException(
                    "Failed to delete all measurements.",
                    DatabaseException.DatabaseErrorCodes.DELETE_FAILED,
                    sqlEx,
                    DeleteAllQuery,
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }
        /// <summary>
        /// Returns all error measurements from database.
        /// Uses parameterized query to filter HasError = true.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity> GetErrorMeasurements()
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                List<QuantityMeasurementEntity> measurements =
                    new List<QuantityMeasurementEntity>();

                string query =
                    @"SELECT Id, FirstOperand, SecondOperand,
                            OperationType, Result, HasError,
                            ErrorMessage, MeasurementType, Timestamp
                    FROM QuantityMeasurementEntity
                    WHERE HasError = 1
                    ORDER BY Timestamp DESC";

                using SqlCommand command =
                    new SqlCommand(query, connection);

                using SqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    measurements.Add(MapReaderToEntity(reader));
                }

                return measurements;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    "Failed to get error measurements.",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    "GetErrorMeasurements",
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Returns count of measurements by operation type.
        /// Uses parameterized query for SQL injection prevention.
        /// UC17
        /// </summary>
        public int GetCountByOperationType(string operationType)
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                string query =
                    @"SELECT COUNT(*)
                    FROM QuantityMeasurementEntity
                    WHERE OperationType = @OperationType";

                using SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue(
                    "@OperationType", operationType);

                object? result = command.ExecuteScalar();

                return result != null
                    ? Convert.ToInt32(result)
                    : 0;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    $"Failed to get count for: {operationType}",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    "GetCountByOperationType",
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        /// <summary>
        /// Returns measurements after specific date.
        /// Uses parameterized query for date filtering.
        /// UC17
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsAfterDate(DateTime date)
        {
            SqlConnection connection =
                _connectionPool.AcquireConnection();

            try
            {
                List<QuantityMeasurementEntity> measurements =
                    new List<QuantityMeasurementEntity>();

                string query =
                    @"SELECT Id, FirstOperand, SecondOperand,
                            OperationType, Result, HasError,
                            ErrorMessage, MeasurementType, Timestamp
                    FROM QuantityMeasurementEntity
                    WHERE Timestamp >= @Date
                    ORDER BY Timestamp DESC";

                using SqlCommand command =
                    new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Date", date);

                using SqlDataReader reader =
                    command.ExecuteReader();

                while (reader.Read())
                {
                    measurements.Add(MapReaderToEntity(reader));
                }

                return measurements;
            }
            catch (SqlException sqlEx)
            {
                throw new DatabaseException(
                    $"Failed to get measurements after: {date}",
                    DatabaseException.DatabaseErrorCodes.SELECT_FAILED,
                    sqlEx,
                    "GetMeasurementsAfterDate",
                    _databaseName);
            }
            finally
            {
                _connectionPool.ReleaseConnection(connection);
            }
        }

        // ─── UC16 Override Default Methods ───────────────────

        /// <summary>
        /// Returns connection pool statistics.
        /// Overrides default interface method.
        /// </summary>
        public string GetPoolStatistics()
        {
            return _connectionPool.GetPoolStatistics();
        }

        /// <summary>
        /// Releases all database connections back to pool.
        /// Called when application is shutting down.
        /// </summary>
        public void ReleaseResources()
        {
            _connectionPool.ReleaseAllResources();

            Console.WriteLine(
                "[DatabaseRepository] Resources released.");
        }

        // ─── Private Helper Methods ───────────────────────────

        /// <summary>
        /// Maps SqlDataReader row to QuantityMeasurementEntity.
        /// Called for each row returned from database.
        /// </summary>
        private QuantityMeasurementEntity MapReaderToEntity(
            SqlDataReader reader)
        {
            return new QuantityMeasurementEntity
            {
                Id            = reader["Id"].ToString()!,
                FirstOperand  = reader["FirstOperand"] == DBNull.Value
                                ? null
                                : reader["FirstOperand"].ToString(),
                SecondOperand = reader["SecondOperand"] == DBNull.Value
                                ? null
                                : reader["SecondOperand"].ToString(),
                OperationType = reader["OperationType"].ToString()!,
                Result        = reader["Result"] == DBNull.Value
                                ? null
                                : reader["Result"].ToString(),
                HasError      = Convert.ToBoolean(reader["HasError"]),
                ErrorMessage  = reader["ErrorMessage"] == DBNull.Value
                                ? null
                                : reader["ErrorMessage"].ToString(),
                MeasurementType = reader["MeasurementType"] == DBNull.Value
                                ? null
                                : reader["MeasurementType"].ToString(),
                Timestamp     = Convert.ToDateTime(reader["Timestamp"])
            };
        }

        /// <summary>
        /// Adds entity properties as parameterized SQL parameters.
        /// Prevents SQL injection by separating data from query.
        /// </summary>
        private void AddEntityParameters(
            SqlCommand command,
            QuantityMeasurementEntity entity)
        {
            command.Parameters.AddWithValue(
                "@Id",            entity.Id);
            command.Parameters.AddWithValue(
                "@FirstOperand",  (object?)entity.FirstOperand
                                  ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@SecondOperand", (object?)entity.SecondOperand
                                  ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@OperationType", entity.OperationType);
            command.Parameters.AddWithValue(
                "@Result",        (object?)entity.Result
                                  ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@HasError",      entity.HasError);
            command.Parameters.AddWithValue(
                "@ErrorMessage",  (object?)entity.ErrorMessage
                                  ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@MeasurementType", (object?)entity.MeasurementType
                                  ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@Timestamp",     entity.Timestamp);
        }

        /// <summary>
        /// Adds entity properties as parameters for History table.
        /// Uses EntityId instead of Id for history table.
        /// Prevents SQL injection by separating data from query.
        /// UC16
        /// </summary>
        private void AddHistoryParameters(
            SqlCommand command,
            QuantityMeasurementEntity entity)
        {
            // History table uses EntityId to reference entity
            command.Parameters.AddWithValue(
                "@EntityId",      entity.Id);
            command.Parameters.AddWithValue(
                "@FirstOperand",  (object?)entity.FirstOperand
                                ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@SecondOperand", (object?)entity.SecondOperand
                                ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@OperationType", entity.OperationType);
            command.Parameters.AddWithValue(
                "@Result",        (object?)entity.Result
                                ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@HasError",      entity.HasError);
            command.Parameters.AddWithValue(
                "@ErrorMessage",  (object?)entity.ErrorMessage
                                ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@MeasurementType", (object?)entity.MeasurementType
                                ?? DBNull.Value);
            command.Parameters.AddWithValue(
                "@Timestamp",     entity.Timestamp);
        }

        /// <summary>
        /// Safely rolls back a transaction.
        /// Called when an error occurs during database operation.
        /// </summary>
        private void RollbackTransaction(SqlTransaction? transaction)
        {
            try
            {
                transaction?.Rollback();
                Console.WriteLine(
                    "[DatabaseRepository] Transaction rolled back.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[DatabaseRepository] Rollback failed: " +
                    $"{ex.Message}");
            }
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return $"QuantityMeasurementDatabaseRepository " +
                   $"[Database: {_databaseName}, " +
                   $"{GetPoolStatistics()}]";
        }

        /// <summary>
        /// Returns measurements filtered by UserId from database.
        /// UC19
        /// </summary>
        public List<QuantityMeasurementEntity>
            GetMeasurementsByUserId(int userId)
        {
            return GetAllMeasurements()
                .Where(e => e.UserId == userId)
                .ToList();
        }
    }
}