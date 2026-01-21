using MessagingToolkit.QRCode.Codec;
using MessagingToolkit.QRCode.Codec.Data;
using Microsoft.Win32;
using Session2Try2.Data;
using Session2Try2.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
using static Session2Try2.Classes.Helper;

namespace Session2Try2.Pages
{
    /// <summary>
    /// Логика взаимодействия для CodeWastePage.xaml
    /// </summary>
    public partial class CodeWastePage : Page
    {

        private ObservableCollection<Service> services = new ObservableCollection<Service>();
        public CodeWastePage()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            Db.Client.Load();
            Db.Service.Load();

            ServiceCb.ItemsSource = Db.Service.ToList();
            ClientCb.ItemsSource = Db.Client.ToList();

            ServiceDg.ItemsSource = services;

            var lastorder = Db.Order.OrderByDescending(el => el.Code).FirstOrDefault();

            var neword = new Order();

            neword.StatusId = 3;
            neword.DateCreate = DateTime.Now;
            neword.OrderService = new List<OrderService>();
            neword.IsDeleted = false;

            if(lastorder != null)
            {
                neword.Code = ((decimal.Parse(lastorder.Code) + 1)).ToString();
            }
            else
            {
                neword.Code = "1";
            }

            OrderSp.DataContext = neword;
        }

        private void CodeTb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                VisibSp.Visibility = Visibility.Visible;
                CodeTb.IsReadOnly = false;
            }
        }
        private void QrBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == true)
            {
                QRCodeDecoder decoder = new QRCodeDecoder();
                Bitmap bitmap = (Bitmap)Bitmap.FromFile(openFileDialog.FileName);
                QRCodeBitmapImage qRCode = new QRCodeBitmapImage(bitmap);
                var res = decoder.Decode(qRCode);
                MessageBox.Show(res);
            }



        }

        private void SearchClientTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(string.IsNullOrEmpty(SearchClientTb.Text))
            {
                ReadactBtn.Visibility
                     = Visibility.Collapsed;
            }
            else
            {
                ClientCb.ItemsSource = Db.Client.Where(el => el.Name.Contains(SearchClientTb.Text) || el.Surname.Contains(SearchClientTb.Text) || el.Patronimic.Contains(SearchClientTb.Text)
|| el.Phone.Contains(SearchClientTb.Text) || el.Email.Contains(SearchClientTb.Text) || el.Organisation.Name.Contains(SearchClientTb.Text) || el.DateBirth.ToString().Contains(SearchClientTb.Text)).ToList();

                ReadactBtn.Visibility = Visibility.Visible;
            }


        }

        private void ServiceSearchTb_TextChanged(object sender, TextChangedEventArgs e)
        {
            ServiceCb.ItemsSource = Db.Service.Where(el => el.Name.Contains(ServiceSearchTb.Text) || el.Price.ToString().Contains(ServiceSearchTb.Text) || el.Code.ToString().Contains(ServiceSearchTb.Text) || el.Duration.ToString().Contains(ServiceSearchTb.Text)).ToList();
        }

        private void AddClientBtn_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow(-1);
            addClientWindow.ShowDialog();
            LoadData();
        }

        private void AddServiceBtn_Click(object sender, RoutedEventArgs e)
        {
            var selected_service = ServiceCb.SelectedItem as Service;

            if (selected_service == null)
            {
                return;
            }
            else
            {
                services.Add(selected_service);
                var order = OrderSp.DataContext as Order;
                order.OrderService.Add(new OrderService { StatusId = 3, ServiceId= selected_service.Id});
                FinalPriceTb.Text = ((decimal.Parse(FinalPriceTb.Text) + selected_service.Price).ToString());
            }


        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            Db.Order.Add(OrderSp.DataContext as Order);
            Db.SaveChanges();

            PdfOrderWindow pdfOrderWindow = new PdfOrderWindow(OrderSp.DataContext as Order);
            pdfOrderWindow.Show();

            NavigationService.GoBack();
        }

        private void ReadactBtn_Click(object sender, RoutedEventArgs e)
        {
            var user = ClientCb.SelectedItem as Client;
            if (user == null)
            {
                return ;
            }

            AddClientWindow addClientWindow = new AddClientWindow(user.Id);
            addClientWindow.ShowDialog();
            LoadData();
            ReadactBtn.Visibility = Visibility.Collapsed;
        }
    }
}
