using Microsoft.Win32;
using Polomka.DB;
using Polomka.Pages;
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

namespace Polomka.Windows
{
    /// <summary>
    /// Логика взаимодействия для EditServicePage.xaml
    /// </summary>
    public partial class ServiceAddPage : Page
    {
        private static Service serv;
        private static List<Service> services { get; set; }
        private static List<ServicePhoto> servicePhotos { get; set; }
        public ServiceAddPage()
        {
            InitializeComponent();
            services = new List<Service>(DBConnection.polomka.Service);
            serv = new Service();
            this.DataContext = this;
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            DBConnection.DisposeAndCreate();
            NavigationService.Navigate(new ServicesPage());
        }
        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new MainPage());
        }

        private void photoBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                serv.MainImagePath = File.ReadAllBytes(openFileDialog.FileName);
                img.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            }
        }

        private void photoDelBtn_Click(object sender, RoutedEventArgs e)
        {
            serv.MainImagePath = null;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nameTb.Text==""||durationTb.Text==""||discountTb.Text==""||descriptionTb.Text==""||costTb.Text=="")
                MessageBox.Show("Заполните все данные","ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    serv.Title = nameTb.Text.Trim();
                    serv.Description = descriptionTb.Text.Trim();
                    serv.Cost = int.Parse(costTb.Text.Trim());
                    serv.DurationInMinutes = int.Parse(durationTb.Text.Trim());
                    serv.Discount = int.Parse(discountTb.Text.Trim());

                    DBConnection.polomka.Service.Add(serv);
                    DBConnection.polomka.SaveChanges();
                    NavigationService.Navigate(new ServicesPage());
                    DBConnection.DisposeAndCreate();
                }
                catch (Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        //private void photoDopBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    OpenFileDialog openFileDialog = new OpenFileDialog()
        //    {
        //        Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
        //    };
        //    if (openFileDialog.ShowDialog().GetValueOrDefault())
        //    {
        //        ServicePhoto servicePhoto = new ServicePhoto();
        //        servicePhoto.ServiceID = serv.ID;
        //        servicePhoto.PhotoPath = File.ReadAllBytes(openFileDialog.FileName);

        //        DBConnection.polomka.ServicePhoto.Add(servicePhoto);
        //        servicePhotos = new List<ServicePhoto>( DBConnection.polomka.ServicePhoto);
        //        servicePhotos = servicePhotos.Where(i => i.ServiceID > servicePhotos.Last().ServiceID).ToList();
        //        servicePhotos.Add(servicePhoto);
        //        //servicePhotos = new List<ServicePhoto>(DBConnection.polomka.ServicePhoto.Where(p => p.ServiceID == serv.ID));
        //        photosLv.ItemsSource = servicePhotos;
        //    }
        //}

        //private void photoDopDelBtn_Click(object sender, RoutedEventArgs e)
        //{
        //    if (photosLv.SelectedItem != null)
        //    {
        //        DBConnection.polomka.ServicePhoto.Remove(photosLv.SelectedItem as ServicePhoto);

        //        servicePhotos = new List<ServicePhoto>(DBConnection.polomka.ServicePhoto.Where(p => p.ServiceID == serv.ID));
        //        photosLv.ItemsSource = servicePhotos;

        //    }
        //}

        private void nameTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = nameTb.Text;
            string filteredText = new string(text.Where(c => char.IsLetter(c) || c == ' ').ToArray());
            nameTb.Text = filteredText;
        }

        private void durationTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = durationTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsDigit(c)).ToArray());
            durationTb.Text = filteredText;
        }

        private void costTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = costTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsDigit(c)).ToArray());
            costTb.Text = filteredText;
        }

        private void discountTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            string text = discountTb.Text.Trim();
            string filteredText = new string(text.Where(c => char.IsDigit(c)).ToArray());
            discountTb.Text = filteredText;
        }

    }
}
