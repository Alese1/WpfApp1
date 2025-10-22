using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для User.xaml
    /// </summary>
    public partial class User : Window
    {
        public UserWindow()
        {
            InitializeComponent();
            LoadUsersData();
            LoadTransferHistory();
        }
        public class User
        {
            public string FullName { get; set; }
            public string Login { get; set; }
            public string Status { get; set; }
            public string Balance { get; set; }
        }

        // Класс для хранения истории переводов
        public class TransferHistory
        {
            public string Date { get; set; }
            public string Recipient { get; set; }
            public string Amount { get; set; }
            public string Status { get; set; }
        }

        // Списки данных
        private List<User> usersList = new List<User>();
        private List<TransferHistory> transferHistory = new List<TransferHistory>();

        // Загрузка тестовых данных пользователей
        private void LoadUsersData()
        {
            var users = new List<User>
    {
        new User { FullName = "Иванов Иван Иванович", Login = "ivanov", Status = "Активен", Balance = 1500.50m },
        new User { FullName = "Петрова Мария Сергеевна", Login = "petrova", Status = "Активен", Balance = 2300.75m },
        new User { FullName = "Сидоров Алексей Петрович", Login = "sidorov", Status = "Заблокирован", Balance = 500.00m }
    };

            dgUsers.ItemsSource = users;
        }

        // Загрузка истории переводов
        private void LoadTransferHistory()
        {
            transferHistory = new List<TransferHistory>
        {
            new TransferHistory { Date = "15.01.2024", Recipient = "Петров П.П.", Amount = "5 000 ₽", Status = "Успешно" },
            new TransferHistory { Date = "14.01.2024", Recipient = "Иванов И.И.", Amount = "2 300 ₽", Status = "Успешно" },
            new TransferHistory { Date = "10.01.2024", Recipient = "Сидорова А.В.", Amount = "1 500 ₽", Status = "Отклонен" },
            new TransferHistory { Date = "05.01.2024", Recipient = "Козлова М.С.", Amount = "7 800 ₽", Status = "Успешно" }
        };
        }

        // === ФУНКЦИОНАЛ БЫСТРЫХ ОПЕРАЦИЙ ===

        // Кнопка "Перевод" - открывает окно перевода
        private void btnTransfer_Click(object sender, RoutedEventArgs e)
        {
            OpenTransferWindow();
        }

        // Кнопка "Оплата" - показывает меню выбора услуг
        private void btnPayment_Click(object sender, RoutedEventArgs e)
        {
            ShowPaymentOptions();
        }

        // Кнопка "Пополнить" - открывает окно пополнения счета
        private void btnDeposit_Click(object sender, RoutedEventArgs e)
        {
            OpenDepositWindow();
        }

        // === РЕАЛИЗАЦИЯ ПЕРЕВОДА ===
        private void OpenTransferWindow()
        {
            Window transferWindow = new Window
            {
                Title = "Перевод средств",
                Width = 400,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            // Выбор получателя
            ComboBox cmbRecipient = new ComboBox
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10),
                DisplayMemberPath = "FullName"
            };
            cmbRecipient.ItemsSource = usersList;

            // Сумма перевода
            TextBox txtAmount = new TextBox
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10),
                ToolTip = "Введите сумму перевода"
            };

            // Комментарий
            TextBox txtComment = new TextBox
            {
                Height = 60,
                Margin = new Thickness(0, 0, 0, 15),
                TextWrapping = TextWrapping.Wrap,
                ToolTip = "Комментарий к переводу"
            };

            // Кнопка перевода
            Button btnConfirm = new Button
            {
                Content = "Выполнить перевод",
                Background = System.Windows.Media.Brushes.DodgerBlue,
                Foreground = System.Windows.Media.Brushes.White,
                Height = 35,
                FontWeight = FontWeights.Bold
            };

            // Добавляем элементы
            panel.Children.Add(new TextBlock { Text = "Выберите получателя:" });
            panel.Children.Add(cmbRecipient);
            panel.Children.Add(new TextBlock { Text = "Сумма перевода:" });
            panel.Children.Add(txtAmount);
            panel.Children.Add(new TextBlock { Text = "Комментарий:" });
            panel.Children.Add(txtComment);
            panel.Children.Add(btnConfirm);

            // Обработчик кнопки
            btnConfirm.Click += (s, e) =>
            {
                if (cmbRecipient.SelectedItem == null)
                {
                    MessageBox.Show("Выберите получателя!", "Ошибка");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtAmount.Text) || !decimal.TryParse(txtAmount.Text, out decimal amount))
                {
                    MessageBox.Show("Введите корректную сумму!", "Ошибка");
                    return;
                }

                User recipient = (User)cmbRecipient.SelectedItem;

                // Добавляем в историю
                transferHistory.Add(new TransferHistory
                {
                    Date = DateTime.Now.ToString("dd.MM.yyyy"),
                    Recipient = recipient.FullName,
                    Amount = $"{amount} ₽",
                    Status = "Успешно"
                });

                MessageBox.Show($"Перевод {recipient.FullName} на сумму {amount}₽ выполнен!", "Успех");
                transferWindow.Close();

            };

            transferWindow.Content = panel;
            transferWindow.ShowDialog();
        }

        // === РЕАЛИЗАЦИЯ ОПЛАТЫ ===
        private void ShowPaymentOptions()
        {
            ContextMenu menu = new ContextMenu();

            // Создаем пункты меню
            string[] services = { "💻 Интернет", "📱 Мобильная связь", "🏠 Коммунальные услуги", "📺 Телевидение", "⚡ Электроэнергия" };

            foreach (string service in services)
            {
                MenuItem item = new MenuItem { Header = service };
                item.Click += (s, e) => OpenPaymentWindow(service);
                menu.Items.Add(item);
            }

            menu.PlacementTarget = btnPayment;
            menu.IsOpen = true;
        }

        private void OpenPaymentWindow(string serviceName)
        {
            Window paymentWindow = new Window
            {
                Title = $"Оплата {serviceName}",
                Width = 350,
                Height = 250,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            // Номер счета
            TextBox txtAccount = new TextBox
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "1234567890"
            };

            // Сумма
            TextBox txtAmount = new TextBox
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 15),
                Text = GetDefaultAmount(serviceName)
            };

            // Кнопка оплаты
            Button btnPay = new Button
            {
                Content = $"Оплатить {serviceName}",
                Background = System.Windows.Media.Brushes.Purple,
                Foreground = System.Windows.Media.Brushes.White,
                Height = 35
            };

            panel.Children.Add(new TextBlock { Text = "Номер лицевого счета:" });
            panel.Children.Add(txtAccount);
            panel.Children.Add(new TextBlock { Text = "Сумма оплаты:" });
            panel.Children.Add(txtAmount);
            panel.Children.Add(btnPay);

            btnPay.Click += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtAccount.Text))
                {
                    MessageBox.Show("Введите номер счета!", "Ошибка");
                    return;
                }

                if (!decimal.TryParse(txtAmount.Text, out decimal amount))
                {
                    MessageBox.Show("Введите корректную сумму!", "Ошибка");
                    return;
                }

                MessageBox.Show($"{serviceName} оплачен на сумму {amount}₽!\nСчет: {txtAccount.Text}", "Оплата выполнена");
                paymentWindow.Close();
            };

            paymentWindow.Content = panel;
            paymentWindow.ShowDialog();
        }

        // Сумма по умолчанию для разных услуг
        private string GetDefaultAmount(string service)
        {
            return service switch
            {
                "💻 Интернет" => "600",
                "📱 Мобильная связь" => "300",
                "🏠 Коммунальные услуги" => "4500",
                "📺 Телевидение" => "800",
                "⚡ Электроэнергия" => "1200",
                _ => "500"
            };
        }

        // === РЕАЛИЗАЦИЯ ПОПОЛНЕНИЯ ===
        private void OpenDepositWindow()
        {
            Window depositWindow = new Window
            {
                Title = "Пополнение счета",
                Width = 400,
                Height = 300,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            StackPanel panel = new StackPanel { Margin = new Thickness(20) };

            // Быстрые суммы
            TextBlock quickText = new TextBlock
            {
                Text = "Быстрые суммы:",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(0, 0, 0, 10)
            };

            WrapPanel quickPanel = new WrapPanel { Margin = new Thickness(0, 0, 0, 15) };

            string[] amounts = { "500", "1000", "2000", "5000" };
            TextBox amountBox = new TextBox
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 10),
                Text = "1000"
            };

            // Создаем кнопки быстрых сумм
            foreach (string amount in amounts)
            {
                Button btnAmount = new Button
                {
                    Content = $"{amount}₽",
                    Margin = new Thickness(0, 0, 5, 5),
                    Background = System.Windows.Media.Brushes.LightBlue
                };
                btnAmount.Click += (s, e) => amountBox.Text = amount;
                quickPanel.Children.Add(btnAmount);
            }

            // Способы пополнения
            ComboBox cmbMethod = new ComboBox
            {
                Height = 30,
                Margin = new Thickness(0, 0, 0, 15)
            };
            cmbMethod.Items.Add("💳 С банковской карты");
            cmbMethod.Items.Add("🏧 Через терминал");
            cmbMethod.Items.Add("📱 С мобильного счета");
            cmbMethod.SelectedIndex = 0;

            // Кнопка пополнения
            Button btnDeposit = new Button
            {
                Content = "Пополнить счет",
                Background = System.Windows.Media.Brushes.Green,
                Foreground = System.Windows.Media.Brushes.White,
                Height = 35,
                FontWeight = FontWeights.Bold
            };

            // Добавляем элементы
            panel.Children.Add(quickText);
            panel.Children.Add(quickPanel);
            panel.Children.Add(new TextBlock { Text = "Сумма пополнения:" });
            panel.Children.Add(amountBox);
            panel.Children.Add(new TextBlock { Text = "Способ пополнения:" });
            panel.Children.Add(cmbMethod);
            panel.Children.Add(btnDeposit);

            // Обработчик кнопки
            btnDeposit.Click += (s, e) =>
            {
                if (!decimal.TryParse(amountBox.Text, out decimal amount) || amount <= 0)
                {
                    MessageBox.Show("Введите корректную сумму!", "Ошибка");
                    return;
                }

                string method = cmbMethod.SelectedItem.ToString();
                MessageBox.Show($"Счет успешно пополнен на {amount}₽\nСпособ: {method}", "Пополнение выполнено");
                depositWindow.Close();
            };

            depositWindow.Content = panel;
            depositWindow.ShowDialog();
        }

        // === ОБРАБОТЧИК ВЫБОРА В ТАБЛИЦЕ ===
        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User selectedUser)
            {
                MessageBox.Show(
                    $"Выбран пользователь:\n\n" +
                    $"ФИО: {selectedUser.FullName}\n" +
                    $"Логин: {selectedUser.Login}\n" +
                    $"Статус: {selectedUser.Status}\n" +
                    $"Баланс: {selectedUser.Balance}",
                    "Информация о пользователе");
            }
        }

        // Обновление данных (можно вызвать после операций)
        private void RefreshData()
        {
            dgUsers.ItemsSource = null;
            dgUsers.ItemsSource = usersList;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Выход",
               MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
        }
    }
}