using Polomka.DB;
using Polomka.Windows;
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
    /// Логика взаимодействия для ServicesPage.xaml
    /// </summary>
    public partial class ServicesPage : Page
    {
        public static List<Service> services { get; set; }
        public ServicesPage()
        {
            InitializeComponent();
            Refresh();
            this.DataContext = services;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Refresh()
        {
            services = new List<Service>(DBConnection.polomka.Service);

            if (costCb.SelectedIndex == 0)
                services.Sort((s1, s2) => s1.Cost.CompareTo(s2.Cost));
            else if (costCb.SelectedIndex == 1)
                services.Sort((s1, s2) => s2.Cost.CompareTo(s1.Cost));
            else
                services = new List<Service>(DBConnection.polomka.Service);

            if (saleCb.SelectedIndex == 0)
                services = services.Where(i=> i.Discount >= 0 && i.Discount<10).ToList();
            else if (saleCb.SelectedIndex == 1)
                services = services.Where(i=> i.Discount >= 10 && i.Discount<30).ToList();
            else if (saleCb.SelectedIndex == 2)
                services = services.Where(i=> i.Discount >= 30 && i.Discount<70).ToList();
            else if (saleCb.SelectedIndex == 3)
                services = services.Where(i=> i.Discount >= 70 && i.Discount<100).ToList();

            services = services.Where(i => i.Title.ToLower().StartsWith(nameTb.Text.ToLower().Trim())).ToList();

            servicesLv.ItemsSource = services;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ServiceAddPage());
        }

        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            Refresh();
        }

        private void saleCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void costCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh();
        }

        private void delSaleBtn_Click(object sender, RoutedEventArgs e)
        {
            saleCb.SelectedItem = null;
            Refresh();
        }

        private void HLDelete_Click(object sender, RoutedEventArgs e)
        {
            var serviceDel = (sender as Hyperlink).DataContext as Service;
            List<ClientService> clientServ= new List<ClientService>(DBConnection.polomka.ClientService);
            if (clientServ.Where(i => i.ID == serviceDel.ID).Count() == 0)
            {
                {
                    try
                    {
                        var result = MessageBox.Show("Вы действительно хотите удалить данного клиента?", "Подтверждение удаления клиента",
                            MessageBoxButton.YesNo, MessageBoxImage.Warning);
                        if (result == MessageBoxResult.Yes)
                        {
                            DBConnection.polomka.Service.Remove(serviceDel);
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
            var serviceEdit = (sender as Hyperlink).DataContext as Service;

            NavigationService.Navigate(new EditServicePage(serviceEdit));

            Refresh();
        }
    }
}
