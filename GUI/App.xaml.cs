using System.Windows;

namespace CustomerManagementApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Tạo cơ sở dữ liệu và bảng nếu chưa tồn tại
            DatabaseHelper.InitializeDatabase();

            // Mở cửa sổ đăng nhập
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
