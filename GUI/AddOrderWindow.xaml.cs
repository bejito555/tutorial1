using System;
using System.Data.SQLite;
using System.Windows;

namespace CustomerManagementApp
{
    public partial class AddOrderWindow : Window
    {
        public AddOrderWindow()
        {
            InitializeComponent();
        }

        private void AddOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string customerId = txtCustomerId.Text;
            string productCode = txtProductCode.Text;
            if (!double.TryParse(txtWeight.Text, out double weight))
            {
                MessageBox.Show("Số Kg không hợp lệ!");
                return;
            }

            // Generate Order ID
            string orderId = GenerateOrderId(customerId);

            // Retrieve customer name
            string customerName = GetCustomerName(customerId);

            if (customerName == null)
            {
                MessageBox.Show("ID khách hàng không tồn tại!");
                return;
            }

            // Save order to database
            SaveOrder(orderId, customerId, productCode, weight);

            // Display order details
            MessageBox.Show($"Đơn hàng đã được tạo:\nMã Đơn Hàng: {orderId}\nThời Gian Tạo: {DateTime.Now}\nTên Khách Hàng: {customerName}\nSố Kg: {weight}");
        }

        private string GenerateOrderId(string customerId)
        {
            // Get the highest existing order number (ID) for the format ID###
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                // Query to get the maximum order number for all orders
                string query = @"
            SELECT MAX(SUBSTR(OrderID, 3, 3)) FROM Orders 
            WHERE OrderID LIKE 'ID%_'";

                using (var command = new SQLiteCommand(query, connection))
                {
                    var result = command.ExecuteScalar();

                    int nextOrderNumber = 1;
                    if (result != DBNull.Value)
                    {
                        // Extract the number and increase it
                        if (int.TryParse(result.ToString(), out int orderNumber))
                        {
                            nextOrderNumber = orderNumber + 1;
                        }
                    }

                    // Return the new OrderID in the format ID###_CustomerID
                    return $"ID{nextOrderNumber:D3}_{customerId}";
                }
            }
        }


        private string GetCustomerName(string customerId)
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Name FROM Customers WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", customerId);
                    var result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        private void SaveOrder(string orderId, string customerId, string productCode, double weight)
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    CREATE TABLE IF NOT EXISTS Orders (
                        OrderID TEXT PRIMARY KEY,
                        CustomerID TEXT NOT NULL,
                        ProductCode TEXT NOT NULL,
                        Weight REAL NOT NULL,
                        OrderDate TEXT NOT NULL
                    );";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.ExecuteNonQuery();
                }

                query = "INSERT INTO Orders (OrderID, CustomerID, ProductCode, Weight, OrderDate) VALUES (@OrderID, @CustomerID, @ProductCode, @Weight, @OrderDate)";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", orderId);
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    command.Parameters.AddWithValue("@ProductCode", productCode);
                    command.Parameters.AddWithValue("@Weight", weight);
                    command.Parameters.AddWithValue("@OrderDate", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
