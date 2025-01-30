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
        private List<Client> clients {  get; set; }
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

        }

        private void surnameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyLetters();
        }
        private void OnlyLetters()
        {
            string text = surnameTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsLetter(c)).ToArray());
            surnameTb.Text = filteredText;
        }

        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyLetters();
        }

        private void patronymicTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnlyLetters();
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
    }
}
