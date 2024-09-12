using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Windows;
using Microsoft.Win32;

namespace CustomerManagementApp
{
    public partial class CustomerListForm : Window
    {
        public CustomerListForm()
        {
            InitializeComponent();
            LoadCustomerData();
        }

        private void LoadCustomerData()
        {
            string connectionString = "Data Source=customers.db;Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Customers";
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(query, connection))
                {
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    CustomerDataGrid.ItemsSource = dataTable.DefaultView;
                }
            }
        }

        private void ExportToTextFile_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text files (*.txt)|*.txt",
                Title = "Chọn Địa Chỉ Lưu File"
            };

            if (saveFileDialog.ShowDialog() == true)
            {
                string filePath = saveFileDialog.FileName;

                try
                {
                    if (CustomerDataGrid.Items.Count > 0)
                    {
                        using (StreamWriter writer = new StreamWriter(filePath))
                        {
                            DataTable dataTable = (DataTable)((DataView)CustomerDataGrid.ItemsSource).Table;
                            foreach (DataColumn column in dataTable.Columns)
                            {
                                writer.Write(column.ColumnName + "\t");
                            }
                            writer.WriteLine();

                            foreach (DataRow row in dataTable.Rows)
                            {
                                foreach (var item in row.ItemArray)
                                {
                                    writer.Write(item.ToString() + "\t");
                                }
                                writer.WriteLine();
                            }
                        }
                        MessageBox.Show($"Dữ liệu đã được xuất thành công vào file: {filePath}", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Information);
                        PushFileToGitHub(filePath);
                    }
                    else
                    {
                        MessageBox.Show("Không có dữ liệu để xuất!", "Thông Báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xuất dữ liệu: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        private void PushFileToGitHub(string filePath)
        {
            // Define the Git commands
            string gitAdd = "git add .";
            string gitCommit = $"git commit -m \"Add exported file {Path.GetFileName(filePath)}\"";
            string gitPush = "git push origin main";

            // Set the working directory to your local git repository path
            string repoPath = @"C:\path\to\your\local\repo";

            // Execute each git command
            ExecuteGitCommand(gitAdd, repoPath);
            ExecuteGitCommand(gitCommit, repoPath);
            ExecuteGitCommand(gitPush, repoPath);
        }

        private void ExecuteGitCommand(string command, string workingDirectory)
        {
            ProcessStartInfo processInfo = new ProcessStartInfo("cmd.exe", "/c " + command)
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true
            };

            using (Process process = Process.Start(processInfo))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    Console.WriteLine(result);
                }
                using (StreamReader error = process.StandardError)
                {
                    string result = error.ReadToEnd();
                    if (!string.IsNullOrEmpty(result))
                    {
                        MessageBox.Show($"Git Error: {result}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
    }
}

