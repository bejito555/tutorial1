using System;
using System.Data.SQLite;
using System.IO;

namespace CustomerManagementApp
{
    public static class DatabaseHelper
    {
        private static string databaseFile = "customers.db";

        public static void InitializeDatabase()
        {
            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
            }
            CreateCustomersTable();
            CreateOrdersTable(); // Nếu cần tạo bảng Orders
        }

        private static void CreateCustomersTable()
        {
            string connectionString = $"Data Source={databaseFile};Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Customers (
                        SerialNumber INTEGER PRIMARY KEY AUTOINCREMENT,
                        ID TEXT NOT NULL,
                        Name TEXT NOT NULL,
                        Phone TEXT NOT NULL,
                        BirthYear TEXT NOT NULL,
                        Gender TEXT NOT NULL
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        private static void CreateOrdersTable()
        {
            string connectionString = $"Data Source={databaseFile};Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        OrderID TEXT PRIMARY KEY,
                        CustomerID TEXT NOT NULL,
                        ProductCode TEXT NOT NULL,
                        Weight REAL NOT NULL,
                        OrderDate TEXT NOT NULL
                    );";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
