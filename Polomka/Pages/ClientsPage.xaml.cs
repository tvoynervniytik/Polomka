using Polomka.DB;
using Polomka.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {
        private List<Client> clients { get; set; }
        public ClientsPage()
        {
            InitializeComponent();
            Refresh();
            DataContext = this;
        }

        

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }
        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientsAddPage());
        }
        private void Refresh()
        {
            
            clients = new List<Client>(DBConnection.polomka.Client);
            if (dateCb.SelectedItem != null)
            {
                if (dateCb.SelectedIndex == 0)
                    clients.Sort((c1,c2) => c1.RegistrationDate.CompareTo(c2.RegistrationDate));
                else if (dateCb.SelectedIndex == 1)
                    clients.Sort((c1, c2) => c2.RegistrationDate.CompareTo(c1.RegistrationDate));
                else
                    clients = new List<Client>(DBConnection.polomka.Client);
            }
            if (surnameTb.Text != "")
                clients = clients.Where(c => c.FirstName.ToLower().StartsWith(surnameTb.Text.Trim().ToLower())).ToList();
            if (maleChb.IsChecked == false)
            {
                clients = clients.Where(i => i.GenderCode != "1").ToList();
            }
            if (femaleChb.IsChecked == false)
            {
                clients = clients.Where(i => i.GenderCode != "2").ToList();
            }
            clientsLv.ItemsSource = clients;

        }

        private void HLDelete_Click(object sender, RoutedEventArgs e)
        {
            var clientDel = (sender as Hyperlink).DataContext as Client;
            List<ClientService> client = new List<ClientService>(DBConnection.polomka.ClientService);
            if (client.Where(i => i.ID == clientDel.ID).Count() == 0)
            {
                {
                    try
                    {
                        var result = MessageBox.Show("Вы действительно хотите удалить данного клиента?", "Подтверждение удаления клиента",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            DBConnection.polomka.Client.Remove(clientDel);
                            DBConnection.polomka.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    Refresh();
                }
            }
            else
                MessageBox.Show("Невозможно удалить клиента: есть записи", "Ошибка удаления клиента", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void HLEdit_Click(object sender, RoutedEventArgs e)
        {
            var seclientDEdit = (sender as Hyperlink).DataContext as Client;

            NavigationService.Navigate(new EditSClientWindow(seclientDEdit));

            Refresh();
        }

        private void surnameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
              Refresh();
        }

        private void dateTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }


        private void maleChb_Click_1(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void femaleChb_Click_1(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }
    }
}
