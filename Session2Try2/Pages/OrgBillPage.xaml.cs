using Microsoft.Win32;
using Session2Try2.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
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
    /// Логика взаимодействия для OrgBillPage.xaml
    /// </summary>
    public partial class OrgBillPage : Page
    {
        public OrgBillPage()
        {
            InitializeComponent();
            ClientCb.ItemsSource = Db.Organisation.ToList();
            StartDtp.SelectedDate = DateTime.Now;
            EndSp.SelectedDate = DateTime.Now + TimeSpan.FromDays(1);
        }

        private void FillDg()
        {
            var org = ClientCb.SelectedItem as Organisation;
            List<OrderService> orders = new List<OrderService>();

            if (org != null)
            {
                foreach(var el in Db.Order.ToList())
                {
                    if (el.Client.OrganisationId == org.Id && el.DateCreate.Date >= StartDtp.SelectedDate.Value.Date && el.DateCreate <= EndSp.SelectedDate.Value.Date )
                    { 
                        orders.AddRange(el.OrderService);
                    }
                }

                ServiceDG.ItemsSource = orders;
                TotoalLab.Content = orders.Sum(el => el.Service.Price).ToString();
            }
        }
        private void ClientCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDg();
        }

        private void StartDtp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDg();
        }

        private void EndSp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            FillDg();
        }

        private void SavePdfBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintQueue = new PrintQueue(new PrintServer(), "Microsoft Print to PDF");
            printDialog.PrintVisual(BillSp, "");
        }

        private void SaveCsvBtn_Click(object sender, RoutedEventArgs e)
        {
            string Bill = "";

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "*.csv| .csv";

            if(saveFileDialog.ShowDialog() == true)
            {
                var client = ClientCb.SelectedItem as Organisation;

                if (client != null)
                {
                    Bill += $"{client.Name}\n";
                    Bill += $"Период с {StartDtp.SelectedDate.Value.Date.ToString("d")} по {EndSp.SelectedDate.Value.ToString("d")}\n";
                    Bill += "Перечень услуг\n";
                    Bill += "Название услуги; Цена\n";
                    
                    foreach(var el in ServiceDG.ItemsSource)
                    {
                        Bill += $"{(el as OrderService).Service.Name};{(el as OrderService).Service.Price.ToString()}\n";
                    }

                    Bill += $"Финальная стоимость: {TotoalLab.Content}\n";
                    File.WriteAllText(saveFileDialog.FileName, Bill);
                }
            }
        }
    }
}
