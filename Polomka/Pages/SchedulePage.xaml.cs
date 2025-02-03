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
    public partial class SchedulePage : Page
    {
        private List<ClientService> clientServices { get; set; }
        private List<Service> services { get; set; }
        private List<Client> clients { get; set; }
        public SchedulePage()
        {
            InitializeComponent();
            services = new List<Service>(DBConnection.polomka.Service);
            clients = new List<Client>(DBConnection.polomka.Client);
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
            DBConnection.page = 1;
            NavigationService.Navigate(new ClientServicesPage());
        }
        private void Refresh()
        {
            clientServices = new List<ClientService>(DBConnection.polomka.ClientService);

            if (dateCb.SelectedItem != null)
            {
                if (dateCb.SelectedIndex == 0)
                    clientServices.Sort((c1, c2) => c1.StartTime.CompareTo(c2.StartTime));
                else if (dateCb.SelectedIndex == 1)
                    clientServices.Sort((c1, c2) => c2.StartTime.CompareTo(c1.StartTime));
                else
                    clientServices = new List<ClientService>(DBConnection.polomka.ClientService);
            }

            clientsLv.ItemsSource = clientServices;
        }

  
        private void dateTb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
            DBConnection.page = 1;
        }


      
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Refresh();
        }

        private void clientsLv_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var clientService1 = clientsLv.SelectedItem as ClientService;
            NavigationService.Navigate(new AddSchedule(clientService1));
        }
    }
}
