using System.Windows;

namespace CustomerManagementApp
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // Kiểm tra thông tin đăng nhập (ví dụ: kiểm tra tài khoản và mật khẩu)
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (username == "mintatran" && password == "010103") // Điều kiện đăng nhập thành công
            {
                // Mở cửa sổ chính
                var mainWindow = new MainWindow();
                mainWindow.Show();

                // Đóng cửa sổ đăng nhập
                this.Close();
            }
            else
            {
                MessageBox.Show("Tài khoản hoặc mật khẩu không chính xác!");
            }
        }

        private void UsernameTextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
