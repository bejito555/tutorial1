using System.Windows;

namespace CustomerManagementApp
{
    public partial class ManageOrderWindow : Window
    {
        public ManageOrderWindow()
        {
            InitializeComponent();
        }

        private void EditOrder_Click(object sender, RoutedEventArgs e)
        {
            string orderId = txtOrderId.Text;

            if (string.IsNullOrEmpty(orderId))
            {
                MessageBox.Show("Vui lòng nhập ID đơn hàng.");
                return;
            }

            var editOrderWindow = new EditOrderWindow(orderId); // Truyền ID đơn hàng qua cửa sổ chỉnh sửa
            editOrderWindow.Show();
        }
    }
}
