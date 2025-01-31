using Microsoft.Win32;
using Polomka.DB;
using Polomka.Pages;
using System;
using System.Collections.Generic;
using System.Data.Linq;
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

namespace Polomka.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditSClientWindow.xaml
    /// </summary>
    public partial class EditSClientWindow : Page
    {
        public Binary ImagePath { get; set; }
        public static Client currentClient {  get; set; }
        public EditSClientWindow(Client client)
        {
            InitializeComponent();
            
            ImagePath = client.Image;

            currentClient = client;

            emailTb.Text = client.Email;
            nameTb.Text = client.LastName;
            surnameTb.Text = client.FirstName;
            patronymicTb.Text = client.Patronymic;
            phoneTb.Text = client.Phone;
            Dp.SelectedDate = client.Birthday;
            genderCb.SelectedIndex = int.Parse(client.GenderCode) - 1;
            this.DataContext = this;
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Dp.SelectedDate == null || surnameTb.Text.Trim() == "" || nameTb.Text.Trim() == "" || patronymicTb.Text.Trim() == "" ||
                emailTb.Text.Trim() == "" || phoneTb.Text.Trim() == "" || genderCb.SelectedItem == null)
                MessageBox.Show("Заполните все поля!", "", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                currentClient.FirstName = surnameTb.Text.Trim();
                currentClient.LastName = nameTb.Text.Trim();
                currentClient.Patronymic = patronymicTb.Text.Trim();
                currentClient.Birthday = Dp.SelectedDate;
                currentClient.Email = emailTb.Text.Trim();
                currentClient.Phone = phoneTb.Text.Trim();
                currentClient.RegistrationDate = DateTime.Now;
                if (genderCb.SelectedIndex == 0)
                    currentClient.GenderCode = "1";
                else if (genderCb.SelectedIndex == 1)
                    currentClient.GenderCode = "2";
                DBConnection.polomka.SaveChanges();
                MessageBox.Show($"Пользователь {currentClient.FirstName} {currentClient.LastName[0]}. {currentClient.Patronymic[0]}. успешно изменен", "", MessageBoxButton.OK, MessageBoxImage.Information);
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
                currentClient.Image = File.ReadAllBytes(openFileDialog.FileName);
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }
        
       
        private void photoDelBtn_Click(object sender, RoutedEventArgs e)
        {
            currentClient.Image = null;
            img.Source = null;
            photoDelBtn.Visibility = Visibility.Hidden;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            DBConnection.DisposeAndCreate();
            NavigationService.Navigate(new ClientsPage());
        }
    }
}
