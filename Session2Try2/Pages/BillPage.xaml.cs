using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Логика взаимодействия для BillPage.xaml
    /// </summary>
    public partial class BillPage : Page
    {
        public BillPage()
        {
            InitializeComponent();
        }

        private void SchetOrg_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrgBillPage());
        }

        private void SchetClient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ClientBillPage());
        }
    }
}
