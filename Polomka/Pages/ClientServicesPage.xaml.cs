using Polomka.DB;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Polomka.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientServicesPage.xaml
    /// </summary>
    public partial class ClientServicesPage : Page
    {
        private string timeService;
        public static List<Service> services { get; set; }
        public static List<Client> clients { get; set; }
        public ClientServicesPage()
        {
            InitializeComponent();
            services = new List<Service>(DBConnection.polomka.Service);
            clients = new List<Client>(DBConnection.polomka.Client);

            this.DataContext = this;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void timeTb_TextChanged(object sender, TextChangedEventArgs e)
        {

            string text = timeTb.Text;
            string filteredText = new string(text.Where(c => char.IsDigit(c) || c == ':').ToArray());
            int count = text.Count(c => c == ':');

            if (filteredText.Length > 0)
            {
                //удаление повторного :
                if (filteredText.Contains("::"))
                {
                    filteredText = filteredText.Substring(0, filteredText.Length - 1);
                }

                if (filteredText.Length >= 3)
                {
                    //запрет на ввод больше 3 чисел подряд
                    for (int i = 0; i <= filteredText.Length - 3; i++)
                    {
                        if (char.IsDigit(filteredText[i]) && char.IsDigit(filteredText[i + 1]) && char.IsDigit(filteredText[i + 2]))
                        {
                            filteredText = filteredText.Remove(i + 2, 1);
                            break;
                        }
                    }
                    //запрет на ввод больше 5 знаков (2 цифры - часов, 2 цифры - минут, 1 - :)
                    if (filteredText.Length > 5)
                    {
                        filteredText.Substring(0, 5);
                    }

                    string[] parts = filteredText.Split(':');
                    // Проверка и исправление первой части (часы)
                    if (parts.Length > 0 && parts[0].Length > 0)
                    {
                        if (parts[0].Length == 1)
                        {
                            parts[0] = "0" + parts[0]; // Добавить 0, если введено одно число
                        }
                        // Проверка на корректное значение часов
                        if (int.TryParse(parts[0], out int hours))
                        {
                            if (hours < 0 || hours > 23)
                            {
                                parts[0] = "00"; // Установить часы в 00, если некорректное значение
                            }
                        }
                        filteredText = parts[0] + ":";

                        if (parts.Length > 1 && parts[1].Length > 0)
                        {
                            // Проверка на корректное значение минут
                            if (int.TryParse(parts[1], out int minutes))
                            {
                                if (minutes < 0 || minutes > 59)
                                {
                                    parts[1] = "00"; // Установить минуты в 00, если некорректное значение
                                }
                            }
                            filteredText += parts[1];
                        }
                    }
                }
            }
            timeTb.Text = filteredText;
            timeService = filteredText;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            var serv = servCb.SelectedItem as Service;
            var client = clientsCb.SelectedItem as Client;
            ClientService clientService = new ClientService();
            if (timeTb.Text == "" || dateDp.SelectedDate == null || client == null || serv == null)
            {
                MessageBox.Show("Вы заполнили не все данные!", "Ошибка заполнения данных", MessageBoxButton.OK, MessageBoxImage.Error); ;
            }
            else
            {
                //Service service1 = new Service();
                clientService.ServiceID = serv.ID;
                clientService.ClientID = client.ID;
                TimeSpan time = DateTime.Parse(timeService).TimeOfDay;

                var startTime = dateDp.SelectedDate.ToString().Substring(0, 11) + time.ToString();
                clientService.StartTime = DateTime.Parse(startTime.Trim());

                DBConnection.polomka.ClientService.Add(clientService);
                DBConnection.polomka.SaveChanges();
                MessageBox.Show($"Добавлена запись на услугу \"{serv.Title.Trim()}\" клиента {client.FirstName.Trim()} {client.LastName.Trim()[0]}.{client.Patronymic.Trim()[0]}. на {startTime}");
                NavigationService.Navigate(new MainPage());
            }
        }
    }
}
