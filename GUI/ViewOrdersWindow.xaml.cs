using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace CustomerManagementApp
{
    public partial class ViewOrdersWindow : Window
    {
        public ViewOrdersWindow()
        {
            InitializeComponent();
        }

        private void ViewOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            string selectedMode = ((ComboBoxItem)cbViewMode.SelectedItem)?.Content.ToString();
            string input = txtInput.Text;

            var orders = new List<string>();

            switch (selectedMode)
            {
                case "Xem Tất Cả Đơn Hàng":
                    orders = GetAllOrders();
                    break;

                case "Xem Theo Ngày":
                    if (DateTime.TryParse(input, out DateTime orderDate))
                    {
                        orders = GetOrdersByDate(orderDate);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập ngày hợp lệ.");
                        return;
                    }
                    break;

                case "Xem Theo ID Khách Hàng":
                    if (!string.IsNullOrEmpty(input))
                    {
                        orders = GetOrdersByCustomerId(input);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập ID khách hàng.");
                        return;
                    }
                    break;

                default:
                    MessageBox.Show("Vui lòng chọn chế độ xem.");
                    return;
            }

            OrdersListBox.Items.Clear();
            foreach (var order in orders)
            {
                OrdersListBox.Items.Add(order);
            }
        }

        private List<string> GetAllOrders()
        {
            var orders = new List<string>();
            string connectionString = "Data Source=customers.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT OrderID, ProductCode, Weight, OrderDate FROM Orders";

                using (var command = new SQLiteCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string orderId = reader["OrderID"].ToString();
                            string productCode = reader["ProductCode"].ToString();
                            string weight = reader["Weight"].ToString();
                            string orderDate = reader["OrderDate"].ToString();

                            orders.Add($"Mã Đơn Hàng: {orderId}, Mã Sản Phẩm: {productCode}, Số Kg: {weight}, Ngày Tạo: {orderDate}");
                        }
                    }
                }
            }

            return orders;
        }

        private List<string> GetOrdersByDate(DateTime orderDate)
        {
            var orders = new List<string>();
            string connectionString = "Data Source=customers.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT OrderID, ProductCode, Weight, OrderDate
                    FROM Orders
                    WHERE DATE(OrderDate) = DATE(@OrderDate)";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderDate", orderDate.ToString("yyyy-MM-dd"));
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string orderId = reader["OrderID"].ToString();
                            string productCode = reader["ProductCode"].ToString();
                            string weight = reader["Weight"].ToString();
                            string orderDateStr = reader["OrderDate"].ToString();

                            orders.Add($"Mã Đơn Hàng: {orderId}, Mã Sản Phẩm: {productCode}, Số Kg: {weight}, Ngày Tạo: {orderDateStr}");
                        }
                    }
                }
            }

            return orders;
        }

        private List<string> GetOrdersByCustomerId(string customerId)
        {
            var orders = new List<string>();
            string connectionString = "Data Source=customers.db;Version=3;";

            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = @"
                    SELECT OrderID, ProductCode, Weight, OrderDate
                    FROM Orders
                    WHERE CustomerID = @CustomerID";

                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CustomerID", customerId);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string orderId = reader["OrderID"].ToString();
                            string productCode = reader["ProductCode"].ToString();
                            string weight = reader["Weight"].ToString();
                            string orderDate = reader["OrderDate"].ToString();

                            orders.Add($"Mã Đơn Hàng: {orderId}, Mã Sản Phẩm: {productCode}, Số Kg: {weight}, Ngày Tạo: {orderDate}");
                        }
                    }
                }
            }

            return orders;
        }

        private void ExportToTextFile_Click(object sender, RoutedEventArgs e)
        {
            string selectedMode = ((ComboBoxItem)cbViewMode.SelectedItem)?.Content.ToString();
            string input = txtInput.Text;

            var orders = new List<string>();

            switch (selectedMode)
            {
                case "Xem Tất Cả Đơn Hàng":
                    orders = GetAllOrders();
                    break;

                case "Xem Theo Ngày":
                    if (DateTime.TryParse(input, out DateTime orderDate))
                    {
                        orders = GetOrdersByDate(orderDate);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập ngày hợp lệ.");
                        return;
                    }
                    break;

                case "Xem Theo ID Khách Hàng":
                    if (!string.IsNullOrEmpty(input))
                    {
                        orders = GetOrdersByCustomerId(input);
                    }
                    else
                    {
                        MessageBox.Show("Vui lòng nhập ID khách hàng.");
                        return;
                    }
                    break;

                default:
                    MessageBox.Show("Vui lòng chọn chế độ xem.");
                    return;
            }

            string fileName = $"Orders_{DateTime.Now.ToString("yyyyMMddHHmmss")}.txt";
            string filePath = Path.Combine(@"E:\OJT\GUI\RS", fileName);

            try
            {
                File.WriteAllLines(filePath, orders);
                MessageBox.Show($"Dữ liệu đã được xuất thành công tại {filePath}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xuất file: {ex.Message}");
            }
        }

        private void cbViewMode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Không cần thực hiện hành động gì ở đây cho chức năng hiện tại
        }
    }
}
