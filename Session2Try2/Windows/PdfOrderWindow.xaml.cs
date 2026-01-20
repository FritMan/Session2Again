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
using System.Windows.Shapes;
using static Session2Try2.Classes.Helper;

namespace Session2Try2.Windows
{
    /// <summary>
    /// Логика взаимодействия для PdfOrderWindow.xaml
    /// </summary>
    public partial class PdfOrderWindow : Window
    {
        private Order _order = new Order();
        public PdfOrderWindow(Order order)
        {
            InitializeComponent();
            _order = order;
            OrderSp.DataContext = order;
            ServiceDg.ItemsSource = order.OrderService.ToList();
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintQueue = new System.Printing.PrintQueue(new System.Printing.PrintServer(), "Microsoft Print to PDF");
            printDialog.PrintVisual(OrderSp, "");
            ConBase64();
            Close();
        }

        private void ConBase64()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = ".txt|*.txt";
            saveFileDialog.DefaultExt = ".txt";
            string base64;

                base64 = "Дата создания;Код;Организация;ФИО;Дата рождения;\n";

                if(_order.Client.Organisation == null)
                {
                    base64 += $"{_order.DateCreate.Date};{_order.Code};' ';{_order.Client.Fio};{_order.Client.DateBirth.Date}\n";
                }
                else
                {
                    base64 += $"{_order.DateCreate.Date};{_order.Code};{_order.Client.Organisation.Name};{_order.Client.Fio};{_order.Client.DateBirth.Date}\n";
                }

                base64 += "Перечень услуг через ;\n";
                base64 += "Название;Цена\n";

                foreach (var el in _order.OrderService.ToList())
                {
                    base64 += $"{el.Service.Name};{el.Service.Price}\n";
                }

            if (saveFileDialog.ShowDialog() == true)
            {

                File.WriteAllText(saveFileDialog.FileName, Convert.ToBase64String(Encoding.UTF8.GetBytes(base64)));
            }
        }
    }
}
