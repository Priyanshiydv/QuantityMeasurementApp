using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace QuantityMeasurement.Repository.Utilities
{
    /// <summary>
    /// Manages a pool of reusable database connections.
    /// Reduces overhead of creating and closing connections frequently.
    /// Thread-safe implementation using lock mechanism.
    /// UC16
    /// </summary>
    public class ConnectionPool
    {
        // ─── Singleton Instance ───────────────────────────────

        private static ConnectionPool? instance;
        private static readonly object lockObject = new object();

        // ─── Pool Fields ──────────────────────────────────────

        /// <summary>
        /// List of available connections in the pool.
        /// </summary>
        private readonly List<SqlConnection> availableConnections;

        /// <summary>
        /// List of connections currently in use.
        /// </summary>
        private readonly List<SqlConnection> usedConnections;

        /// <summary>
        /// Database connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Maximum number of connections allowed in pool.
        /// </summary>
        private readonly int maxPoolSize;

        /// <summary>
        /// Minimum number of connections to maintain in pool.
        /// </summary>
        private readonly int minPoolSize;

        /// <summary>
        /// Connection timeout in seconds.
        /// </summary>
        private readonly int connectionTimeout;

        // ─── Private Constructor (Singleton) ──────────────────

        /// <summary>
        /// Private constructor to enforce Singleton pattern.
        /// Initializes connection pool with minimum connections.
        /// </summary>
        private ConnectionPool(
            string connectionString,
            int minPoolSize,
            int maxPoolSize,
            int connectionTimeout)
        {
            // Store configuration
            this.connectionString  = connectionString;
            this.minPoolSize       = minPoolSize;
            this.maxPoolSize       = maxPoolSize;
            this.connectionTimeout = connectionTimeout;

            // Initialize connection lists
            availableConnections = new List<SqlConnection>();
            usedConnections      = new List<SqlConnection>();

            // Create minimum connections on startup
            InitializePool();

            Console.WriteLine(
                $"[ConnectionPool] Initialized with " +
                $"{minPoolSize} connections. " +
                $"Max: {maxPoolSize}");
        }

        // ─── Singleton Access ─────────────────────────────────

        /// <summary>
        /// Returns the single instance of ConnectionPool.
        /// Creates instance using ApplicationConfig settings.
        /// Thread-safe Singleton implementation.
        /// </summary>
        public static ConnectionPool GetInstance()
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        // Load config from ApplicationConfig
                        ApplicationConfig config =
                            ApplicationConfig.GetInstance();

                        instance = new ConnectionPool(
                            config.GetProductionConnectionString(),
                            config.GetMinPoolSize(),
                            config.GetMaxPoolSize(),
                            config.GetConnectionTimeout()
                        );
                    }
                }
            }
            return instance;
        }

        /// <summary>
        /// Returns instance using custom connection string.
        /// Used for test database connections.
        /// </summary>
        public static ConnectionPool GetInstance(
            string connectionString,
            int minPoolSize  = 2,
            int maxPoolSize  = 5,
            int timeout      = 30)
        {
            if (instance == null)
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        instance = new ConnectionPool(
                            connectionString,
                            minPoolSize,
                            maxPoolSize,
                            timeout
                        );
                    }
                }
            }
            return instance;
        }

        // ─── Pool Operations ──────────────────────────────────

        /// <summary>
        /// Acquires a connection from the pool.
        /// Creates new connection if pool not exhausted.
        /// Throws exception if max pool size reached.
        /// </summary>
        public SqlConnection AcquireConnection()
        {
            lock (lockObject)
            {
                // Check if available connection exists
                if (availableConnections.Count > 0)
                {
                    // Get first available connection
                    SqlConnection connection =
                        availableConnections[0];

                    availableConnections.RemoveAt(0);

                    // Reopen if connection was closed
                    if (connection.State !=
                        System.Data.ConnectionState.Open)
                    {
                        connection.Open();
                    }

                    usedConnections.Add(connection);

                    Console.WriteLine(
                        $"[ConnectionPool] Connection acquired. " +
                        $"Available: {availableConnections.Count}, " +
                        $"Used: {usedConnections.Count}");

                    return connection;
                }

                // Create new connection if max not reached
                int totalConnections =
                    availableConnections.Count +
                    usedConnections.Count;

                if (totalConnections < maxPoolSize)
                {
                    SqlConnection newConnection =
                        CreateNewConnection();

                    usedConnections.Add(newConnection);

                    Console.WriteLine(
                        $"[ConnectionPool] New connection created. " +
                        $"Total: {totalConnections + 1}");

                    return newConnection;
                }

                // Pool exhausted - throw exception
                throw new InvalidOperationException(
                    $"Connection pool exhausted. " +
                    $"Max pool size: {maxPoolSize}. " +
                    $"All connections in use.");
            }
        }

        /// <summary>
        /// Releases a connection back to the pool.
        /// Connection becomes available for reuse.
        /// </summary>
        public void ReleaseConnection(SqlConnection connection)
        {
            lock (lockObject)
            {
                if (connection == null)
                    return;

                // Move from used to available
                if (usedConnections.Contains(connection))
                {
                    usedConnections.Remove(connection);
                    availableConnections.Add(connection);

                    Console.WriteLine(
                        $"[ConnectionPool] Connection released. " +
                        $"Available: {availableConnections.Count}, " +
                        $"Used: {usedConnections.Count}");
                }
            }
        }

        // ─── Pool Statistics ──────────────────────────────────

        /// <summary>
        /// Returns current pool statistics.
        /// Shows available, used and total connections.
        /// </summary>
        public string GetPoolStatistics()
        {
            lock (lockObject)
            {
                int total = availableConnections.Count +
                            usedConnections.Count;

                return $"ConnectionPool Statistics: " +
                       $"[Available: {availableConnections.Count}, " +
                       $"Used: {usedConnections.Count}, " +
                       $"Total: {total}, " +
                       $"MaxSize: {maxPoolSize}]";
            }
        }

        /// <summary>
        /// Returns number of available connections.
        /// </summary>
        public int GetAvailableCount()
        {
            lock (lockObject)
            {
                return availableConnections.Count;
            }
        }

        /// <summary>
        /// Returns number of connections currently in use.
        /// </summary>
        public int GetUsedCount()
        {
            lock (lockObject)
            {
                return usedConnections.Count;
            }
        }

        // ─── Resource Cleanup ─────────────────────────────────

        /// <summary>
        /// Closes and disposes all connections in the pool.
        /// Called when application is shutting down.
        /// </summary>
        public void ReleaseAllResources()
        {
            lock (lockObject)
            {
                // Close all available connections
                foreach (SqlConnection connection
                         in availableConnections)
                {
                    CloseConnection(connection);
                }

                // Close all used connections
                foreach (SqlConnection connection
                         in usedConnections)
                {
                    CloseConnection(connection);
                }

                availableConnections.Clear();
                usedConnections.Clear();

                // Reset singleton for fresh start
                instance = null;

                Console.WriteLine(
                    "[ConnectionPool] All resources released.");
            }
        }

        // ─── Private Helper Methods ───────────────────────────

        /// <summary>
        /// Initializes pool with minimum number of connections.
        /// Called once during pool creation.
        /// </summary>
        private void InitializePool()
        {
            for (int i = 0; i < minPoolSize; i++)
            {
                SqlConnection connection = CreateNewConnection();
                availableConnections.Add(connection);
            }
        }

        /// <summary>
        /// Creates a new open SQL connection.
        /// </summary>
        private SqlConnection CreateNewConnection()
        {
            SqlConnection connection =
                new SqlConnection(connectionString);

            connection.Open();
            return connection;
        }

        /// <summary>
        /// Safely closes and disposes a SQL connection.
        /// </summary>
        private void CloseConnection(SqlConnection connection)
        {
            try
            {
                if (connection.State ==
                    System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
                connection.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(
                    $"[ConnectionPool] Warning: " +
                    $"Error closing connection: {ex.Message}");
            }
        }

        // ─── ToString ─────────────────────────────────────────

        public override string ToString()
        {
            return GetPoolStatistics();
        }
    }
}