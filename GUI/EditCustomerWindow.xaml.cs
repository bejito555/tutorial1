using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace CustomerManagementApp
{
    public partial class EditCustomerWindow : Window
    {
        private string _customerId;

        public EditCustomerWindow(string customerId)
        {
            InitializeComponent();
            _customerId = customerId;
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Name, Phone, BirthYear, Gender FROM Customers WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", _customerId);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtCustomerId.Text = _customerId;
                            txtCustomerName.Text = reader["Name"].ToString();
                            txtPhone.Text = reader["Phone"].ToString();
                            txtBirthYear.Text = reader["BirthYear"].ToString();
                            cmbGender.SelectedItem = reader["Gender"].ToString();
                        }
                        else
                        {
                            MessageBox.Show("Khách hàng không tồn tại.");
                            this.Close();
                        }
                    }
                }
            }
        }

        private void UpdateCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            string newCustomerName = txtCustomerName.Text;
            string newPhone = txtPhone.Text;
            string newBirthYear = txtBirthYear.Text;
            string newGender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(newCustomerName) || string.IsNullOrEmpty(newPhone) || string.IsNullOrEmpty(newBirthYear) || string.IsNullOrEmpty(newGender))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            UpdateCustomer(_customerId, newCustomerName, newPhone, newBirthYear, newGender);
        }

        private void UpdateCustomer(string customerId, string newCustomerName, string newPhone, string newBirthYear, string newGender)
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Customers SET Name = @NewName, Phone = @NewPhone, BirthYear = @NewBirthYear, Gender = @NewGender WHERE ID = @ID";
                using (var command = new SQLiteCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@NewName", newCustomerName);
                    command.Parameters.AddWithValue("@NewPhone", newPhone);
                    command.Parameters.AddWithValue("@NewBirthYear", newBirthYear);
                    command.Parameters.AddWithValue("@NewGender", newGender);
                    command.Parameters.AddWithValue("@ID", customerId);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thông tin khách hàng đã được cập nhật!");
                    }
                    else
                    {
                        MessageBox.Show("Khách hàng không tồn tại.");
                    }
                }
            }
        }
    }
}
