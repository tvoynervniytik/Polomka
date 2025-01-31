using Polomka.DB;
using Polomka.Pages;
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

namespace Polomka.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditSClientWindow.xaml
    /// </summary>
    public partial class EditSClientWindow : Page
    {
        public EditSClientWindow(Client client)
        {
            InitializeComponent();
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientsPage());
        }

        private void photoBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void photoDelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void photoDopBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void photoDopDelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
