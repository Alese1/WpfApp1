using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
     
            public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text;
            string password = txtPassword.Password;
            string role = ((System.Windows.Controls.ComboBoxItem)cmbRole.SelectedItem).Content.ToString();

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                lblError.Text = "Введите логин и пароль";
                lblError.Visibility = Visibility.Visible;
                return;
            }

            // Простая проверка (в реальном приложении нужно использовать хеширование)
            if (login == "user" && password == "user" && role == "Пользователь")
            {
                UserWindow userWindow = new UserWindow();
                userWindow.Show();
                this.Close();
            }
            else if (login == "manager" && password == "manager" && role == "Менеджер")
            {
                ManagerWindow managerWindow = new ManagerWindow();
                managerWindow.Show();
                this.Close();
            }
            else
            {
                lblError.Text = "Неверные учетные данные";
                lblError.Visibility = Visibility.Visible;
            }
        }
    }
}