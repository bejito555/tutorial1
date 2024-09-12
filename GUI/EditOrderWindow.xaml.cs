using System;
using System.Data.SQLite;
using System.Windows;

namespace CustomerManagementApp
{
    public partial class EditOrderWindow : Window
    {
        private string _orderId;

        public EditOrderWindow(string orderId)
        {
            InitializeComponent();
            _orderId = orderId;
            LoadOrderData();
        }

        private void LoadOrderData()
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ProductCode, Weight FROM Orders WHERE OrderID = @OrderID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@OrderID", _orderId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtOrderId.Text = _orderId;
                            txtProductCode.Text = reader["ProductCode"].ToString();
                            txtWeight.Text = reader["Weight"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Đơn hàng không tồn tại.");
                            this.Close();
                        }
                    }
                }
            }
        }

        private void UpdateOrderButton_Click(object sender, RoutedEventArgs e)
        {
            string newProductCode = txtProductCode.Text;
            if (!double.TryParse(txtWeight.Text, out double newWeight))
            {
                MessageBox.Show("Cân nặng không hợp lệ!");
                return;
            }

            UpdateOrder(_orderId, newProductCode, newWeight);
        }

        private void UpdateOrder(string orderId, string newProductCode, double newWeight)
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Orders SET ProductCode = @NewProductCode, Weight = @NewWeight WHERE OrderID = @OrderID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewProductCode", newProductCode);
                    command.Parameters.AddWithValue("@NewWeight", newWeight);
                    command.Parameters.AddWithValue("@OrderID", orderId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thông tin đơn hàng đã được cập nhật!");
                    }
                    else
                    {
                        MessageBox.Show("Đơn hàng không tồn tại.");
                    }
                }
            }
        }
    }
}
