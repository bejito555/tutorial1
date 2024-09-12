using System.Windows;

namespace CustomerManagementApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OpenRegistrationForm_Click(object sender, RoutedEventArgs e)
        {
            var registrationForm = new RegistrationForm();
            registrationForm.Show();
        }

        private void OpenCustomerListForm_Click(object sender, RoutedEventArgs e)
        {
            var customerListForm = new CustomerListForm();
            customerListForm.Show();
        }

        private void OpenAddOrderWindow_Click(object sender, RoutedEventArgs e)
        {
            var addOrderWindow = new AddOrderWindow();
            addOrderWindow.Show();
        }

        private void OpenViewOrdersWindow_Click(object sender, RoutedEventArgs e)
        {
            var viewOrdersWindow = new ViewOrdersWindow();
            viewOrdersWindow.Show();
        }

        private void OpenManageCustomerWindow_Click(object sender, RoutedEventArgs e)
        {
            var manageCustomerWindow = new ManageCustomerWindow();
            manageCustomerWindow.Show();
        }

        private void OpenManageOrderWindow_Click(object sender, RoutedEventArgs e)
        {
            var manageOrderWindow = new ManageOrderWindow();
            manageOrderWindow.Show();
        }
    }
}
