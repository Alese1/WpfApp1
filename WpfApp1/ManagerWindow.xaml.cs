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
    /// Логика взаимодействия для ManagerWindow.xaml
    /// </summary>
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();
            Title = "Окно модератора";
            InitializeData();


        }
        // Модель данных для пользователя
        public class User
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public string Login { get; set; }
            public string Email { get; set; }
            public string Status { get; set; }
            public decimal Balance { get; set; }
        }

        // Список пользователей
        private List<User> users;
        private List<User> filteredUsers;

        private void InitializeData()
        {

            users = new List<User>
            {
                new User { Id = 1, FullName = "Иванов Иван Иванович", Login = "ivanov", Email = "ivanov@mail.ru", Status = "Активен", Balance = 25000.50m },
                new User { Id = 2, FullName = "Петров Петр Петрович", Login = "petrov", Email = "petrov@mail.ru", Status = "Активен", Balance = 15000.00m },
                new User { Id = 3, FullName = "Сидорова Мария Сергеевна", Login = "sidorova", Email = "sidorova@mail.ru", Status = "Активен", Balance = 32000.75m },
                new User { Id = 4, FullName = "Козлов Алексей Владимирович", Login = "kozlov", Email = "kozlov@mail.ru", Status = "Заблокирован", Balance = 5000.00m },
                new User { Id = 5, FullName = "Николаева Екатерина Дмитриевна", Login = "nikolaeva", Email = "nikolaeva@mail.ru", Status = "Активен", Balance = 18000.25m },
                new User { Id = 6, FullName = "Смирнов Дмитрий Олегович", Login = "smirnov", Email = "smirnov@mail.ru", Status = "Заблокирован", Balance = 12000.00m }
            };

            filteredUsers = new List<User>(users);
            dgUsers.ItemsSource = users;
        }

        private void UpdateStatistics()
        {
            // Обновление статистики пользователей
            txtTotalUsers.Text = users.Count.ToString();
            txtActiveUsers.Text = users.Count(u => u.Status == "Активен").ToString();
            txtBlockedUsers.Text = users.Count(u => u.Status == "Заблокирован").ToString();
        }

        // Обработчик выхода из системы
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

        // Обработчик поиска пользователей
        private void btnSearchUsers_Click(object sender, RoutedEventArgs e)
        {
            ApplyFilters();
        }

        private void ApplyFilters()
        {
            string searchText = txtSearchUsers.Text.ToLower();
            string filterStatus = ((ComboBoxItem)cmbUserFilter.SelectedItem).Content.ToString();

            // Применение фильтров
            filteredUsers = users.Where(user =>
                (user.FullName.ToLower().Contains(searchText) ||
                 user.Login.ToLower().Contains(searchText) ||
                 user.Email.ToLower().Contains(searchText)) &&
                (filterStatus == "Все пользователи" ||
                 (filterStatus == "Активные" && user.Status == "Активен") ||
                 (filterStatus == "Заблокированные" && user.Status == "Заблокирован"))
            ).ToList();


            UpdateStatistics();
        }

        // Обработчик добавления пользователя
        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            // Простое диалоговое окно для добавления пользователя
            var dialog = new Window
            {
                Title = "Добавить пользователя",
                Width = 400,
                Height = 350,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };

            var stackPanel = new StackPanel { Margin = new Thickness(20) };

            // Поля для ввода данных
            AddInputField(stackPanel, "ФИО:", "fullName");
            AddInputField(stackPanel, "Логин:", "login");
            AddInputField(stackPanel, "Email:", "email");
            AddInputField(stackPanel, "Баланс:", "balance");

            // Выбор статуса
            var statusPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 10, 0, 20) };
            statusPanel.Children.Add(new TextBlock { Text = "Статус:", Width = 100, VerticalAlignment = VerticalAlignment.Center });
            var statusComboBox = new ComboBox { Width = 200 };
            statusComboBox.Items.Add("Активен");
            statusComboBox.Items.Add("Заблокирован");
            statusComboBox.SelectedIndex = 0;
            statusPanel.Children.Add(statusComboBox);
            stackPanel.Children.Add(statusPanel);

            // Кнопки
            var buttonsPanel = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Center };

            var addButton = new Button
            {
                Content = "Добавить",
                Background = System.Windows.Media.Brushes.Green,
                Foreground = System.Windows.Media.Brushes.White,
                Padding = new Thickness(15, 8, 15, 8),
                Margin = new Thickness(0, 0, 10, 0)
            };

            var cancelButton = new Button
            {
                Content = "Отмена",
                Background = System.Windows.Media.Brushes.Gray,
                Foreground = System.Windows.Media.Brushes.White,
                Padding = new Thickness(15, 8, 15, 8)
            };

            addButton.Click += (s, args) =>
            {
                // Простая валидация и добавление пользователя
                var fullName = ((TextBox)FindElement(stackPanel, "fullName")).Text;
                var login = ((TextBox)FindElement(stackPanel, "login")).Text;
                var email = ((TextBox)FindElement(stackPanel, "email")).Text;
                var balanceText = ((TextBox)FindElement(stackPanel, "balance")).Text;

                if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(login))
                {
                    MessageBox.Show("Заполните обязательные поля: ФИО и Логин");
                    return;
                }

                if (!decimal.TryParse(balanceText, out decimal balance))
                {
                    balance = 0;
                }

                var newUser = new User
                {
                    Id = users.Count + 1,
                    FullName = fullName,
                    Login = login,
                    Email = email,
                    Status = statusComboBox.SelectedItem.ToString(),
                    Balance = balance
                };

                users.Add(newUser);
                ApplyFilters();
                MessageBox.Show("Пользователь успешно добавлен!");
                dialog.Close();
            };

            cancelButton.Click += (s, args) => dialog.Close();

            buttonsPanel.Children.Add(addButton);
            buttonsPanel.Children.Add(cancelButton);
            stackPanel.Children.Add(buttonsPanel);

            dialog.Content = stackPanel;
            dialog.ShowDialog();
        }

        // Вспомогательный метод для добавления полей ввода
        private void AddInputField(StackPanel panel, string label, string tag)
        {
            var fieldPanel = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 5, 0, 5) };
            fieldPanel.Children.Add(new TextBlock { Text = label, Width = 100, VerticalAlignment = VerticalAlignment.Center });
            var textBox = new TextBox { Width = 200, Tag = tag };
            fieldPanel.Children.Add(textBox);
            panel.Children.Add(fieldPanel);
        }

        // Вспомогательный метод для поиска элемента по тегу
        private FrameworkElement FindElement(StackPanel panel, string tag)
        {
            foreach (var child in panel.Children)
            {
                if (child is StackPanel childPanel)
                {
                    foreach (var innerChild in childPanel.Children)
                    {
                        if (innerChild is TextBox textBox && textBox.Tag?.ToString() == tag)
                            return textBox;
                    }
                }
            }
            return null;
        }

        // Обработчик выбора пользователя в DataGrid
        private void dgUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgUsers.SelectedItem is User selectedUser)
            {
                // Можно добавить дополнительную логику при выборе пользователя
                // Например, отображение подробной информации или контекстное меню

                // Просто показываем информацию о выбранном пользователе
                string message = $"Выбран пользователь:\n" +
                               $"ID: {selectedUser.Id}\n" +
                               $"ФИО: {selectedUser.FullName}\n" +
                               $"Логин: {selectedUser.Login}\n" +
                               $"Email: {selectedUser.Email}\n" +
                               $"Статус: {selectedUser.Status}\n" +
                               $"Баланс: {selectedUser.Balance:N2} ₽";

                // MessageBox.Show(message, "Информация о пользователе");

                // Или можно обновлять какую-то панель с деталями вместо MessageBox
            }
        }

        // Обработчик для текстового поля поиска (можно добавить поиск при вводе)
        private void txtSearchUsers_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Раскомментируйте для поиска при вводе текста
            // ApplyFilters();
        }

        // Обработчик для комбобокса фильтра (можно добавить автоматическое применение фильтра)
        private void cmbUserFilter_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Раскомментируйте для автоматического применения фильтра при изменении выбора
            // ApplyFilters();
        }

    }
}