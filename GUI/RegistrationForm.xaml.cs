using System;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;

namespace CustomerManagementApp
{
    public partial class RegistrationForm : Window
    {
        public RegistrationForm()
        {
            InitializeComponent();
        }

        private void SubmitForm_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text;
            string phone = txtPhone.Text;
            string birthYear = txtBirthYear.Text;
            string gender = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone) || string.IsNullOrEmpty(birthYear) || string.IsNullOrEmpty(gender))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            string newId = GenerateNewId();
            try
            {
                using (SQLiteConnection conn = new SQLiteConnection("Data Source=customers.db;Version=3;"))
                {
                    conn.Open();
                    string query = "INSERT INTO Customers (ID, Name, Phone, BirthYear, Gender) VALUES (@ID, @Name, @Phone, @BirthYear, @Gender)";
                    using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@ID", newId);
                        cmd.Parameters.AddWithValue("@Name", name);
                        cmd.Parameters.AddWithValue("@Phone", phone);
                        cmd.Parameters.AddWithValue("@BirthYear", birthYear);
                        cmd.Parameters.AddWithValue("@Gender", gender);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show($"Đăng ký khách hàng thành công! Mã khách hàng là: {newId}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Có lỗi xảy ra: " + ex.Message);
            }
        }

        private string GenerateNewId()
        {
            string newId = "NT001";
            using (SQLiteConnection conn = new SQLiteConnection("Data Source=customers.db;Version=3;"))
            {
                conn.Open();
                string query = "SELECT MAX(ID) FROM Customers";
                using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
                {
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value && result != null)
                    {
                        string maxId = result.ToString();
                        int idNumber = int.Parse(maxId.Substring(2)) + 1; // Lấy số từ ID và tăng lên
                        newId = "80" + idNumber.ToString("D3"); // Định dạng số thành ba chữ số
                    }
                }
            }
            return newId;
        }
    }
}
