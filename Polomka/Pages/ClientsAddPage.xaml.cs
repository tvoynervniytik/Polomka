using Microsoft.Win32;
using Polomka.DB;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Логика взаимодействия для ClientsAddPage.xaml
    /// </summary>
    public partial class ClientsAddPage : Page
    {
        //public string ImagePath { get; set; }
        //public string ImageDirectory { get; set; }
        private Client client = new Client();
        private List<Client> clients { get; set; }
        public ClientsAddPage()
        {
            InitializeComponent();
            clients = new List<Client>(DBConnection.polomka.Client);
            
            this.DataContext = clients;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientsPage());
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Dp.SelectedDate == null || surnameTb.Text.Trim() == "" || nameTb.Text.Trim() == "" || patronymicTb.Text.Trim() == "" ||
                emailTb.Text.Trim() == "" || phoneTb.Text.Trim()=="" || genderCb.SelectedItem==null)
                MessageBox.Show("Заполните все поля!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                client.FirstName = surnameTb.Text.Trim();
                client.LastName = nameTb.Text.Trim();
                client.Patronymic = patronymicTb.Text.Trim();
                client.Birthday = Dp.SelectedDate;
                client.Email = emailTb.Text.Trim();
                client.Phone = phoneTb.Text.Trim();
                client.RegistrationDate = DateTime.Now;
                if (genderCb.SelectedIndex == 0)
                    client.GenderCode = "1";
                else if (genderCb.SelectedIndex == 1)
                    client.GenderCode = "2";
                DBConnection.polomka.Client.Add(client);
                DBConnection.polomka.SaveChanges();
                MessageBox.Show($"Пользователь {client.FirstName} {client.LastName[0]}. {client.Patronymic[0]}. успешно добавлен в {client.RegistrationDate}", "", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new ClientsPage());
            }
        }
        private void surnameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = surnameTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsLetter(c)).ToArray());
            surnameTb.Text = filteredText;
        }
        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = nameTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsLetter(c)).ToArray());
            nameTb.Text = filteredText;
        }
        private void patronymicTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = patronymicTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsLetter(c)).ToArray());
            patronymicTb.Text = filteredText;
        }
        private void Dp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {

            if (Dp.SelectedDate != null)
            {
                if (Dp.SelectedDate > DateTime.Now)
                {
                    MessageBox.Show("Дата не позже нынешней", "Ошибка даты", MessageBoxButton.OK, MessageBoxImage.Error);
                    Dp.SelectedDate = null;
                }
                else
                {
                    DateTime selectedDate = Dp.SelectedDate.Value;
                    DateTime eighteenYearsAgo = DateTime.Now.AddYears(-18);

                    if (selectedDate > eighteenYearsAgo)
                    {
                        MessageBox.Show("С выбранной даты должно пройти не менее 18 лет", "Ошибка даты", MessageBoxButton.OK, MessageBoxImage.Error);
                        Dp.SelectedDate = null;
                    }
                }
            }
        }
        private void phoneTb_TextChanged(object sender, TextChangedEventArgs e)
        {

            string text = phoneTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsDigit(c)).ToArray());
            phoneTb.Text = filteredText;
        }

        private void photoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };

            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                client.Image = File.ReadAllBytes(openFileDialog.FileName);
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                photoDelBtn.Visibility = Visibility.Visible;
            }
        }

        private void photoDelBtn_Click(object sender, RoutedEventArgs e)
        {
            client.Image = null;
            img.Source = null;
        }
    }
}
