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
    /// Логика взаимодействия для EditServicePage.xaml
    /// </summary>
    public partial class EditServicePage : Page
    {
        public Service serv {  get; set; }
        public Binary ImagePath { get; set; }
        public static List<ServicePhoto> servicePhotos { get; set; }
        public static List<Service> services { get; set; }
        public EditServicePage(Service service)
        {
            InitializeComponent();
            serv = service;
            ImagePath = service.MainImagePath;
            //MessageBox.Show($"{ImagePath}");
            services = new List<Service>(DBConnection.polomka.Service);

            idTb.Text = serv.ID.ToString();
            nameTb.Text = serv.Title;
            int cost = (int)serv.Cost;
            costTb.Text = cost.ToString();
            discountTb.Text = serv.Discount.ToString();
            descriptionTb.Text = serv.Description.ToString();
            durationTb.Text = serv.DurationInMinutes.ToString();

            servicePhotos = new List<ServicePhoto>(DBConnection.polomka.ServicePhoto.Where(p => p.ServiceID == serv.ID));
            photosLv.ItemsSource = servicePhotos;
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
          
            img.Source = null;
        }

        private void addBtn_Click(object sender, RoutedEventArgs e)
        {
            if (nameTb.Text == "" || durationTb.Text == "" || discountTb.Text == "" || descriptionTb.Text == "" || costTb.Text == "")
                MessageBox.Show("Заполните все данные", "ошибка ввода данных", MessageBoxButton.OK, MessageBoxImage.Error);
            else
            {
                try
                {
                    serv.Title = nameTb.Text.Trim();
                    serv.Description = descriptionTb.Text.Trim();
                    serv.Cost = decimal.Parse(costTb.Text.Trim());
                    serv.DurationInMinutes = int.Parse(durationTb.Text.Trim());
                    serv.Discount = int.Parse(discountTb.Text.Trim());
                    
                    DBConnection.polomka.SaveChanges();
                    NavigationService.Navigate(new ServicesPage());
                    //DBConnection.DisposeAndCreate();
                }
                catch(Exception ex) { MessageBox.Show(ex.Message); }
            }
        }

        private void photoDopBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "*.png|*.png|*.jpeg|*.jpeg|*.jpg|*.jpg"
            };
            if (openFileDialog.ShowDialog().GetValueOrDefault())
            {
                ServicePhoto servicePhoto = new ServicePhoto();
                servicePhoto.ServiceID = serv.ID;
                servicePhoto.PhotoPath = File.ReadAllBytes(openFileDialog.FileName);

                DBConnection.polomka.ServicePhoto.Add(servicePhoto);
                DBConnection.polomka.SaveChanges();
                servicePhotos = new List<ServicePhoto>(DBConnection.polomka.ServicePhoto.Where(i=> i.ServiceID == serv.ID));
                photosLv.ItemsSource = servicePhotos;
            }
        }

        private void photoDopDelBtn_Click(object sender, RoutedEventArgs e)
        {
            if (photosLv.SelectedItem != null)
            {
                DBConnection.polomka.ServicePhoto.Remove(photosLv.SelectedItem as ServicePhoto);
                DBConnection.polomka.SaveChanges();
                servicePhotos = new List<ServicePhoto>(DBConnection.polomka.ServicePhoto.Where(p => p.ServiceID == serv.ID));
                photosLv.ItemsSource = servicePhotos;
            }
        }

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
