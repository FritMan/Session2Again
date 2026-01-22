using Microsoft.Win32;
using Session2Try2.Data;
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
using static Session2Try2.Classes.Helper;

namespace Session2Try2.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientBillPage.xaml
    /// </summary>
    public partial class ClientBillPage : Page
    {
        public ClientBillPage()
        {
            InitializeComponent();
            ClientCb.ItemsSource = Db.Client.ToList();
            StartDp.SelectedDate = DateTime.Now;
            EndDp.SelectedDate = DateTime.Now + TimeSpan.FromDays(1);
        }

        private void LoadData()
        {
            var client = ClientCb.SelectedItem as Client;
            List<OrderService> orders = new List<OrderService>();

            if (client != null)
            {
                foreach(var el in Db.Order.Where(el => el.ClientId == client.Id).ToList())
                {
                    if(el.DateCreate.Date >= StartDp.SelectedDate.Value.Date && el.DateCreate.Date <= EndDp.SelectedDate.Value.Date)
                    {
                        orders.AddRange(el.OrderService);
                    }
                }

                ServiceDg.ItemsSource = orders;
                TotalLab.Content = orders.Sum(el => el.Service.Price).ToString();
            }
        }

        private void StartDp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }

        private void EndDp_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
        }


        private void ClientCb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            LoadData();
            PhoneTb.Text = (ClientCb.SelectedItem as Client).Phone;
        }

        private void PdfBtn_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintQueue = new System.Printing.PrintQueue(new System.Printing.PrintServer(), "Microsoft Print to PDF");
            printDialog.PrintVisual(BillSp, "");
        }

        private void CsvBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.DefaultExt = "csv";
            saveFileDialog.Filter = "*.csv|.csv";

            var client = ClientCb.SelectedItem as Client;

            if (client != null) {
                if (saveFileDialog.ShowDialog() == true)
                {
                    string Bill = "ФИО;Телефон\n";

                    Bill += $"{client.Fio};{client.Phone};\n";
                    Bill += $"Период оплаты\n";
                    Bill += $"С {StartDp.SelectedDate.Value.Date.ToString("d")} по {EndDp.SelectedDate.Value.Date.ToString("d")};\n";
                    Bill += "Перчень услуг\n";
                    Bill += "Название;Цена\n";

                    foreach (var el in ServiceDg.ItemsSource)
                    {
                        var res = el as OrderService;
                        Bill += $"{res.Service.Name};{res.Service.Price}\n";
                    }

                    Bill += $"Финальная сумма: {TotalLab.Content} \n";

                    File.WriteAllText(saveFileDialog.FileName, Bill);
                }
            }
        }
    }
}
