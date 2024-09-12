using System.Windows;

namespace CustomerManagementApp
{
    public partial class ManageCustomerWindow : Window
    {
        public ManageCustomerWindow()
        {
            InitializeComponent();
        }

        private void EditCustomer_Click(object sender, RoutedEventArgs e)
        {
            string customerId = txtCustomerId.Text;

            if (string.IsNullOrEmpty(customerId))
            {
                MessageBox.Show("Vui lòng nhập ID khách hàng.");
                return;
            }

            var editCustomerWindow = new EditCustomerWindow(customerId); // Truyền ID khách hàng qua cửa sổ chỉnh sửa
            editCustomerWindow.Show();
        }
    }
}
