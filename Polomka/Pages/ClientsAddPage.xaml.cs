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
    /// Логика взаимодействия для ClientsAddPage.xaml
    /// </summary>
    public partial class ClientsAddPage : Page
    {
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
    }
}
